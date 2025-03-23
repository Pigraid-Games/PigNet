#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Security.Cryptography;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.Passive;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Particles;
using MiNET.UI;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Nbt;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
using MiNET.Worlds;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Threading.Tasks;
using Iced.Intel;
using JetBrains.Annotations;
using MiNET.BlockEntities;
using MiNET.Net.RakNet;
using MiNET.Sounds;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace MiNET;

public sealed class Player : Entity, IMcpeMessageHandler
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Player));
	
	// Network & Connection
	private MiNetServer Server { get; set; }
	public IPEndPoint EndPoint { get; private set; }
	public INetworkHandler NetworkHandler { get; set; }
	public bool IsConnected { get; set; }
	public string ServerAddress { get; set; }
	public Session Session { get; set; }
	public CertificateData CertificateData { get; set; }
	
	// Identity & Metadata
	public string Username { get; set; }
	public string DisplayName { get; set; }
	public long ClientId { get; set; }
	public UUID ClientUuid { get; set; }
	public PlayerInfo PlayerInfo { get; set; }
	public Skin Skin { get; set; }
	
	// Inventory & Items
	public PlayerInventory Inventory { get; set; }
	public ItemStackInventoryManager ItemStackInventoryManager { get; set; }
	internal IInventory _openInventory;
	private readonly object _inventorySync = new();
	public bool UsingAnvil { get; set; }
	
	// Movement & Physics
	public long Vehicle { get; set; }
	public float MovementSpeed { get; set; } = 0.1f;
	public double CurrentSpeed { get; private set; }
	public double StartFallY { get; private set; }
	public bool IsFalling { get; set; }
	public bool IsFlyingHorizontally { get; set; }
	public bool IsFlying { get; set; }
	private float _baseSpeed;
	private readonly object _sprintLock = new();
	
	// Gameplay Attributes
	public bool IsSleeping { get; set; }
	public GameMode GameMode { get; set; }
	public bool UseCreativeInventory { get; set; } = true;
	public bool EnableCommands { get; set; } = Config.GetProperty("EnableCommands", true);
	public PermissionLevel PermissionLevel { get; set; } = PermissionLevel.Operator;
	public int CommandPermission { get; set; } = (int)Net.CommandPermission.Normal;
	public ActionPermissions ActionPermissions { get; set; } = ActionPermissions.Default;
	
	// Chunk & World Interaction
	private readonly Dictionary<ChunkCoordinates, McpeWrapper> _chunksUsed = new();
	private ChunkCoordinates _currentChunkPosition;
	public int MaxViewDistance { get; set; } = 22;
	public int MoveRenderDistance { get; set; } = 1;
	public int ChunkRadius { get; private set; } = -1;
	private readonly object _sendChunkSync = new();
	private readonly object _sendMoveListSync = new();
	private DateTime _lastMoveListSendTime = DateTime.UtcNow;
	public PlayerLocation SpawnPosition { get; set; }
	
	// Effects & Attributes
	public ConcurrentDictionary<EffectType, Effect> Effects { get; set; } = new();
	public HungerManager HungerManager { get; set; }
	public ExperienceManager ExperienceManager { get; set; }
	public DamageCalculator DamageCalculator { get; set; } = new();
	
	// Status & Permissions
	public bool IsSpectator { get; set; }
	public bool IsWorldImmutable { get; set; }
	public bool IsWorldBuilder { get; set; }
	public bool IsMuted { get; set; }
	public bool IsNoPvp { get; set; }
	public bool IsNoPvm { get; set; }
	public bool IsNoMvp { get; set; }
	public bool IsNoClip { get; set; }
	public bool IsInvicible { get; set; } = false;
	
	// Combat
	public Entity LastAttackTarget { get; set; }
	
	//Packets & Forms
	private Form CurrentForm { get; set; }
	public List<Popup> Popups { get; set; } = [];
	
	// Player Pack Data (resource Packs & Texture Packs)
	public TexturePackInfos PlayerPackData { get; set; } = [];
	public ResourcePackInfos PlayerPackDataB { get; set; } = [];
	public readonly Dictionary<string, PlayerPackMapData> PlayerPackMap = new();
	
	// Sync & Miscellaneous
	public long CurrentTick { get; set; }
	public AuthInputFlags LastAuthInputFlag { get; set; }
	public static Dictionary<string, long> Pings { get; set; } = new();
	private readonly object _moveSyncLock = new();
	private readonly object _disconnectSync = new();
	private bool _haveJoined;
	private const bool ServerHaveResources = true;
	private readonly object _mapInfoSync = new();
	private static readonly char[] Separator = [';'];
	private static readonly char[] SeparatorArray = [';'];
	private int _lastOrderingIndex;

    // Miscellaneous Settings
	public bool MuteEmoteAnnouncements { get; set; } = false;
	

	public Player(MiNetServer server, IPEndPoint endPoint) : base(EntityType.None, null)
	{
		Server = server;
		EndPoint = endPoint;

		Inventory = new PlayerInventory(this);
		HungerManager = new HungerManager(this);
		ExperienceManager = new ExperienceManager(this);
		ItemStackInventoryManager = new ItemStackInventoryManager(this);

		IsSpawned = false;
		IsConnected = endPoint != null; // Can't connect if there is no endpoint

		Width = 0.6f;
		Length = Width;
		Height = 1.80;

		HideNameTag = false;
		IsAlwaysShowName = true;
		CanClimb = true;
		HasCollision = true;
		IsAffectedByGravity = true;
		NoAi = false;
	}

	public void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message)
	{
		// Beware that message might be null here.

		ConnectionInfo serverInfo = Server.ConnectionInfo;
		Interlocked.Increment(ref serverInfo.ConnectionsInConnectPhase);
		
		SendPlayerStatus(0);
		SendResourcePacksInfo();
	}

	public void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message)
	{
		var jsonSerializerSettings = new JsonSerializerSettings
		{
			PreserveReferencesHandling = PreserveReferencesHandling.None,
			Formatting = Formatting.Indented
		};

		string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
		Log.Debug($"{message.GetType().Name}\n{result}");

		byte[] content = File.ReadAllBytes(PlayerPackMap[message.packageId].pack);

		McpeResourcePackChunkData chunkData = McpeResourcePackChunkData.CreateObject();
		chunkData.packageId = message.packageId;
		chunkData.chunkIndex = message.chunkIndex;
		chunkData.progress = 16384 * message.chunkIndex;
		chunkData.payload = GetChunk(content, (int) chunkData.chunkIndex, 16384);
		SendPacket(chunkData);
	}

	public static byte[] GetChunk(byte[] content, int chunkIndex, int chunkSize)
	{
		int start = chunkIndex * chunkSize;
		int length = Math.Min(chunkSize, content.Length - start);
		byte[] chunk = new byte[length];
		Array.Copy(content, start, chunk, 0, length);
		return chunk;
	}

	public void HandleMcpePlayerSkin(McpePlayerSkin message)
	{
		McpePlayerSkin pk = McpePlayerSkin.CreateObject();
		pk.uuid = ClientUuid;
		pk.skin = message.skin;
		pk.oldSkinName = Skin.SkinId;
		pk.skinName = message.skinName;
		Skin = message.skin;
		Level.RelayBroadcast(pk);
	}

	public void HandleMcpeModalFormResponse(McpeModalFormResponse message)
	{
		if (CurrentForm == null) Log.Warn("No current form set for player when processing response");

		Form form = CurrentForm;
		if (form == null || form.Id != message.formId)
		{
			Log.Warn("Receive data for form not currently active");
			return;
		}
		CurrentForm = null;
		form.FromJson(message.data, this);
	}

	public void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message) => SetGameMode((GameMode) message.gamemode);
	public void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message) => OnLocalPlayerIsInitialized(new PlayerEventArgs(this));

	public void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message)
	{
		if (Log.IsDebugEnabled) Log.Debug($"Handled packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");

		switch (message.responseStatus)
		{
			case 2:
			{
				foreach (string packId in message.resourcepackids)
				{
					string uuid = packId[..Math.Min(packId.Length, 36)];
					byte[] content = File.ReadAllBytes(PlayerPackMap[uuid].pack);
					byte[] packHash = SHA256.HashData(content);

					McpeResourcePackDataInfo dataInfo = McpeResourcePackDataInfo.CreateObject();
					dataInfo.packageId = uuid;
					dataInfo.maxChunkSize = 16384;
					dataInfo.chunkCount = (uint) Math.Ceiling((double) content.Length / 16384);
					dataInfo.compressedPackageSize = (ulong) content.Length;
					dataInfo.hash = packHash;
					dataInfo.isPremium = false;
					dataInfo.packType = (byte) PlayerPackMap[uuid].type;
					SendPacket(dataInfo);
				}
				return;
			}
			case 3:
				SendResourcePackStack();
				return;
			case 4:
				MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Start(null); });
				PlayerPackData.Clear();
				PlayerPackDataB.Clear();
				PlayerPackMap.Clear();
				return;
		}
	}

	public void SendResourcePacksInfo()
	{
		McpeResourcePacksInfo packInfo = McpeResourcePacksInfo.CreateObject();
		if (ServerHaveResources)
		{
			var packInfos = new TexturePackInfos();
			var packInfosB = new ResourcePackInfos();
			string resourceDirectory = Config.GetProperty("ResourceDirectory", "ResourcePacks");
			string behaviorDirectory = Config.GetProperty("BehaviorDirectory", "BehaviorPacks");
			packInfo.mustAccept = Config.GetProperty("ForceResourcePacks", false);
			packInfo.templateUUID = new UUID(Guid.Empty.ToByteArray());

			ProcessResourcePacks(resourceDirectory, packInfos, ResourcePackType.Resources);
			ProcessResourcePacks(behaviorDirectory, packInfosB, ResourcePackType.Behaviour);

			PlayerPackData = packInfos;
			PlayerPackDataB = packInfosB;
			packInfo.texturepacks = packInfos;

		}
		SendPacket(packInfo);
	}

	private void ProcessResourcePacks(string directory, dynamic packInfos, ResourcePackType type)
	{
		if (!Directory.Exists(directory)) return;

		foreach (string zipPack in Directory.GetFiles(directory, "*.zip"))
		{
			try
			{
				using ZipArchive archive = ZipFile.OpenRead(zipPack);
				ZipArchiveEntry manifestEntry = archive.Entries.FirstOrDefault(e => e.FullName == "manifest.json");
				if (manifestEntry == null)
				{
					Disconnect($"Invalid {type.ToString().ToLower()} pack {zipPack}. Unable to locate manifest.json");
					continue;
				}

				bool encrypted = File.Exists($"{zipPack}.key");

				using Stream stream = manifestEntry.Open();
				using var reader = new StreamReader(stream);
				string jsonContent = reader.ReadToEnd();
				manifestStructure obj = JsonConvert.DeserializeObject<manifestStructure>(jsonContent);

				var packInfo = new TexturePackInfo
				{
					UUID = new UUID(obj.Header.Uuid),
					Version = $"{obj.Header.Version[0]}.{obj.Header.Version[1]}.{obj.Header.Version[2]}",
					Size = (ulong) new FileInfo(zipPack).Length,
					ContentKey = encrypted ? File.ReadAllText($"{zipPack}.key") : "",
					ContentIdentity = obj.Header.Uuid
				};

				packInfos.Add(packInfo);

				PlayerPackMap[obj.Header.Uuid] = new PlayerPackMapData
				{
					pack = zipPack,
					type = type
				};
			}
			catch (Exception ex)
			{
				Disconnect($"Failed to process {zipPack}: {ex.Message}");
			}
		}
	}

	public void SendResourcePackStack()
	{
		McpeResourcePackStack packStack = McpeResourcePackStack.CreateObject();
		packStack.gameVersion = McpeProtocolInfo.GameVersion;

		if (ServerHaveResources)
		{
			var packVersions = new ResourcePackIdVersions();
			var packVersionsB = new ResourcePackIdVersions();
			packVersions.AddRange(PlayerPackData.Select(packData => new PackIdVersion
			{
				Id = packData.UUID.ToString(),
				Version = packData.Version
			}));
			packVersionsB.AddRange(PlayerPackDataB.Select(packData => new PackIdVersion
			{
				Id = packData.UUID.ToString(),
				Version = packData.Version
			}));
			packStack.resourcepackidversions = packVersions;
			packStack.behaviorpackidversions = packVersionsB;
		}

		SendPacket(packStack);
	}

	public void HandleMcpeRiderJump(McpeRiderJump message)
	{
		if (!IsRiding || Vehicle <= 0) return;
		if (!Level.TryGetEntity(Vehicle, out Mob mob)) return;
		mob.IsRearing = true;
		mob.BroadcastSetEntityData();
	}

	public void HandleMcpeSetActorData(McpeSetActorData message)
	{
		// Only used by EDU NPC so far.
		if (Level.TryGetEntity(message.runtimeEntityId, out Entity entity)) entity.SetEntityData(message.metadata);
	}

	public void HandleMcpeNpcRequest(McpeNpcRequest message)
	{
		// Only used by EDU NPC.

		if (!Level.TryGetEntity(message.runtimeEntityId, out Entity entity)) return;
		// 0 is edit
		// 0 is exec command
		// 2 is exec link

		if (message.unknown0 != 0) return;
		var metadata = new MetadataDictionary();
		//metadata[42] = new MetadataString(message.unknown1); todo whats this
		entity.SetEntityData(metadata);
	}
	
	public void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
	{
		lock (_mapInfoSync)
		{
			long mapId = message.mapId;

			Log.Trace($"Requested map with ID: {mapId} 0x{mapId:X2}");

			if (!Level.TryGetEntity(mapId, out MapEntity mapEntity))
			{
				mapEntity = new MapEntity(Level, mapId);
				mapEntity.SpawnEntity();
			}
			else mapEntity?.AddToMapListeners(this, mapId);
		}
	}

	public void SendMapInfo(MapInfo mapInfo)
	{
		McpeClientboundMapItemData packet = McpeClientboundMapItemData.CreateObject();
		packet.mapinfo = mapInfo;
		SendPacket(packet);
	}

	public void SetChunkRadius(int radius)
	{
		ChunkRadius = Math.Max(5, Math.Min(radius, MaxViewDistance));
	}

	public void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
	{
		Log.Debug($"Requested chunk radius of: {message.chunkRadius}");

		SetChunkRadius(message.chunkRadius);
		SendChunkRadiusUpdate();
		MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
	}

	public void HandleMcpeMoveEntity(McpeMoveEntity message)
	{
		if (Vehicle != message.runtimeEntityId || !Level.TryGetEntity(message.runtimeEntityId, out Entity entity)) return;
		entity.KnownPosition = message.position;
		entity.IsOnGround = (message.flags & 1) == 1;
		if (entity.IsOnGround) Log.Debug("Horse is on ground");
	}

	public void HandleMcpeAnimate(McpeAnimate message)
	{
		if (Level == null) return;

		McpeAnimate msg = McpeAnimate.CreateObject();
		msg.runtimeEntityId = EntityId;
		msg.actionId = message.actionId;
		msg.unknownFloat = message.unknownFloat;

		Level.RelayBroadcast(this, msg);
	}
	
	public void HandleMcpePlayerAction(McpePlayerAction message)
	{
		switch ((PlayerAction) message.actionId)
		{
			case PlayerAction.StartBreak:
			{
				if (message.face == (int) BlockFace.Up)
				{
					Block block = Level.GetBlock(message.coordinates.BlockUp());
					if (block is Fire)
					{
						Level.BreakBlock(this, message.coordinates.BlockUp());
						break;
					}
				}


				if (GameMode == GameMode.Survival)
				{
					Block target = Level.GetBlock(message.coordinates);
					Item[] drops = target.GetDrops(Inventory.GetItemInHand());
					float toolTypeFactor = drops == null || drops.Length == 0 ? 5f : 1.5f;
					double breakTime = Math.Ceiling(target.Hardness * toolTypeFactor * 20);

					McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
					breakEvent.eventId = 3600;
					breakEvent.position = message.coordinates;
					breakEvent.data = (int) (65535 / breakTime);
					Log.Debug("Break speed: " + breakEvent.data);
					Level.RelayBroadcast(breakEvent);
				}

				break;
			}
			case PlayerAction.Breaking:
			{
				Block target = Level.GetBlock(message.coordinates);
				int data = target.GetRuntimeId() | ((byte) (message.face << 24));

				McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
				breakEvent.eventId = 2014;
				breakEvent.position = message.coordinates;
				breakEvent.data = data;
				Level.RelayBroadcast(breakEvent);
				break;
			}
			case PlayerAction.AbortBreak:
			case PlayerAction.StopBreak:
			{
				McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
				breakEvent.eventId = 3601;
				breakEvent.position = message.coordinates;
				Level.RelayBroadcast(breakEvent);
				break;
			}
			case PlayerAction.StartSleeping:
			{
				break;
			}
			case PlayerAction.StopSleeping:
			{
				IsSleeping = false;
				if (Level.GetBlock(SpawnPosition) is Bed bed)
					bed.SetOccupied(Level, false);
				else
					Log.Warn($"Did not find a bed at {SpawnPosition}");
				break;
			}
			case PlayerAction.Jump:
			{
				HungerManager.IncreaseExhaustion(IsSprinting ? 0.2f : 0.05f);
				break;
			}
			case PlayerAction.StartSprint:
			{
				SetSprinting(true);
				break;
			}
			case PlayerAction.StopSprint:
			{
				SetSprinting(false);
				break;
			}
			case PlayerAction.StartSneak:
			{
				SetSprinting(false);
				IsSneaking = true;
				break;
			}
			case PlayerAction.StopSneak:
			{
				SetSprinting(false);
				IsSneaking = false;
				break;
			}
			case PlayerAction.CreativeDestroy:
			{
				break;
			}
			case PlayerAction.DimensionChangeAck:
			{
				SendPlayerStatus(3);
				break;
			}
			case PlayerAction.WorldImmutable:
			{
				break;
			}
			case PlayerAction.StartGlide:
			{
				IsGliding = true;
				Height = 0.6;

				var particle = new WhiteSmokeParticle(Level) { Position = KnownPosition.ToVector3() };
				particle.Spawn();

				break;
			}
			case PlayerAction.StopGlide:
			{
				IsGliding = false;
				Height = 1.8;
				break;
			}
			case PlayerAction.SetEnchantmentSeed:
			{
				Log.Debug($"Got PlayerAction.SetEnchantmentSeed with data={message.face} at {message.coordinates}");
				break;
			}
			case PlayerAction.InteractBlock:
			{
				break;
			}
			case PlayerAction.StartSwimming:
			{
				IsSwimming = true;
				break;
			}
			case PlayerAction.StopSwimming:
			{
				IsSwimming = false;
				break;
			}
			case PlayerAction.MissedSwing:
			{
				break;
			}
			case PlayerAction.StartCrawling:
			{
				break;
			}
			case PlayerAction.StopCrawling:
			{
				break;
			}
			case PlayerAction.StartItemUse:
			{
				IsUsingItem = true;
				break;
			}
			case PlayerAction.StopItemUse:
			{
				IsUsingItem = false;
				break;
			}
			case PlayerAction.StartFlying:
			{
				break;
			}
			case PlayerAction.StopFlying:
			{
				break;
			}
			case PlayerAction.Respawn:
			{
				break;
			}
			case PlayerAction.ChangeSkin:
			case PlayerAction.GetUpdatedBlock:
			case PlayerAction.DropItem:
			case PlayerAction.StartSpinAttack:
			case PlayerAction.StopSpinAttack:
			case PlayerAction.PredictDestroyBlock:
			case PlayerAction.ContinueDestroyBlock:
			case PlayerAction.HandledTeleport:
			default:
			{
				Log.Warn($"Unhandled action ID={message.actionId}");
				throw new ArgumentOutOfRangeException(nameof(message.actionId));
			}
		}

		BroadcastSetEntityData();
	}

	public void SetSprinting(bool isSprinting)
	{
		lock (_sprintLock)
		{
			if (isSprinting == IsSprinting) return;

			if (isSprinting)
			{
				IsSprinting = true;
				_baseSpeed = MovementSpeed;
				MovementSpeed += MovementSpeed * 0.3f;
			}
			else
			{
				IsSprinting = false;
				MovementSpeed = _baseSpeed;
			}

			SendUpdateAttributes();
		}
	}

	public void HandleMcpeBlockEntityData(McpeBlockEntityData message)
	{
		if (Log.IsDebugEnabled)
		{
			Log.DebugFormat("x:  {0}", message.coordinates.X);
			Log.DebugFormat("y:  {0}", message.coordinates.Y);
			Log.DebugFormat("z:  {0}", message.coordinates.Z);
			Log.DebugFormat("NBT {0}", message.namedtag.NbtFile);
		}

		BlockEntity blockEntity = Level.GetBlockEntity(message.coordinates);

		if (blockEntity == null) return;

		blockEntity.SetCompound((NbtCompound) message.namedtag.NbtFile.RootTag);
		Level.SetBlockEntity(blockEntity);
	}

	public void HandleMcpeAdventureSettings(McpeAdventureSettings message)
	{
		uint flags = message.flags;
		IsAutoJump = (flags & 0x20) == 0x20;
		IsFlying = (flags & 0x200) == 0x200;
	}

	public void SendEntitiesAnimation(string animationName, long[] entityIds)
	{
		McpeAnimateEntity packet = McpeAnimateEntity.CreateObject();
		packet.animationName = animationName;
		packet.entities = entityIds;
		SendPacket(packet);
	}

	public void SendAnimation(string animationName, bool relay = true)
	{
		{
			McpeAnimateEntity packet = McpeAnimateEntity.CreateObject();
			packet.animationName = animationName;
			packet.entities = [EntityManager.EntityIdSelf];
			SendPacket(packet);
		}

		if (!relay) return;
		{
			McpeAnimateEntity packet = McpeAnimateEntity.CreateObject();
			packet.animationName = animationName;
			packet.entities = [EntityId];
			Level.RelayBroadcast(this, packet);
		}
	}

	public void SendAnimation(AnimateProperties animateProperties, bool relay = true)
	{
		{
			McpeAnimateEntity packet = McpeAnimateEntity.CreateObject();
			packet.animationName = animateProperties.AnimationName;
			packet.entities = [EntityManager.EntityIdSelf];
			packet.controllerName = animateProperties.ControllerName;
			packet.blendOutTime = animateProperties.BlendOutTime;
			packet.stopExpression = animateProperties.StopExpression;
			packet.molangVersion = animateProperties.MolangVersion;
			packet.nextState = animateProperties.NextState;
			SendPacket(packet);
		}

		if (!relay) return;
		{
			McpeAnimateEntity packet = McpeAnimateEntity.CreateObject();
			packet.animationName = animateProperties.AnimationName;
			packet.entities = [EntityId];
			packet.controllerName = animateProperties.ControllerName;
			packet.blendOutTime = animateProperties.BlendOutTime;
			packet.stopExpression = animateProperties.StopExpression;
			packet.molangVersion = animateProperties.MolangVersion;
			packet.nextState = animateProperties.NextState;
			Level.RelayBroadcast(this, packet);
		}
	}

	public void SendGameRules()
	{
		McpeGameRulesChanged gameRulesChanged = McpeGameRulesChanged.CreateObject();
		gameRulesChanged.rules = Level.GetGameRules();
		SendPacket(gameRulesChanged);
	}

	public void SendAdventureSettings()
	{
		McpeUpdateAdventureSettings settings = McpeUpdateAdventureSettings.CreateObject();
		settings.noPvm = IsNoPvm;
		settings.noMvp = IsNoMvp;
		settings.autoJump = IsAutoJump;
		settings.immutableWorld = IsWorldImmutable;
		settings.showNametags = true;
		SendPacket(settings);
	}

	public void SendAbilities()
	{
		McpeUpdateAbilities packet = McpeUpdateAbilities.CreateObject();
		packet.layers = GetAbilities();
		packet.commandPermissions = (byte) CommandPermission;
		packet.playerPermissions = (byte) PermissionLevel;
		packet.entityUniqueId = BinaryPrimitives.ReverseEndianness(EntityId);
		SendPacket(packet);
	}

	private AbilityLayers GetAbilities()
	{
		PlayerAbility abilities = 0;

		if (AllowFly || GameMode.AllowsFlying())
		{
			abilities |= PlayerAbility.MayFly;
			if (IsFlying) abilities |= PlayerAbility.Flying;
		}

		if (!GameMode.HasCollision()) abilities |= PlayerAbility.NoClip;
		if (!GameMode.AllowsTakingDamage()) abilities |= PlayerAbility.Invulnerable;
		if (GameMode.HasCreativeInventory()) abilities |= PlayerAbility.InstantBuild;

		if (PermissionLevel is PermissionLevel.Operator or PermissionLevel.Member)
			abilities |= PlayerAbility.Build | PlayerAbility.Mine |
						PlayerAbility.DoorsAndSwitches | PlayerAbility.OpenContainers |
						PlayerAbility.AttackPlayers | PlayerAbility.AttackMobs;
		if (PermissionLevel == PermissionLevel.Operator) abilities |= PlayerAbility.OperatorCommands;
		if (IsMuted) abilities |= PlayerAbility.Muted;

		var layers = new AbilityLayers();
		var baseLayer = new AbilityLayer
		{
			Type = AbilityLayerType.Base,
			Abilities = PlayerAbility.All,
			Values = (uint) abilities,
			FlySpeed = 0.1f,
			VerticalFlySpeed = 0.1f
		};

		layers.Add(baseLayer);
		return layers;
	}

	[Wired]
	public void SetSpectator(bool isSpectator)
	{
		IsSpectator = isSpectator;
		SendAdventureSettings();
	}

	public bool IsAutoJump { get; set; }

	[Wired]
	public void SetAutoJump(bool isAutoJump)
	{
		IsAutoJump = isAutoJump;
		SendAdventureSettings();
	}

	public bool AllowFly { get; set; }

	[Wired]
	public void SetAllowFly(bool allowFly)
	{
		AllowFly = allowFly;
		SendAdventureSettings();
	}

	public void Start(object o)
	{
		var watch = new Stopwatch();
		watch.Restart();

		ConnectionInfo serverInfo = Server.ConnectionInfo;

		try
		{
			Session = Server.SessionManager.CreateSession(this);

			lock (_disconnectSync)
			{
				if (!IsConnected) return;

				if (Level != null) return; // Already called this method.

				Level = Server.LevelManager.GetLevel(this, Dimension.Overworld.ToString());
			}

			if (Level == null)
			{
				Disconnect("No level assigned.");
				return;
			}

			OnPlayerJoining(new PlayerEventArgs(this));

			SpawnPosition = (PlayerLocation) (SpawnPosition ?? Level.SpawnPoint).Clone();
			KnownPosition = (PlayerLocation) SpawnPosition.Clone();
			NameTag = (string) Username.Clone();

			// Check if the user already exist, that case bump the old one
			Level.RemoveDuplicatePlayers(Username, ClientId);

			Level.EntityManager.AddEntity(this);

			GameMode = Config.GetProperty("Player.GameMode", Level.GameMode);

			//
			// Start game - spawn sequence starts here
			//

			SendSetTime();

			SendStartGame();

			SendItemComponents();

			SendAvailableEntityIdentifiers();

			SendBiomeDefinitionList();

			BroadcastSetEntityData();

			if (ChunkRadius == -1) ChunkRadius = 5;

			SendChunkRadiusUpdate();

			//SendSetSpawnPosition();

			SendSetTime();

			SendSetDifficulty();

			SendSetCommandsEnabled();

			SendAdventureSettings();

			SendGameRules();

			// Vanilla 2nd player list here

			Level.AddPlayer(this, false);

			SendUpdateAttributes();

			SendPlayerInventory();

			SendCreativeInventory();

			SendCraftingRecipes();

			SendAvailableCommands(); // Don't send this before StartGame!

			SendNetworkChunkPublisherUpdate();
		}
		catch (Exception e)
		{
			Log.Error(e);
		}
		finally
		{
			Interlocked.Decrement(ref serverInfo.ConnectionsInConnectPhase);
		}

		LastUpdatedTime = DateTime.UtcNow;
		Log.InfoFormat("Login complete by: {0} from {2} in {1}ms", Username, watch.ElapsedMilliseconds, EndPoint);
	}

	public void SendAvailableEntityIdentifiers()
	{
		var nbt = new Nbt
		{
			NbtFile = new NbtFile
			{
				BigEndian = false,
				UseVarInt = true,
				RootTag = new NbtCompound("") { EntityHelpers.GenerateEntityIdentifiers() }
			}
		};

		McpeAvailableEntityIdentifiers pk = McpeAvailableEntityIdentifiers.CreateObject();
		pk.namedtag = nbt;
		SendPacket(pk);
	}

	public void SendBiomeDefinitionList()
	{
		var nbt = new Nbt
		{
			NbtFile = new NbtFile
			{
				BigEndian = false,
				UseVarInt = true,
				RootTag = BiomeUtils.GenerateDefinitionList(),
			}
		};

		McpeBiomeDefinitionList pk = McpeBiomeDefinitionList.CreateObject();
		pk.namedtag = nbt;
		SendPacket(pk);
	}
	
	private void SendSetCommandsEnabled()
	{
		McpeSetCommandsEnabled enabled = McpeSetCommandsEnabled.CreateObject();
		enabled.enabled = EnableCommands;
		SendPacket(enabled);
	}

	private void SendAvailableCommands()
	{
		McpeAvailableCommands commands = McpeAvailableCommands.CreateObject();
		commands.CommandSet = Server.PluginManager.Commands;
		SendPacket(commands);
	}

	public void HandleMcpeCommandRequest(McpeCommandRequest message)
	{
		Log.Debug($"UUID: {message.unknownUuid}");

		object result = Server.PluginManager.HandleCommand(this, message.command);
		if (result is not string sRes) return;
		SendMessage(sRes);
	}

	public void InitializePlayer()
	{
		SendSetEntityData();
		SendPlayerStatus(3);
		SendSetTime();
		
		IsSpawned = true;
		SetPosition(SpawnPosition);

		LastUpdatedTime = DateTime.UtcNow;
		_haveJoined = true;

		OnPlayerJoin(new PlayerEventArgs(this));

		String ops = Config.GetProperty("ServerOps", "");
		foreach (string opsP in ops.Split(SeparatorArray, StringSplitOptions.RemoveEmptyEntries))
		{
			if (Username != opsP) continue;
			ActionPermissions = ActionPermissions.Operator;
			CommandPermission = 4;
			PermissionLevel = PermissionLevel.Operator;
			SendAbilities();
		}

		bool isWhitelisted = Config.GetProperty("IsWhitelisted", false);
		if (!isWhitelisted) return;
		String whitelistList = Config.GetProperty("WhitelistedPlayers", "");
		bool isAllowed = false;
		foreach (string whitelistP in whitelistList.Split(Separator, StringSplitOptions.RemoveEmptyEntries))
			if (Username == whitelistP) isAllowed = true;
		if (!isAllowed) Disconnect("You are not whitelisted");
	}

	public void SavePlayerInventory()
	{
		var pInventoryData = new List<string>();
		string basePath = Config.GetProperty("LevelDBWorldFolder", "World").Trim();
		string rDataJson = File.ReadAllText(basePath + "/PlayerData/" + ClientUuid + ".json");
		PlayerData prDataJson = JsonConvert.DeserializeObject<PlayerData>(rDataJson);
		for (int i = 0; i < PlayerInventory.InventorySize; i++) pInventoryData.Add(Inventory.Slots[i].Name);
		prDataJson.Inventory = pInventoryData;
		File.WriteAllText(basePath + "/PlayerData/" + ClientUuid + ".json", prDataJson.ToString());
	}

	public void HandleMcpeRespawn(McpeRespawn message)
	{
		if (message.state == (byte) McpeRespawn.RespawnState.ClientReady)
		{
			HealthManager.ResetHealth();

			HungerManager.ResetHunger();

			BroadcastSetEntityData();

			SendUpdateAttributes();

			SendSetSpawnPosition();

			SendAdventureSettings();

			SendPlayerInventory();

			CleanCache();

			ForcedSendChunk(SpawnPosition);

			// send teleport to spawn
			SetPosition(SpawnPosition);

			Level.SpawnToAll(this);

			IsSpawned = true;

			Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

			SendSetTime();

			MiNetServer.FastThreadPool.QueueUserWorkItem(() => ForcedSendChunks());

			McpeRespawn mcpeRespawn = McpeRespawn.CreateObject();
			mcpeRespawn.x = SpawnPosition.X;
			mcpeRespawn.y = SpawnPosition.Y;
			mcpeRespawn.z = SpawnPosition.Z;
			mcpeRespawn.state = (byte) McpeRespawn.RespawnState.Ready;
			mcpeRespawn.runtimeEntityId = EntityId;
			SendPacket(mcpeRespawn);
		}
		else
			Log.Warn($"Unhandled respawn state = {message.state}");
	}

	[Wired]
	public void SetPosition(PlayerLocation position, bool teleport = true)
	{
		KnownPosition = position;
		LastUpdatedTime = DateTime.UtcNow;

		McpeMovePlayer packet = McpeMovePlayer.CreateObject();
		packet.runtimeEntityId = EntityManager.EntityIdSelf;
		packet.x = position.X;
		packet.y = position.Y + 1.62f;
		packet.z = position.Z;
		packet.yaw = position.Yaw;
		packet.headYaw = position.HeadYaw;
		packet.pitch = position.Pitch;
		packet.mode = (byte) (teleport ? 1 : 0);

		SendPacket(packet);
	}

	public void Teleport(PlayerLocation newPosition)
	{
		KnownPosition = newPosition;
		LastUpdatedTime = DateTime.UtcNow;

		McpeMovePlayer packet = McpeMovePlayer.CreateObject();
		packet.runtimeEntityId = EntityManager.EntityIdSelf;
		packet.x = newPosition.X;
		packet.y = newPosition.Y + 1.62f;
		packet.z = newPosition.Z;
		packet.yaw = newPosition.Yaw;
		packet.headYaw = newPosition.HeadYaw;
		packet.pitch = newPosition.Pitch;
		packet.mode = 1;

		SendPacket(packet);
	}

	public void ChangeDimension(Level toLevel, PlayerLocation spawnPoint, Dimension dimension, Func<Level> levelFunc = null)
	{
		switch (dimension)
		{
			case Dimension.Overworld:
				break;
			case Dimension.Nether:
				if (!Level.WorldProvider.HaveNether())
				{
					Log.Warn($"This world doesn't have nether");
					return;
				}
				break;
			case Dimension.TheEnd:
				if (!Level.WorldProvider.HaveTheEnd())
				{
					Log.Warn($"This world doesn't have the end");
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
		}

		switch (dimension)
		{
			case Dimension.Overworld:
			{
				var start = (BlockCoordinates) KnownPosition;
				start *= new BlockCoordinates(8, 1, 8);
				SendChangeDimension(dimension, false, start);
				break;
			}
			case Dimension.Nether:
			{
				var start = (BlockCoordinates) KnownPosition;
				start /= new BlockCoordinates(8, 1, 8);
				SendChangeDimension(dimension, false, start);
				break;
			}
			case Dimension.TheEnd:
			{
				var start = (BlockCoordinates) KnownPosition;
				SendChangeDimension(dimension, false, start);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
		}

		Level.RemovePlayer(this);

		Dimension fromDimension = Level.Dimension;

		if (toLevel == null && levelFunc != null) toLevel = levelFunc();

		Level = toLevel; // Change level
		SpawnPosition = spawnPoint ?? Level?.SpawnPoint;

		BroadcastSetEntityData();

		SendUpdateAttributes();

		CleanCache();

		switch (dimension)
		{
			// Check if we need to generate a platform
			case Dimension.TheEnd:
			{
				BlockCoordinates platformPosition = ((BlockCoordinates) SpawnPosition).BlockDown();
				if (!(Level?.GetBlock(platformPosition) is Obsidian))
					for (int x = 0; x < 5; x++)
					for (int z = 0; z < 5; z++)
					for (int y = 0; y < 5; y++)
					{
						BlockCoordinates coordinates = new BlockCoordinates(x, y, z) + platformPosition + new BlockCoordinates(-2, 0, -2);
						if (y == 0)
							Level.SetBlock(new Obsidian() { Coordinates = coordinates });
						else
							Level.SetAir(coordinates);
					}
				break;
			}
			case Dimension.Overworld when fromDimension == Dimension.TheEnd:
				// Spawn on player home spawn
				break;
			case Dimension.Nether:
			{
				// Find closes portal or spawn new
				// coordinate translation x/8

				BlockCoordinates start = (BlockCoordinates) KnownPosition;
				start /= new BlockCoordinates(8, 1, 8);

				PlayerLocation pos = FindNetherSpawn(Level, start);
				SpawnPosition = pos ?? CreateNetherPortal(Level);
				break;
			}
			default:
			{
				if (fromDimension == Dimension.Nether)
				{
					// Find closes portal or spawn new
					// coordinate translation x * 8

					BlockCoordinates start = (BlockCoordinates) KnownPosition;
					start *= new BlockCoordinates(8, 1, 8);

					PlayerLocation pos = FindNetherSpawn(Level, start);
					SpawnPosition = pos ?? CreateNetherPortal(Level);
				}
				break;
			}
		}

		Log.Debug($"Spawn point: {SpawnPosition}");

		SendChunkRadiusUpdate();

		ForcedSendChunk(SpawnPosition);

		// send teleport to spawn
		SetPosition(SpawnPosition);

		MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
		{
			Level.AddPlayer(this, true);

			ForcedSendChunks(() =>
			{
				Log.WarnFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

				SendSetTime();
			});
		});
	}

	private PlayerLocation FindNetherSpawn(Level level, BlockCoordinates start)
	{
		const int width = 128;
		int height = Level.Dimension == Dimension.Overworld ? 256 : 128;


		int portalId = new Portal().Id;
		int obsidianId = new Obsidian().Id;

		Log.Debug($"Starting point: {start}");

		BlockCoordinates? closestPortal = null;
		int closestDistance = int.MaxValue;
		for (int x = start.X - width; x < start.X + width; x++)
		for (int z = start.Z - width; z < start.Z + width; z++)
		{
			if (level.Dimension == Dimension.Overworld) height = level.GetHeight(new BlockCoordinates(x, 0, z)) + 10;

			for (int y = height - 1; y >= 0; y--)
			{
				var coordinates = new BlockCoordinates(x, y, z);
				if (coordinates.DistanceTo(start) > closestDistance) continue;

				bool b = level.IsBlock(coordinates, portalId);
				b &= level.IsBlock(coordinates.BlockDown(), obsidianId);
				if (!b) continue;
				var portal = (Portal) level.GetBlock(coordinates);
				if (portal.PortalAxis == "z")
					b &= level.IsBlock(coordinates.BlockNorth(), portalId);
				else
					b &= level.IsBlock(coordinates.BlockEast(), portalId);

				Log.Debug($"Found portal block at {coordinates}, axis={portal.PortalAxis}");
				if (!b || !(coordinates.DistanceTo(start) < closestDistance)) continue;
				Log.Debug($"Found a closer portal at {coordinates}");
				closestPortal = coordinates;
				closestDistance = (int) coordinates.DistanceTo(start);
			}
		}

		return closestPortal;
	}

	// NOTE: Shouldn't that be in another kind of class?... It's out of context.
	private PlayerLocation CreateNetherPortal(Level level)
	{
		const int width = 16;
		int height = Level.Dimension == Dimension.Overworld ? 256 : 128;


		var start = (BlockCoordinates) KnownPosition;
		if (Level.Dimension == Dimension.Nether)
			start /= new BlockCoordinates(8, 1, 8);
		else
			start *= new BlockCoordinates(8, 1, 8);

		Log.Debug($"Starting point: {start}");

		PortalInfo closestPortal = null;
		int closestPortalDistance = int.MaxValue;
		for (int x = start.X - width; x < start.X + width; x++)
		{
			for (int z = start.Z - width; z < start.Z + width; z++)
			{
				if (level.Dimension == Dimension.Overworld)
				{
					height = level.GetHeight(new BlockCoordinates(x, 0, z)) + 10;
				}

				for (int y = height - 1; y >= 0; y--)
				{
					var coordinates = new BlockCoordinates(x, y, z);
					if (coordinates.DistanceTo(start) > closestPortalDistance) continue;

					if (!(!level.IsAir(coordinates) && level.IsAir(coordinates.BlockUp()))) continue;

					var bbox = new BoundingBox(coordinates, coordinates + new BlockCoordinates(3, 5, 4));
					if (!SpawnAreaClear(bbox))
					{
						bbox = new BoundingBox(coordinates, coordinates + new BlockCoordinates(4, 5, 3));
						if (!SpawnAreaClear(bbox))
						{
							bbox = new BoundingBox(coordinates, coordinates + new BlockCoordinates(1, 5, 4));
							if (!SpawnAreaClear(bbox))
							{
								bbox = new BoundingBox(coordinates, coordinates + new BlockCoordinates(4, 5, 1));
								if (!SpawnAreaClear(bbox)) continue;
							}
						}
					}

					Log.Debug($"Found portal location at {coordinates}");
					if (!(coordinates.DistanceTo(start) < closestPortalDistance)) continue;
					Log.Debug($"Found a closer portal location at {coordinates}");
					closestPortal = new PortalInfo
					{
						Coordinates = coordinates,
						Size = bbox
					};
					closestPortalDistance = (int) coordinates.DistanceTo(start);
				}
			}
		}

		if (closestPortal == null)
		{
			// Force create between Y=YMAX - (10 to 70)
			int y = (int) Math.Max(Height - 70, start.Y);
			y = (int) Math.Min(Height - 10, y);
			start.Y = y;

			Log.Debug($"Force portal location at {start}");

			closestPortal = new PortalInfo
			{
				HasPlatform = true,
				Coordinates = start,
				Size = new BoundingBox(start, start + new BlockCoordinates(4, 5, 3))
			};
		}
		BuildPortal(level, closestPortal);
		return closestPortal?.Coordinates;
	}

	public static void BuildPortal(Level level, PortalInfo portalInfo)
	{
		BoundingBox bbox = portalInfo.Size;

		Log.Debug($"Building portal from BBOX: {bbox}");

		int minX = (int) (bbox.Min.X);
		int minZ = (int) (bbox.Min.Z);
		int width = (int) (bbox.Width);
		int depth = (int) (bbox.Depth);
		int height = (int) (bbox.Height);

		int midPoint = depth > 2 ? depth / 2 : 0;

		bool haveSetCoordinate = false;
		for (int x = 0; x < width; x++)
		for (int z = 0; z < depth; z++)
		for (int y = 0; y < height; y++)
		{
			var coordinates = new BlockCoordinates(x + minX, (int) (y + bbox.Min.Y), z + minZ);
			Log.Debug($"Place: {coordinates}");

			if (width > depth && z == midPoint)
			{
				if ((x == 0 || x == width - 1) || (y == 0 || y == height - 1))
					level.SetBlock(new Obsidian { Coordinates = coordinates });
				else
				{
					level.SetBlock(new Portal
					{
						Coordinates = coordinates,
						PortalAxis = "x"
					});
					if (!haveSetCoordinate)
					{
						haveSetCoordinate = true;
						portalInfo.Coordinates = coordinates;
					}
				}
			}
			else if (width <= depth && x == midPoint)
			{
				if ((z == 0 || z == depth - 1) || (y == 0 || y == height - 1))
					level.SetBlock(new Obsidian { Coordinates = coordinates });
				else
				{
					level.SetBlock(new Portal
					{
						Coordinates = coordinates,
						PortalAxis = "z",
					});
					if (!haveSetCoordinate)
					{
						haveSetCoordinate = true;
						portalInfo.Coordinates = coordinates;
					}
				}
			}

			if (portalInfo.HasPlatform && y == 0) level.SetBlock(new Obsidian { Coordinates = coordinates });
		}
	}
	
	// NOTE: Shouldn't that be in another class? Such as the Level one ? It's out of context.
	private bool SpawnAreaClear(BoundingBox bbox)
	{
		BlockCoordinates min = bbox.Min;
		BlockCoordinates max = bbox.Max;
		for (int x = min.X; x < max.X; x++)
		for (int z = min.Z; z < max.Z; z++)
		for (int y = min.Y; y < max.Y; y++)
		{
			if (y == min.Y)
			{
				if (!Level.GetBlock(new BlockCoordinates(x, y, z)).IsBuildable) return false;
			}
			else
			{
				if (!Level.IsAir(new BlockCoordinates(x, y, z))) return false;
			}
		}
		return true;
		
	}

	// NOTE: Shouldn't that be in another class? Such as the Level one ? It's out of context
	public void SpawnLevel(Level toLevel, PlayerLocation spawnPoint, bool useLoadingScreen = false, Func<Level> levelFunc = null, Action postSpawnAction = null)
	{
		bool oldNoAi = NoAi;
		SetNoAi(true);

		if (useLoadingScreen) SendChangeDimension(Dimension.Nether);
		if (toLevel == null && levelFunc != null) toLevel = levelFunc();

		Action transferFunc = delegate
		{
			if (useLoadingScreen) SendChangeDimension(Dimension.Overworld);
			Level.RemovePlayer(this);

			Level = toLevel; // Change level
			SpawnPosition = spawnPoint ?? Level?.SpawnPoint;

			HungerManager.ResetHunger();

			HealthManager.ResetHealth();

			BroadcastSetEntityData();

			SendUpdateAttributes();

			SendSetSpawnPosition();

			SendAdventureSettings();

			SendPlayerInventory();

			CleanCache();

			ForcedSendChunk(SpawnPosition);

			MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
			{
				Level.AddPlayer(this, true);

				SetNoAi(oldNoAi);

				ForcedSendChunks(() =>
				{
					Log.InfoFormat("Respawn player {0} on level {1}", Username, Level.LevelId);

					SendSetTime();

					postSpawnAction?.Invoke();
				});
			});
			SetPosition(SpawnPosition);
		};

		transferFunc();
	}

	private void SendChangeDimension(Dimension dimension, bool respawn = false, Vector3 position = new())
	{
		McpeChangeDimension changeDimension = McpeChangeDimension.CreateObject();
		changeDimension.dimension = (int) dimension;
		changeDimension.position = position;
		changeDimension.respawn = respawn;
		changeDimension.NoBatch = true; // This is here because the client crashes otherwise.
		SendPacket(changeDimension);
	}

	public override void BroadcastSetEntityData(MetadataDictionary metadata)
	{
		McpeSetActorData mcpeSetActorData = McpeSetActorData.CreateObject();
		mcpeSetActorData.runtimeEntityId = EntityManager.EntityIdSelf;
		mcpeSetActorData.metadata = metadata;
		mcpeSetActorData.tick = CurrentTick;
		SendPacket(mcpeSetActorData);

		base.BroadcastSetEntityData(metadata);
	}

	public void SendSetEntityData()
	{
		McpeSetActorData mcpeSetActorData = McpeSetActorData.CreateObject();
		mcpeSetActorData.runtimeEntityId = EntityManager.EntityIdSelf;
		mcpeSetActorData.metadata = GetMetadata();
		mcpeSetActorData.tick = CurrentTick;
		SendPacket(mcpeSetActorData);
	}

	public void SendSetDifficulty()
	{
		McpeSetDifficulty mcpeSetDifficulty = McpeSetDifficulty.CreateObject();
		mcpeSetDifficulty.difficulty = (uint) Level.Difficulty;
		SendPacket(mcpeSetDifficulty);
	}

	// TODO: Move this to the inventory system
	public void SendPlayerInventory()
	{
		// TODO: We need to move the inventoryIds in an enum or something more clean
		McpeInventoryContent inventoryContent = McpeInventoryContent.CreateObject();
		inventoryContent.inventoryId = 0x00;
		inventoryContent.input = Inventory.GetSlots();
		SendPacket(inventoryContent);

		Inventory.ArmorInventory.SendArmorContentPacket(this);
		Inventory.OffHandInventory.SendUpdate();

		McpeInventoryContent uiContent = McpeInventoryContent.CreateObject();
		uiContent.inventoryId = 0x7c;
		uiContent.input = Inventory.GetUiSlots();
		SendPacket(uiContent);

		McpeMobEquipment mobEquipment = McpeMobEquipment.CreateObject();
		mobEquipment.runtimeEntityId = EntityManager.EntityIdSelf;
		mobEquipment.item = Inventory.GetItemInHand();
		mobEquipment.slot = (byte) Inventory.InHandSlot;
		mobEquipment.selectedSlot = (byte) Inventory.InHandSlot;
		SendPacket(mobEquipment);
	}

	public void SendCraftingRecipes()
	{
		McpeCraftingData craftingData = McpeCraftingData.CreateObject();
		craftingData.recipes = RecipeManager.Recipes;
		SendPacket(craftingData);
	}

	// TODO: Move this to the inventory system
	public void SendCreativeInventory()
	{
		if (!UseCreativeInventory) return;

		McpeCreativeContent creativeContent = McpeCreativeContent.CreateObject();
		creativeContent.groups = InventoryUtils.GetCreativeGroups();
		creativeContent.input = InventoryUtils.GetCreativeMetadataSlots();
		SendPacket(creativeContent);
	}

	private void SendChunkRadiusUpdate()
	{
		McpeChunkRadiusUpdate packet = McpeChunkRadiusUpdate.CreateObject();
		packet.chunkRadius = ChunkRadius;
		SendPacket(packet);
	}

	public void SendPlayerStatus(int status)
	{
		McpePlayStatus mcpePlayerStatus = McpePlayStatus.CreateObject();
		mcpePlayerStatus.status = status;
		SendPacket(mcpePlayerStatus);
	}

	[Wired]
	public void SetGameMode(GameMode gameMode)
	{
		GameMode = gameMode;
		SendSetPlayerGameType();
		SendAbilities();
	}


	public void SendSetPlayerGameType()
	{
		McpeSetPlayerGameType playerGameType = McpeSetPlayerGameType.CreateObject();
		playerGameType.gamemode = (int) GameMode;
		SendPacket(playerGameType);
	}

	[Wired]
	public void StrikeLightning()
	{
		var lightning = new Lightning(Level) { KnownPosition = KnownPosition };

		if (lightning.Level == null) return;

		lightning.SpawnEntity();
	}

	public void Disconnect(string reason, bool sendDisconnect = true)
	{
		try
		{
			lock (_disconnectSync)
			{
				if (IsConnected)
				{
					if (Level != null) OnPlayerLeave(new PlayerEventArgs(this));

					if (sendDisconnect)
					{
						McpeDisconnect disconnect = McpeDisconnect.CreateObject();
						disconnect.message = reason;
						NetworkHandler.SendPacket(disconnect);
					}
					NetworkHandler = null;

					IsConnected = false;
				}

				Level?.RemovePlayer(this);

				Session playerSession = Session;
				Session = null;
				if (playerSession != null)
				{
					Server.SessionManager.RemoveSession(playerSession);
					playerSession.Player = null;
				}

				string levelId = Level == null ? "Unknown" : Level.LevelId;
				if (!_haveJoined)
					Log.WarnFormat("Disconnected crashed player {0}/{1} from level <{3}>, reason: {2}", Username, EndPoint.Address, reason, levelId);
				else
					Log.Warn(string.Format("Disconnected player {0}/{1} from level <{3}>, reason: {2}", Username, EndPoint.Address, reason, levelId));

				CleanCache();
			}
		}
		catch (Exception e)
		{
			Log.Error("On disconnect player", e);
			throw;
		}
	}

	public void HandleMcpeText(McpeText message)
	{
		string text = message.message;
		if (string.IsNullOrEmpty(text)) return;
		Level.BroadcastMessage(text, sender: this, platformId: message.platformChatId);
	}

	public void HandleMcpeMovePlayer(McpeMovePlayer message)
	{
		if (!IsSpawned || HealthManager.IsDead) return;

		if (Server.ServerRole != ServerRole.Node)
		{
			lock (_moveSyncLock)
			{
				if (_lastOrderingIndex > message.ReliabilityHeader.OrderingIndex) return;
				_lastOrderingIndex = message.ReliabilityHeader.OrderingIndex;
			}
		}

		var origin = KnownPosition.ToVector3();
		double distanceTo = Vector3.Distance(origin, new Vector3(message.x, message.y - 1.62f, message.z));

		CurrentSpeed = distanceTo / ((double) (DateTime.UtcNow - LastUpdatedTime).Ticks / TimeSpan.TicksPerSecond);

		double verticalMove = message.y - 1.62 - KnownPosition.Y;

		bool isOnGround = IsOnGround;
		bool isFlyingHorizontally = false;
		if (Math.Abs(distanceTo) > 0.01)
		{
			isOnGround = CheckOnGround(new PlayerLocation(message.x, message.y, message.z));
			isFlyingHorizontally = DetectSimpleFly(new PlayerLocation(message.x, message.y, message.z), isOnGround);
		}

		IsFlyingHorizontally = isFlyingHorizontally;
		IsOnGround = isOnGround;

		// Hunger management
		if (!IsGliding) HungerManager.Move(Vector3.Distance(new Vector3(KnownPosition.X, 0, KnownPosition.Z), new Vector3(message.x, 0, message.z)));

		KnownPosition = new PlayerLocation
		{
			X = message.x,
			Y = message.y - 1.62f,
			Z = message.z,
			Pitch = message.pitch,
			Yaw = message.yaw,
			HeadYaw = message.headYaw
		};

		IsFalling = verticalMove < 0 && !IsOnGround && !IsGliding;

		if (IsFalling)
		{
			if (StartFallY == 0) StartFallY = KnownPosition.Y;
		}
		else
		{
			double damage = StartFallY - KnownPosition.Y;
			if ((damage - 3) > 0) 
				HealthManager.TakeHit(null, (int) DamageCalculator.CalculatePlayerDamage(null, this, null, damage, DamageCause.Fall), DamageCause.Fall);
			StartFallY = 0;
		}

		LastUpdatedTime = DateTime.UtcNow;

		var chunkPosition = new ChunkCoordinates(KnownPosition);
		if (_currentChunkPosition != chunkPosition && _currentChunkPosition.DistanceTo(chunkPosition) >= MoveRenderDistance) 
			MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
	}

	private bool DetectSimpleFly(PlayerLocation message, bool isOnGround)
	{
		double d = Math.Abs(KnownPosition.Y - (message.Y - 1.62f));
		return !(AllowFly || IsOnGround || isOnGround || d > 0.001);
	}

	private static readonly int[] Layers = [-1, 0];
	private static readonly int[] Arounds = [0, 1, -1];

	public bool CheckOnGround(PlayerLocation message)
	{
		if (Level == null)
			return true;

		BlockCoordinates pos = new Vector3(message.X, message.Y - 1.62f, message.Z);

		return Layers.Any(layer => Arounds.Any(x => 
			Arounds
				.Select(z => new BlockCoordinates(x, layer, z))
				.Select(offset => Level.GetBlock(pos + offset))
				.Any(block => block.IsSolid)));
	}

	// NOTE: This shouldn't be here I guess, it's handling a level thing but in the player class
	public void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message)
	{
		McpeLevelSoundEventOld sound = McpeLevelSoundEventOld.CreateObject();
		sound.soundId = message.soundId;
		sound.position = message.position;
		sound.blockId = message.blockId;
		sound.entityType = message.entityType;
		sound.isBabyMob = message.isBabyMob;
		sound.isGlobal = message.isGlobal;
		Level.RelayBroadcast(sound);
	}

	public void HandleMcpePlayerAuthInput(McpePlayerAuthInput message)
	{
		CurrentTick = message.Tick;
		if (!PlayerLocation.Equal(KnownPosition, message.Position))
		{
			var origin = KnownPosition.ToVector3();
			double distanceTo = Vector3.Distance(origin, new Vector3(message.Position.X, message.Position.Y - 1.62f, message.Position.Z));

			CurrentSpeed = distanceTo / ((double) (DateTime.UtcNow - LastUpdatedTime).Ticks / TimeSpan.TicksPerSecond);

			double verticalMove = message.Position.Y - 1.62 - KnownPosition.Y;

			bool isOnGround = IsOnGround;
			bool isFlyingHorizontally = false;
			if (Math.Abs(distanceTo) > 0.01)
			{
				isOnGround = CheckOnGround(message.Position);
				isFlyingHorizontally = DetectSimpleFly(message.Position, isOnGround);
			}

			IsFlyingHorizontally = isFlyingHorizontally;
			IsOnGround = isOnGround;

			if (!IsGliding)
				HungerManager.Move(Vector3.Distance(new Vector3(KnownPosition.X, 0, KnownPosition.Z), new Vector3(message.Position.X, 0, message.Position.Z)));

			KnownPosition = new PlayerLocation
			{
				X = message.Position.X,
				Y = message.Position.Y - 1.62f,
				Z = message.Position.Z,
				Pitch = message.Position.Pitch,
				Yaw = message.Position.Yaw,
				HeadYaw = message.Position.HeadYaw
			};

			IsFalling = verticalMove < 0 && !IsOnGround && !IsGliding;

			if (IsFalling)
			{
				if (StartFallY == 0)
					StartFallY = KnownPosition.Y;
			}
			else
			{
				double damage = StartFallY - KnownPosition.Y;
				if ((damage - 3) > 0 && Level.Falldamage) 
					HealthManager.TakeHit(null, (int) DamageCalculator.CalculatePlayerDamage(null, this, null, damage, DamageCause.Fall), DamageCause.Fall);
				StartFallY = 0;
			}

			LastUpdatedTime = DateTime.UtcNow;

			var chunkPosition = new ChunkCoordinates(KnownPosition);
			if (_currentChunkPosition != chunkPosition && _currentChunkPosition.DistanceTo(chunkPosition) >= MoveRenderDistance) 
				MiNetServer.FastThreadPool.QueueUserWorkItem(SendChunksForKnownPosition);
		}

		if (message.InputFlags != LastAuthInputFlag)
		{
			LastAuthInputFlag = message.InputFlags;
			Log.Debug($"updated AuthInputFlags: {message.InputFlags}");

			if ((message.InputFlags & AuthInputFlags.StartSneaking) != 0)
			{
				IsSneaking = true;
				if(Inventory.OffHandInventory.GetItem() is ItemShield)
				{
					IsBlockedWithShield = true;
					IsTransitionBlocking = true;
				}
				BroadcastSetEntityData();
			}

			if ((message.InputFlags & AuthInputFlags.StopSneaking) != 0)
			{
				IsSneaking = false;
				IsBlockedWithShield = false;
				BroadcastSetEntityData();
			}

			if ((message.InputFlags & AuthInputFlags.StartSwimming) != 0)
			{
				IsSwimming = true;
				BroadcastSetEntityData();
			}

			if ((message.InputFlags & AuthInputFlags.StopSwimming) != 0)
			{
				IsSwimming = false;
				BroadcastSetEntityData();
			}

			if ((message.InputFlags & AuthInputFlags.StartSprinting) != 0) SetSprinting(true);

			if ((message.InputFlags & AuthInputFlags.StopSprinting) != 0) SetSprinting(false);

			if ((message.InputFlags & AuthInputFlags.StartGliding) != 0)
			{
				IsGliding = true;
				Height = 0.6;
				BroadcastSetEntityData();
			}

			if ((message.InputFlags & AuthInputFlags.StopGliding) != 0)
			{
				IsGliding = false;
				Height = 1.8;
				BroadcastSetEntityData();
			}

			if ((message.InputFlags & AuthInputFlags.StartJumping) != 0) HungerManager.IncreaseExhaustion(IsSprinting ? 0.2f : 0.05f);
		}

		if (message.Actions == null) return;
		foreach (McpePlayerAuthInput.PlayerBlockActionData action in message.Actions.PlayerBlockAction)
		{
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (action.PlayerActionType)
			{
				case PlayerAction.StartBreak:
				case PlayerAction.ContinueDestroyBlock:
				{
					if (GameMode == GameMode.Survival)
					{
						Block target = Level.GetBlock(action.BlockCoordinates);
						Item tool = Inventory.GetItemInHand();
						Item[] drops = target.GetDrops(tool);

						double toolTypeFactor = drops == null || drops.Length == 0 ? 5 : 1.5;
						toolTypeFactor /= tool.GetBreakingSpeed(target);

						double breakTime = Math.Ceiling(target.Hardness * toolTypeFactor * 20);
						McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
						breakEvent.eventId = 3600;
						breakEvent.position = action.BlockCoordinates;
						breakEvent.data = (int) (65535 / breakTime);
						Level.RelayBroadcast(breakEvent);
					}

					break;
				}
				case PlayerAction.Breaking:
				{
					Block target = Level.GetBlock(action.BlockCoordinates);
					int data = target.GetRuntimeId() | ((byte) (action.Facing << 24));

					McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
					breakEvent.eventId = 2014;
					breakEvent.position = action.BlockCoordinates;
					breakEvent.data = data;
					Level.RelayBroadcast(breakEvent);
					break;
				}
				case PlayerAction.AbortBreak:
				case PlayerAction.StopBreak:
				{
					McpeLevelEvent breakEvent = McpeLevelEvent.CreateObject();
					breakEvent.eventId = 3601;
					breakEvent.position = action.BlockCoordinates;
					Level.RelayBroadcast(breakEvent);
					break;
				}
				case PlayerAction.PredictDestroyBlock:
				{
					Level.BreakBlock(this, action.BlockCoordinates);
					break;
				}
				default:
				{
					Log.Warn($"Unhandled action ID={action.PlayerActionType}");
					throw new ArgumentOutOfRangeException(nameof(action.PlayerActionType));
				}
			}

			BroadcastSetEntityData();
		}
	}

	public void HandleMcpeItemStackRequest(McpeItemStackRequest message)
	{
		McpeItemStackResponse response = McpeItemStackResponse.CreateObject();
		var updatedSlots = new List<StackRequestSlotInfo>();
		response.responses = [];
		foreach (ItemStackActionList request in message.requests)
		{
			var stackResponse = new ItemStackResponse()
			{
				Result = StackResponseStatus.Ok,
				RequestId = request.RequestId,
				ResponseContainerInfos = new List<StackResponseContainerInfo>()
			};

			response.responses.Add(stackResponse);

			try
			{
				StackRequestSlotInfo info = null;
				List<StackResponseContainerInfo> actionList = ItemStackInventoryManager.HandleItemStackActions(request.RequestId, request, ref info);
				stackResponse.ResponseContainerInfos.AddRange(actionList);

				if (info != null) updatedSlots.Add(info);
			}
			catch (Exception e)
			{
				Log.Warn($"Failed to process inventory actions", e);
				stackResponse.Result = StackResponseStatus.Error;
				stackResponse.ResponseContainerInfos.Clear();
			}
		}

		SendPacket(response);

		foreach (StackRequestSlotInfo slot in updatedSlots) Inventory.SendSetSlot(slot.Slot, slot.ContainerId);
	}

	public void HandleMcpeMobEquipment(McpeMobEquipment message)
	{
		if (HealthManager.IsDead) return;

		switch (message.windowsId)
		{
			case 0:
			{
				byte selectedHotbarSlot = message.selectedSlot;
				if (selectedHotbarSlot > 8)
				{
					Log.Error($"Player {Username} called set equipment with held hotbar slot {message.selectedSlot} with item {message.item}");
					return;
				}

				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} called set equipment with held hotbar slot {message.selectedSlot} with item {message.item}, RuntimeID: {message.item.RuntimeId}");

				Inventory.SetHeldItemSlot(selectedHotbarSlot, false);
				if (Log.IsDebugEnabled)
					Log.Debug($"Player {Username} now holding {Inventory.GetItemInHand()} RuntimeID: {Inventory.GetItemInHand().RuntimeId}");
				break;
			}
			case 119 when message.slot != 1:
				Log.Error($"Player {Username} called set equipment with offhand slot {message.slot} with item {message.item}");
				return;
			case 119:
			{
				if (Log.IsDebugEnabled) Log.Debug($"Player {Username} called set equipment with offhand slot {message.slot} with item {message.item}");
				break;
			}
		}
	}
	
	public void SetOpenInventory(IInventory inventory) => _openInventory = inventory;
	
	public void OpenInventory(BlockCoordinates inventoryCoord)
	{
		if (Level.GetBlockEntity(inventoryCoord) != null && !Level.BlockEntities.Contains(Level.GetBlockEntity(inventoryCoord))) { Level.BlockEntities.Add(Level.GetBlockEntity(inventoryCoord)); }
		// https://github.com/pmmp/PocketMine-MP/blob/stable/src/pocketmine/network/mcpe/protocol/types/WindowTypes.php
		lock (_inventorySync)
		{
			if (_openInventory is Inventory openInventory)
			{
				if (openInventory.Coordinates.Equals(inventoryCoord)) return;
				HandleMcpeContainerClose(null);
			}

			// get inventory from coordinates
			// - get blockentity
			// - get inventory from block entity

			Inventory inventory = Level.InventoryManager.GetInventory(inventoryCoord);

			if (inventory == null)
			{
				Log.Warn($"No inventory found at {inventoryCoord}");
				return;
			}

			// get inventory # from inventory manager
			// set inventory as active on player

			_openInventory = inventory;

			if (inventory.Type == 0 && !inventory.IsOpen()) // Chest open animation
			{
				McpeBlockEvent tileEvent = McpeBlockEvent.CreateObject();
				tileEvent.coordinates = inventoryCoord;
				tileEvent.case1 = 1;
				tileEvent.case2 = 2;
				Level.RelayBroadcast(tileEvent);
			}

			// subscribe to inventory changes
			inventory.InventoryChange += OnInventoryChange;
			inventory.AddObserver(this);

			// open inventory

			McpeContainerOpen containerOpen = McpeContainerOpen.CreateObject();
			containerOpen.windowId = inventory.WindowsId;
			containerOpen.type = inventory.Type;
			containerOpen.coordinates = inventoryCoord;
			containerOpen.runtimeEntityId = -1;
			SendPacket(containerOpen);

			McpeInventoryContent containerSetContent = McpeInventoryContent.CreateObject();
			containerSetContent.inventoryId = inventory.WindowsId;
			containerSetContent.input = inventory.Slots;
			SendPacket(containerSetContent);
		}
	}

	private void OnInventoryChange(Player player, Inventory inventory, byte slot, Item itemStack)
	{
		if (player == this)
		{
			//TODO: This needs to be synced to work properly under heavy load (SG).
			//Level.SetBlockEntity(inventory.BlockEntity, false);
		}
		else
		{
			McpeInventorySlot sendSlot = McpeInventorySlot.CreateObject();
			sendSlot.inventoryId = inventory.WindowsId;
			sendSlot.slot = slot;
			sendSlot.item = itemStack;
			SendPacket(sendSlot);
		}
	}

	public void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
	{
		switch (message.transaction)
		{
			case InventoryMismatchTransaction inventoryMismatchTransaction:
				HandleInventoryMismatchTransaction(inventoryMismatchTransaction);
				break;
			case ItemReleaseTransaction itemReleaseTransaction:
				HandleItemReleaseTransaction(itemReleaseTransaction);
				break;
			case ItemUseOnEntityTransaction itemUseOnEntityTransaction:
				HandleItemUseOnEntityTransaction(itemUseOnEntityTransaction);
				break;
			case ItemUseTransaction itemUseTransaction:
				HandleItemUseTransaction(itemUseTransaction);
				break;
			case NormalTransaction normalTransaction:
				HandleNormalTransaction(normalTransaction);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void HandleItemUseOnEntityTransaction(ItemUseOnEntityTransaction transaction)
	{
		switch (transaction.ActionType)
		{
			case McpeInventoryTransaction.ItemUseOnEntityAction.Interact: // Right click
				EntityInteract(transaction);
				break;
			case McpeInventoryTransaction.ItemUseOnEntityAction.Attack: // Left click
				EntityAttack(transaction);
				break;
			case McpeInventoryTransaction.ItemUseOnEntityAction.ItemInteract:
				Log.Warn($"Got Entity ItemInteract. Was't sure it existed, but obviously it does :-o");
				EntityItemInteract(transaction);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void EntityItemInteract(ItemUseOnEntityTransaction transaction)
	{
		Item itemInHand = Inventory.GetItemInHand();
		if (itemInHand.Id != transaction.Item.Id || itemInHand.Metadata != transaction.Item.Metadata) 
			Log.Warn($"Attack item mismatch. Expected {itemInHand}, but client reported {transaction.Item}");
		
		if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;
		target.DoItemInteraction(this, itemInHand);
	}

	private void EntityInteract(ItemUseOnEntityTransaction transaction)
	{
		DoInteraction((int) transaction.ActionType, this);

		if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;
		target.DoInteraction((int) transaction.ActionType, this);
	}

	private void EntityAttack(ItemUseOnEntityTransaction transaction)
	{
		Item itemInHand = Inventory.GetItemInHand();
		if (itemInHand.Id != transaction.Item.Id || itemInHand.Metadata != transaction.Item.Metadata)
			Log.Warn($"Attack item mismatch. Expected {itemInHand}, but client reported {transaction.Item}");

		if (!Level.TryGetEntity(transaction.EntityId, out Entity target)) return;


		LastAttackTarget = target;

		if (target is Player player)
		{
			if (!OnPlayerDamageToPlayer(new PlayerDamageToPlayerEventArgs(player, this))) return;
		}
		else
		{
			if (!OnPlayerDamageToEntity(new PlayerDamageToEntityEventArgs(target, this)) && target != null) return;
		}

		double damage = DamageCalculator.CalculateItemDamage(itemInHand);

		if (IsFalling) damage += DamageCalculator.CalculateFallDamage(this, damage, target);

		damage += DamageCalculator.CalculateEffectDamage(this);

		if (damage < 0) damage = 0;

		damage += DamageCalculator.CalculateDamageIncreaseFromEnchantments(this, itemInHand, target);

		int reducedDamage = (int) DamageCalculator.CalculatePlayerDamage(this, target, itemInHand, damage, DamageCause.EntityAttack);

		target?.HealthManager.TakeHit(this, itemInHand, reducedDamage, DamageCause.EntityAttack);

		short fireAspectLevel = itemInHand.GetEnchantingLevel(EnchantingType.FireAspect);
		if (fireAspectLevel > 0) target?.HealthManager.Ignite(fireAspectLevel * 80);

		Inventory.DamageItemInHand(ItemDamageReason.EntityAttack, target, null);
		HungerManager.IncreaseExhaustion(0.1f);
	}

	// ReSharper disable once UnusedParameter.Local
	private void HandleInventoryMismatchTransaction(InventoryMismatchTransaction transaction) => Log.Warn("Transaction mismatch");

	private void HandleItemReleaseTransaction(ItemReleaseTransaction transaction)
	{
		Item itemInHand = Inventory.GetItemInHand();

		switch (transaction.ActionType)
		{
			case McpeInventoryTransaction.ItemReleaseAction.Release:
			{
				itemInHand.Release(Level, this, transaction.FromPosition);
				break;
			}
			case McpeInventoryTransaction.ItemReleaseAction.Use:
			{
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
		}
		IsUsingItem = false;
		BroadcastSetEntityData();

		HandleTransactionRecords(transaction.TransactionRecords);
	}

	private void HandleItemUseTransaction(ItemUseTransaction transaction)
	{
		Item itemInHand = Inventory.GetItemInHand();

		switch (transaction.ActionType)
		{
			case McpeInventoryTransaction.ItemUseAction.Place:
			{
				Level.Interact(this, itemInHand, transaction.Position, (BlockFace) transaction.Face, transaction.ClickPosition);
				break;
			}
			case McpeInventoryTransaction.ItemUseAction.Use:
			{
				itemInHand.UseItem(Level, this, transaction.Position);
				if (itemInHand is not ItemBlock) BroadcastSetEntityData();
				break;
			}
			case McpeInventoryTransaction.ItemUseAction.Destroy:
			{
				//TODO: Add face and other parameters to break. For logic in break block.
				Level.BreakBlock(this, transaction.Position, (BlockFace) transaction.Face);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
		}

		HandleTransactionRecords(transaction.TransactionRecords);
	}

	private void HandleNormalTransaction(NormalTransaction transaction) => HandleTransactionRecords(transaction.TransactionRecords);

	private void HandleTransactionRecords(List<TransactionRecord> records)
	{
		if (records.Count == 0) return;

		foreach (TransactionRecord record in records)
		{
			//Item oldItem = record.OldItem;
			Item newItem = record.NewItem;
			//int slot = record.Slot;

			switch (record)
			{
				//TODO Handle custom items, like player inventory and cursor. Not entirely sure how to handle this for crafting and similar inventories.
				case CraftTransactionRecord:
				{
					throw new Exception("This should never happen with new inventory transactions");
				}
				case CreativeTransactionRecord:
				{
					throw new Exception("This should never happen with new inventory transactions");
				}
				case WorldInteractionTransactionRecord:
				{
					// Drop
					Item sourceItem = Inventory.GetItemInHand();

					if (!OnItemDrop(new ItemDropEventArgs(this, Level, sourceItem))) Inventory.SendSetSlot(Inventory.InHandSlot);
					else
					{
						if (newItem.Id != sourceItem.Id) Log.Warn($"Inventory mismatch. Client reported drop item as {newItem} and it did not match existing item {sourceItem}");

						byte count = newItem.Count;

						Item dropItem;
						if (sourceItem.Count == count)
						{
							dropItem = sourceItem;
							Inventory.ClearInventorySlot((byte) Inventory.InHandSlot);
						}
						else
						{
							dropItem = (Item) sourceItem.Clone();
							sourceItem.Count -= count;
							dropItem.Count = count;
							dropItem.UniqueId = Environment.TickCount;
						}

						DropItem(dropItem);
					}
					break;
				}
			}
		}
	}

	public readonly List<byte[]> HiddenPlayers = [];
	private McpeSetPlayerGameType gameType;

	public void HidePlayer(Player player)
	{
		if (player == this) return;
		if (HiddenPlayers.Contains(player.PlayerInfo.ClientUuid.GetBytes())) return;
		HiddenPlayers.Add(player.PlayerInfo.ClientUuid.GetBytes());
		player.DespawnFromPlayers([this]);
	}

	public void ShowPlayer(Player player)
	{
		if (player == this) return;
		if (!HiddenPlayers.Contains(player.PlayerInfo.ClientUuid.GetBytes())) return;
		HiddenPlayers.Remove(player.PlayerInfo.ClientUuid.GetBytes());
		// TODO: Fix this, it doesn't show the player
		if(player.IsConnected) player.SpawnToPlayers([this]);
	}

	public ItemEntity DropItem(Item item)
	{
		var itemEntity = new ItemEntity(Level, item)
		{
			Velocity = KnownPosition.GetDirection().Normalize() * 0.3f,
			KnownPosition = KnownPosition + new Vector3(0f, 1.62f, 0f)
		};
		itemEntity.SpawnEntity();

		return itemEntity;
	}

	public bool PickUpItem(ItemEntity item) => Inventory.SetFirstEmptySlot(item.Item, true);

	public void HandleMcpeContainerClose(McpeContainerClose message)
	{
		UsingAnvil = false;

		lock (_inventorySync)
		{
			switch (_openInventory)
			{
				case Inventory inventory:
				{
					_openInventory = null;

					// unsubscribe to inventory changes
					inventory.InventoryChange -= OnInventoryChange;
					inventory.RemoveObserver(this);

					if (message != null && message.windowId != inventory.WindowsId) return;

					// close container 
					if (inventory.Type == 0 && !inventory.IsOpen())
					{
						McpeBlockEvent tileEvent = McpeBlockEvent.CreateObject();
						tileEvent.coordinates = inventory.Coordinates;
						tileEvent.case1 = 1;
						tileEvent.case2 = 0;
						Level.RelayBroadcast(tileEvent);
					}

					McpeContainerClose closePacket = McpeContainerClose.CreateObject();
					closePacket.windowId = inventory.WindowsId;
					closePacket.server = message == null;
					SendPacket(closePacket);

					Block block = Level.GetBlock(inventory.Coordinates);
					switch (block)
					{
						case Chest or TrappedChest:
							Level.BroadcastSound(inventory.Coordinates, LevelSoundEventType.ChestClosed);
							break;
						case EnderChest:
							Level.BroadcastSound(inventory.Coordinates, LevelSoundEventType.EnderchestClosed);
							break;
						case ShulkerBox:
							Level.BroadcastSound(inventory.Coordinates, LevelSoundEventType.ShulkerboxClosed);
							break;
						case Barrel:
							Level.BroadcastSound(inventory.Coordinates, LevelSoundEventType.BlockBarrelClose);
							break;
					}
					break;
				}
				case HorseInventory:
					_openInventory = null;
					break;
				default:
				{
					McpeContainerClose closePacket = McpeContainerClose.CreateObject();
					closePacket.windowId = 0;
					closePacket.server = message == null;
					SendPacket(closePacket);
					break;
				}
			}
		}
	}

	public void HandleMcpeEmote(McpeEmotePacket message)
	{
		McpeEmotePacket msg = McpeEmotePacket.CreateObject();
		msg.runtimeEntityId = EntityId;
		msg.xuid = message.xuid;
		msg.platformId = message.platformId;
		msg.emoteId = message.emoteId;
		msg.flags = McpeEmotePacket.FlagServer | McpeEmotePacket.MuteAnnouncement;
		Level.RelayBroadcast(this, msg);
	}

	public void HandleMcpePermissionRequest(McpePermissionRequest message)
	{
		//TODO Figure out how to get player from runtimeId to send abilities packet

		switch (message.permission)
		{
			case 0:
			{
				ActionPermissions = 0;
				CommandPermission = 0;
				PermissionLevel = PermissionLevel.Visitor;
				SendAbilities();
				break;
			}
			case 2:
			{
				ActionPermissions = ActionPermissions.Default;
				CommandPermission = 0;
				PermissionLevel = PermissionLevel.Member;
				SendAbilities();
				break;
			}
			case 4:
			{
				ActionPermissions = ActionPermissions.All;
				CommandPermission = 4;
				PermissionLevel = PermissionLevel.Operator;
				SendAbilities();
				break;
			}
		}
	}

	/// <summary>
	///     Handles the interact.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public void HandleMcpeInteract(McpeInteract message)
	{
		Entity target;
		long runtimeEntityId = message.targetRuntimeEntityId;
		if (runtimeEntityId == EntityManager.EntityIdSelf) target = this;
		else if (!Level.TryGetEntity(runtimeEntityId, out target)) return;

		if (message.actionId != 4)
		{
			Log.Debug($"Interact Action ID: {message.actionId}");
			Log.Debug($"Interact Target Entity ID: {runtimeEntityId}");
		}

		if (target == null) return;
		switch ((McpeInteract.Actions) message.actionId)
		{
			case McpeInteract.Actions.LeaveVehicle:
			{
				if (Level.TryGetEntity(Vehicle, out Mob mob)) mob.Unmount(this);

				break;
			}
			case McpeInteract.Actions.MouseOver:
			{
				// Mouse over
				DoMouseOverInteraction(message.actionId, this);
				target.DoMouseOverInteraction(message.actionId, this);
				break;
			}
			case McpeInteract.Actions.OpenInventory:
			{
				if (target == this)
				{
					McpeContainerOpen containerOpen = McpeContainerOpen.CreateObject();
					containerOpen.windowId = 0;
					containerOpen.type = 255;
					containerOpen.runtimeEntityId = EntityManager.EntityIdSelf;
					SendPacket(containerOpen);
				}
				else if (IsRiding) // Riding; Open inventory
				{
					if (Level.TryGetEntity(Vehicle, out Mob mob) && mob is Horse horse) horse.Inventory.Open(this);
				}

				break;
			}
			case McpeInteract.Actions.RightClick:
				break;
			case McpeInteract.Actions.LeftClick:
				break;
			case McpeInteract.Actions.OpenNpc:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void HandleMcpeBlockPickRequest(McpeBlockPickRequest message)
	{
		if (GameMode != GameMode.Creative) return;
		Block block = Level.GetBlock(message.x, message.y, message.z);
		Log.Debug($"Picked block {block.Id}:{block.Metadata} from blockstate {block.GetRuntimeId()}. Expected block to be in slot {message.selectedSlot}");
		int id = block.Id;
		if (id > 255) id = -(id - 255);
		Item item = ItemFactory.GetItem((short) id, block.Metadata);
		switch (item)
		{
			case ItemBlock blockItem:
				Log.Debug($"Have BlockItem with block state {blockItem.Block.GetRuntimeId()}");
				break;
			case null:
				return;
		}

		Inventory.SetInventorySlot(Inventory.InHandSlot, item, true);
	}

	public void HandleMcpeTakeItemActor(McpeEntityPickRequest message)
	{
		if (GameMode != GameMode.Creative) return;
		if (!Level.Entities.TryGetValue((long) message.runtimeEntityId, out Entity entity)) return;
		Item item = ItemFactory.GetItem("minecraft:spawn_egg", (short) EntityHelpers.ToEntityType(entity.EntityTypeId));
		Inventory.SetInventorySlot(Inventory.InHandSlot, item);
	}
	
	public void HandleMcpeEntityEvent(McpeEntityEvent message)
	{
		// NOTE: Is this really used ? Tried to trigger it, never saw any console logs. 2025-03-02
		Log.Debug("Entity Id:" + message.runtimeEntityId);
		Log.Debug("Entity Event Id:" + message.eventId);
		Log.Debug("Entity Event unknown:" + message.data);
			
		switch (message.eventId)
		{
			case 34:
			{
				ExperienceManager.RemoveExperienceLevels(message.data);
				break;
			}
			case 57:
			{
				int data = message.data;
				if (data != 0) BroadcastEntityEvent(57, data);
				break;
			}
		}
	}

	public void ActuallyRespawn()
	{
		SetSprinting(false);
		SetSneaking(false);
		SetFlying(false);
		//HealthManager.Extinguish();
		RemoveAllEffects();
		HealthManager.ResetHealth();
		HealthManager.CooldownTick = 60;
		Level.SpawnToAll(this);
	}

	public void SetSneaking(bool status)
	{
		IsSneaking = status;
		SendUpdateAttributes();
	}

	public void SetFlying(bool status)
	{
		IsFlying = status;
		SendUpdateAttributes();
	}

	public void SendRespawn()
	{
		McpeRespawn mcpeRespawn = McpeRespawn.CreateObject();
		mcpeRespawn.runtimeEntityId = EntityId;
		mcpeRespawn.x = SpawnPosition.X;
		mcpeRespawn.y = SpawnPosition.Y;
		mcpeRespawn.z = SpawnPosition.Z;
		SendPacket(mcpeRespawn);
	}

	public void SendStartGame()
	{
		var levelSettings = new LevelSettings
		{
			spawnSettings = new SpawnSettings
			{
				Dimension = (int) (Level?.Dimension ?? 0),
				BiomeName = "",
				BiomeType = 0
			},
			seed = 12345,
			generator = 1,
			gamemode = (int) GameMode,
			x = (int) SpawnPosition.X,
			y = (int) (SpawnPosition.Y + Height),
			z = (int) SpawnPosition.Z,
			hasAchievementsDisabled = true,
			time = (int) Level!.WorldTime,
			eduOffer = PlayerInfo.Edition == 1 ? 1 : 0,
			rainLevel = Level.rainLevel,
			lightningLevel = 0,
			isMultiplayer = true,
			broadcastToLan = true,
			enableCommands = EnableCommands,
			isTexturepacksRequired = false,
			gamerules = Level.GetGameRules(),
			bonusChest = false,
			mapEnabled = false,
			permissionLevel = (byte) PermissionLevel,
			gameVersion = McpeProtocolInfo.GameVersion,
			hasEduFeaturesEnabled = true,
			onlySpawnV1Villagers = false,
			emoteChatMuted = true
		};

		McpeStartGame startGame = McpeStartGame.CreateObject();
		startGame.levelSettings = levelSettings;
		startGame.entityIdSelf = EntityId;
		startGame.runtimeEntityId = EntityManager.EntityIdSelf;
		startGame.playerGamemode = (int) GameMode;
		startGame.spawn = SpawnPosition;
		startGame.rotation = new Vector2(KnownPosition.HeadYaw, KnownPosition.Pitch);
		startGame.levelId = "1m0AAMIFIgA=";
		startGame.worldName = Level.LevelName;
		startGame.premiumWorldTemplateId = "";
		startGame.isTrial = false;
		startGame.currentTick = Level.TickTime;
		startGame.enchantmentSeed = 123456;
		if (Config.GetProperty("ServerAuthoritativeMovement", true))
		{
			startGame.movementType = 3;
			startGame.movementRewindHistorySize = 40;
			startGame.enableNewBlockBreakSystem = true;
		}

		startGame.enableNewInventorySystem = true;
		startGame.blockPaletteChecksum = 0;
		startGame.serverVersion = McpeProtocolInfo.GameVersion;
		startGame.propertyData = new Nbt
		{
			NbtFile = new NbtFile
			{
				BigEndian = false,
				UseVarInt = true,
				RootTag = new NbtCompound("")
			}
		};
		startGame.worldTemplateId = new UUID(Guid.Empty.ToByteArray());

		SendPacket(startGame);
	}

	public void SendItemComponents()
	{
		McpeItemComponent itemComponent = McpeItemComponent.CreateObject();
		itemComponent.entries = ItemFactory.Itemstates;
		SendPacket(itemComponent);
	}
	
	public void SendSetSpawnPosition()
	{
		McpeSetSpawnPosition mcpeSetSpawnPosition = McpeSetSpawnPosition.CreateObject();
		mcpeSetSpawnPosition.spawnType = 1;
		mcpeSetSpawnPosition.coordinates = (BlockCoordinates) SpawnPosition;
		SendPacket(mcpeSetSpawnPosition);
	}

	private void ForcedSendChunk(PlayerLocation position)
	{
		lock (_sendChunkSync)
		{
			var chunkPosition = new ChunkCoordinates(position);
			McpeWrapper chunk = null;

			foreach (ChunkColumn cachedChunk in Level.GetLoadedChunks())
				if (cachedChunk.X == chunkPosition.X && cachedChunk.Z == chunkPosition.Z) chunk = cachedChunk.GetBatch();

			_chunksUsed.TryAdd(chunkPosition, chunk);

			if (chunk != null) SendPacket(chunk);
		}
	}

	public void SendNetworkChunkPublisherUpdate()
	{
		McpeNetworkChunkPublisherUpdate pk = McpeNetworkChunkPublisherUpdate.CreateObject();
		pk.coordinates = KnownPosition.GetCoordinates3D();
		pk.radius = (uint) (MaxViewDistance * 16);
		SendPacket(pk);
	}

	public void ForcedSendChunks(Action postAction = null)
	{
		Monitor.Enter(_sendChunkSync);
		try
		{
			var chunkPosition = new ChunkCoordinates(KnownPosition);

			_currentChunkPosition = chunkPosition;

			if (Level == null) return;

			SendNetworkChunkPublisherUpdate();
			int packetCount = 0;
			foreach (McpeWrapper chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius))
			{
				if (chunk != null) SendPacket(chunk);

				if (++packetCount % 16 == 0) Thread.Sleep(12);
			}
		}
		finally
		{
			Monitor.Exit(_sendChunkSync);
		}

		postAction?.Invoke();
	}

	private void SendChunksForKnownPosition()
	{
		if (!Monitor.TryEnter(_sendChunkSync)) return;

		try
		{
			if (ChunkRadius <= 0) return;


			var chunkPosition = new ChunkCoordinates(KnownPosition);
			switch (IsSpawned)
			{
				case true when _currentChunkPosition == chunkPosition:
				case true when _currentChunkPosition.DistanceTo(chunkPosition) < MoveRenderDistance:
					return;
			}

			_currentChunkPosition = chunkPosition;

			int packetCount = 0;

			if (Level == null) return;

			SendNetworkChunkPublisherUpdate();

			foreach (McpeWrapper chunk in Level.GenerateChunks(_currentChunkPosition, _chunksUsed, ChunkRadius, () => KnownPosition))
			{
				if (chunk != null) SendPacket(chunk);
				if (++packetCount % 16 == 0) Thread.Sleep(12);
				if (!IsSpawned && packetCount == 56) InitializePlayer();
			}

			Log.Debug($"Sent {packetCount} chunks for {chunkPosition} with view distance {MaxViewDistance}");
		}
		catch (Exception e)
		{
			Log.Error($"Failed sending chunks for {KnownPosition}", e);
		}
		finally
		{
			Monitor.Exit(_sendChunkSync);
		}
	}

	public void SendUpdateAttributes()
	{
		var attributes = new PlayerAttributes
		{
			["minecraft:attack_damage"] = new()
			{
				Name = "minecraft:attack_damage",
				MinValue = 1,
				MaxValue = 1,
				Value = 1,
				Default = 1,
				Modifiers = new AttributeModifiers()
			},
			["minecraft:absorption"] = new()
			{
				Name = "minecraft:absorption",
				MinValue = 0,
				MaxValue = float.MaxValue,
				Value = HealthManager.Absorption,
				Default = 0,
				Modifiers = new AttributeModifiers()
			},
			["minecraft:health"] = new()
			{
				Name = "minecraft:health",
				MinValue = 0,
				MaxValue = HealthManager.MaxHearts,
				Value = HealthManager.Hearts,
				Default = HealthManager.MaxHearts,
				Modifiers = new AttributeModifiers()
			},
			["minecraft:movement"] = new()
			{
				Name = "minecraft:movement",
				MinValue = 0,
				MaxValue = 0.5f,
				Value = MovementSpeed,
				Default = MovementSpeed,
				Modifiers = new AttributeModifiers()
			},
			["minecraft:knockback_resistance"] = new()
			{
				Name = "minecraft:knockback_resistance",
				MinValue = 0,
				MaxValue = 1,
				Value = 0,
				Default = 0,
				Modifiers = new AttributeModifiers()
			},
			["minecraft:luck"] = new()
			{
				Name = "minecraft:luck",
				MinValue = -1025,
				MaxValue = 1024,
				Value = 0,
				Default = 0,
				Modifiers = new AttributeModifiers()
			},
			["minecraft:follow_range"] = new()
			{
				Name = "minecraft:follow_range",
				MinValue = 0,
				MaxValue = 2048,
				Value = 16,
				Default = 16,
				Modifiers = new AttributeModifiers()
			}
		};
		// Workaround, bad design.
		attributes = HungerManager.AddHungerAttributes(attributes);
		attributes = ExperienceManager.AddExperienceAttributes(attributes);

		McpeUpdateAttributes updateAttributes = McpeUpdateAttributes.CreateObject();
		updateAttributes.runtimeEntityId = EntityManager.EntityIdSelf;
		updateAttributes.attributes = attributes;
		updateAttributes.tick = CurrentTick;
		SendPacket(updateAttributes);
	}

	public void SendForm(Form form)
	{
		CurrentForm = form;

		McpeModalFormRequest message = McpeModalFormRequest.CreateObject();
		message.formId = form.Id;
		message.formData = form.ToJson();
		SendPacket(message);
	}

	public void SendSetTime()
	{
		SendSetTime((int) Level.WorldTime);
	}

	public void SendSetTime(int time)
	{
		McpeSetTime message = McpeSetTime.CreateObject();
		message.time = time;
		SendPacket(message);
	}

	public void SendSound(Sound sound)
	{
		SendSound(sound.Position, (LevelSoundEventType) sound.Id);
	}

	public void SendSound(BlockCoordinates position, LevelSoundEventType sound, int blockId = 0)
	{
		McpeLevelSoundEvent packet = McpeLevelSoundEvent.CreateObject();
		packet.position = position;
		packet.soundId = (uint) sound;
		packet.blockId = blockId;
		SendPacket(packet);
	}

	public void SendSetDownfall(int downfall)
	{
		McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
		levelEvent.eventId = 3001;
		levelEvent.data = downfall;
		SendPacket(levelEvent);
	}
	
	public void SendTitle(string text, TitleType type = TitleType.Title, int fadeIn = 6, int fadeOut = 6, int stayTime = 20, Player sender = null)
		=> Level.BroadcastTitle(text, type, fadeIn, fadeOut, stayTime, sender, [this]);
	
	public void SendMessage(string text, MessageType type = MessageType.Chat, Player sender = null, bool needsTranslation = false, string[] parameters = null, string platformId = null) 
		=> Level.BroadcastMessage(text, type, sender, [this], needsTranslation, parameters, platformId: platformId);

	public void SendMovePlayer(bool teleport = false)
	{
		McpeMovePlayer packet = McpeMovePlayer.CreateObject();
		packet.runtimeEntityId = EntityManager.EntityIdSelf;
		packet.x = KnownPosition.X;
		packet.y = KnownPosition.Y + 1.62f;
		packet.z = KnownPosition.Z;
		packet.yaw = KnownPosition.Yaw;
		packet.headYaw = KnownPosition.HeadYaw;
		packet.pitch = KnownPosition.Pitch;
		packet.mode = (byte) (teleport ? 1 : 0);

		SendPacket(packet);
	}

	public override void OnTick(Entity[] entities)
	{
		OnTicking(new PlayerEventArgs(this));

		if (DetectInPortal())
		{
			if (PortalDetected == Level.TickTime)
			{
				PortalDetected = -1;

				Dimension dimension = Level.Dimension == Dimension.Overworld ? Dimension.Nether : Dimension.Overworld;
				Log.Debug($"Dimension change to {dimension} from {Level.Dimension} initiated, Game mode={GameMode}");

				ThreadPool.QueueUserWorkItem(delegate
				{
					Level oldLevel = Level;

					ChangeDimension(null, null, dimension, delegate
					{
						Level nextLevel = dimension switch
						{
							Dimension.Overworld => oldLevel.OverworldLevel,
							Dimension.Nether => oldLevel.NetherLevel,
							_ => oldLevel.TheEndLevel
						};
						return nextLevel;
					});
				});
			}
			else if (PortalDetected == 0) PortalDetected = Level.TickTime + (GameMode == GameMode.Creative ? 1 : 4 * 20);
		}
		else
		{
			if (PortalDetected != 0) Log.Debug($"Reset portal detected");
			if (IsSpawned) PortalDetected = 0;
		}

		HungerManager.OnTick();

		base.OnTick(entities);

		if (LastAttackTarget != null && LastAttackTarget.HealthManager.IsDead) LastAttackTarget = null;
		foreach (KeyValuePair<EffectType, Effect> effect in Effects) effect.Value.OnTick(this);

		bool hasDisplayedPopup = false;
		bool hasDisplayedTip = false;
		
		// ReSharper disable once RemoveRedundantBraces
		lock (Popups)
		{
			foreach (Popup popup in Popups.OrderByDescending(p => p.Priority).ThenByDescending(p => p.CurrentTick))
			{
				if (popup.CurrentTick >= popup.Duration + popup.DisplayDelay)
				{
					Popups.Remove(popup);
					continue;
				}

				if (popup.CurrentTick >= popup.DisplayDelay)
				{
					// Tip is ontop
					if (popup.MessageType == MessageType.Tip && !hasDisplayedTip)
					{
						if (popup.CurrentTick <= popup.Duration + popup.DisplayDelay - 30)
							if (popup.CurrentTick % 20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30)
								SendMessage(popup.Message, type: popup.MessageType);
						hasDisplayedTip = true;
					}

					// Popup is below
					if (popup.MessageType == MessageType.Popup && !hasDisplayedPopup)
					{
						if (popup.CurrentTick <= popup.Duration + popup.DisplayDelay - 30)
							if (popup.CurrentTick % 20 == 0 || popup.CurrentTick == popup.Duration + popup.DisplayDelay - 30)
								SendMessage(popup.Message, type: popup.MessageType);
						hasDisplayedPopup = true;
					}
				}

				popup.CurrentTick++;
			}
		}

		OnTicked(new PlayerEventArgs(this));
	}

	public void AddPopup(Popup popup)
	{
		lock (Popups)
		{
			if (popup.Id == 0) popup.Id = popup.Message.GetHashCode();
			Popup exist = Popups.FirstOrDefault(pop => pop.Id == popup.Id);
			if (exist != null) Popups.Remove(exist);

			Popups.Add(popup);
		}
	}

	public void ClearPopups()
	{
		lock (Popups) Popups.Clear();
	}

	public override void Knockback(Vector3 velocity)
	{
		McpeSetActorMotion motions = McpeSetActorMotion.CreateObject();
		motions.runtimeEntityId = EntityManager.EntityIdSelf;
		motions.velocity = velocity;
		motions.tick = CurrentTick;
		SendPacket(motions);
	}

	public string ButtonText { get; set; }

	public override MetadataDictionary GetMetadata()
	{
		MetadataDictionary metadata = base.GetMetadata();
		metadata[(int) MetadataFlags.NameTag] = new MetadataString(NameTag ?? Username);
		metadata[(int) MetadataFlags.PlayerFlags] = new MetadataByte((byte) (IsSleeping ? 0b10 : 0));
		metadata[(int) MetadataFlags.BedPosition] = new MetadataIntCoordinates((int) SpawnPosition.X, (int) SpawnPosition.Y, (int) SpawnPosition.Z);
		return metadata;
	}

	[Wired]
	public void SetNoAi(bool noAi)
	{
		NoAi = noAi;
		BroadcastSetEntityData();
	}

	[Wired]
	public void SetHideNameTag(bool hideNameTag)
	{
		HideNameTag = hideNameTag;
		BroadcastSetEntityData();
	}

	[Wired]
	public void SetNameTag(string nameTag)
	{
		NameTag = nameTag;
		BroadcastSetEntityData();
	}

	[Wired]
	public void SetDisplayName(string displayName)
	{
		// TODO: Fix this
		DisplayName = displayName;

		McpePlayerList playerList = McpePlayerList.CreateObject();
		playerList.records = new PlayerRemoveRecords { this };

		Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.Encode())); // Replace with records, to remove need for player and encode
		playerList.records = null;
		playerList.PutPool();

		playerList = McpePlayerList.CreateObject();
		playerList.records = new PlayerAddRecords { this };
		Level.RelayBroadcast(Level.CreateMcpeBatch(playerList.Encode())); // Replace with records, to remove need for player and encode
		playerList.records = null;
		playerList.PutPool();
	}

	[Wired]
	public void SetEffect(Effect effect, bool ignoreIfLowerLevel = false)
	{
		if (Effects.TryGetValue(effect.EffectId, out Effect effect1))
		{
			if (ignoreIfLowerLevel && effect1.Level > effect.Level) return;
			effect.SendUpdate(this);
		}
		else effect.SendAdd(this);

		Effects[effect.EffectId] = effect;

		UpdatePotionColor();
	}

	[Wired]
	public void RemoveEffect(Effect effect, bool recalcColor = true)
	{
		if (Effects.ContainsKey(effect.EffectId))
		{
			effect.SendRemove(this);
			Effects.TryRemove(effect.EffectId, out effect);
		}


		if (recalcColor) UpdatePotionColor();
	}

	[Wired]
	public void RemoveAllEffects()
	{
		foreach (KeyValuePair<EffectType, Effect> effect in Effects) RemoveEffect(effect.Value, false);
		UpdatePotionColor();
	}

	public void UpdatePotionColor()
	{
		if (Effects.IsEmpty)
			PotionColor = 0;
		else
		{
			int r = 0, g = 0, b = 0;
			int levels = 0;
			foreach (Effect effect in Effects.Values)
			{
				if (!effect.Particles) continue;

				Color color = effect.ParticleColor;
				int level = effect.Level + 1;
				r += color.R * level;
				g += color.G * level;
				b += color.B * level;
				levels += level;
			}

			if (levels == 0)
				PotionColor = 0;
			else
			{
				r /= levels;
				g /= levels;
				b /= levels;

				PotionColor = (int) (0xff000000 | (r << 16) | (uint) (g << 8) | (uint) b);
			}
		}

		BroadcastSetEntityData();
	}

	public override void DespawnEntity()
	{
		IsSpawned = false;
		Level.DespawnFromAll(this);
	}

	public override void BroadcastEntityEvent()
	{
		BroadcastEntityEvent(HealthManager.Health <= 0 ? 3 : 2);

		if (!HealthManager.IsDead || !Level.DoShowDeathMessage) return;
		var player = HealthManager.LastDamageSource as Player;
		BroadcastDeathMessage(player, HealthManager.LastDamageCause);
	}

	public void BroadcastEntityEvent(int eventId, int data = 0)
	{
		{
			McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
			entityEvent.runtimeEntityId = EntityManager.EntityIdSelf;
			entityEvent.eventId = (byte) eventId;
			entityEvent.data = data;
			SendPacket(entityEvent);
		}
		{
			McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
			entityEvent.runtimeEntityId = EntityId;
			entityEvent.eventId = (byte) eventId;
			entityEvent.data = data;
			Level.RelayBroadcast(this, entityEvent);
		}
	}

	public void BroadcastDeathMessage(Player player, DamageCause lastDamageCause)
	{
		string deathMessage = string.Format(HealthManager.GetDescription(lastDamageCause), Username, player == null ? "" : player.Username);
		Level.BroadcastMessage(deathMessage, type: MessageType.Raw);
		Log.Debug(deathMessage);
	}
	
	public void SendPacket(Packet packet)
	{
		if (NetworkHandler == null)
			packet.PutPool();
		else
			NetworkHandler?.SendPacket(packet);
	}

	public void SendMoveList(McpeWrapper batch, DateTime sendTime)
	{
		if (sendTime < _lastMoveListSendTime || !Monitor.TryEnter(_sendMoveListSync))
		{
			batch.PutPool();
			return;
		}

		_lastMoveListSendTime = sendTime;

		try
		{
			SendPacket(batch);
		}
		finally
		{
			Monitor.Exit(_sendMoveListSync);
		}
	}

	public void CleanCache()
	{
		lock (_sendChunkSync) _chunksUsed.Clear();
	}

	public void CleanCache(ChunkColumn chunk)
	{
		lock (_sendChunkSync) _chunksUsed.Remove(new ChunkCoordinates(chunk.X, chunk.Z));
	}

	public void DropInventory()
	{
		List<Item> slots = Inventory.Slots;
		List<Item> uiSlots = Inventory.UiInventory.Slots;

		var coordinates = KnownPosition.ToVector3();
		coordinates.Y += 0.5f;

		foreach (Item stack in slots.ToArray()) Level.DropItem(coordinates, stack);

		foreach (Item stack in uiSlots.ToArray()) Level.DropItem(coordinates, stack);

		if (Inventory.ArmorInventory.GetHeadItem().Id != 0) Level.DropItem(coordinates, Inventory.ArmorInventory.GetHeadItem());
		if (Inventory.ArmorInventory.GetChestItem().Id != 0) Level.DropItem(coordinates, Inventory.ArmorInventory.GetChestItem());
		if (Inventory.ArmorInventory.GetLegsItem().Id != 0) Level.DropItem(coordinates, Inventory.ArmorInventory.GetLegsItem());
		if (Inventory.ArmorInventory.GetFeetItem().Id != 0) Level.DropItem(coordinates, Inventory.ArmorInventory.GetFeetItem());
		
		Inventory.ArmorInventory.Clear();
		Inventory.Clear();
	}

	public override void SpawnToPlayers(Player[] players)
	{
		McpeAddPlayer mcpeAddPlayer = McpeAddPlayer.CreateObject();
		mcpeAddPlayer.uuid = ClientUuid;
		mcpeAddPlayer.username = Username;
		mcpeAddPlayer.entityIdSelf = EntityId;
		mcpeAddPlayer.runtimeEntityId = EntityId;
		mcpeAddPlayer.x = KnownPosition.X;
		mcpeAddPlayer.y = KnownPosition.Y;
		mcpeAddPlayer.z = KnownPosition.Z;
		mcpeAddPlayer.speedX = Velocity.X;
		mcpeAddPlayer.speedY = Velocity.Y;
		mcpeAddPlayer.speedZ = Velocity.Z;
		mcpeAddPlayer.yaw = KnownPosition.Yaw;
		mcpeAddPlayer.headYaw = KnownPosition.HeadYaw;
		mcpeAddPlayer.pitch = KnownPosition.Pitch;
		mcpeAddPlayer.metadata = GetMetadata();
		mcpeAddPlayer.deviceId = PlayerInfo.DeviceId;
		mcpeAddPlayer.deviceOs = PlayerInfo.DeviceOS;
		mcpeAddPlayer.gameType = (uint) GameMode;
		mcpeAddPlayer.layers = GetAbilities();

		Level.RelayBroadcast(this, players, mcpeAddPlayer);

		if (IsRiding)
		{
			McpeSetActorLink link = McpeSetActorLink.CreateObject();
			link.linkType = (byte) McpeSetActorLink.LinkActions.Ride;
			link.riderId = EntityId;
			link.riddenId = Vehicle;
			Level.RelayBroadcast(players, link);
		}

		SendEquipmentForPlayer(players);
		Inventory.ArmorInventory.SendMobArmorEquipmentPacket(players);
	}

	public void SendEquipmentForPlayer(Player[] receivers = null)
	{
		McpeMobEquipment mcpePlayerEquipment = McpeMobEquipment.CreateObject();
		mcpePlayerEquipment.runtimeEntityId = EntityId;
		mcpePlayerEquipment.item = Inventory.GetItemInHand();
		mcpePlayerEquipment.slot = 0;
		if (receivers == null)
			Level.RelayBroadcast(this, mcpePlayerEquipment);
		else
			Level.RelayBroadcast(this, receivers, mcpePlayerEquipment);
	}

	public override void DespawnFromPlayers(Player[] players)
	{
		McpeRemoveEntity mcpeRemovePlayer = McpeRemoveEntity.CreateObject();
		mcpeRemovePlayer.entityIdSelf = EntityId;
		Level.RelayBroadcast(this, players, mcpeRemovePlayer);
	}

	public void CorrectPlayerMovement() //probably useful to prevent movement hacks. Todo check after release
	{
		McpeCorrectPlayerMovement packet = McpeCorrectPlayerMovement.CreateObject();
		packet.Type = (byte) (Vehicle == 0 ? 0 : 3);
		packet.Postition = KnownPosition;
		packet.Velocity = Velocity;
		packet.OnGround = !IsGliding && IsOnGround;
		packet.Tick = CurrentTick;
		SendPacket(packet);
	}
	
	// Unhandled packets
	public void HandleMcpeClientCacheStatus(McpeClientCacheStatus message) => Log.Warn($"Cache status: {(message.enabled ? "Enabled" : "Disabled")}");
	public void HandleMcpePacketViolationWarning(McpePacketViolationWarning message) => Log.Error($"Client reported a level {message.severity} packet violation of type {message.violationType} for packet 0x{message.packetId:X2}: {message.reason}");
	public void HandleMcpeCraftingEvent(McpeCraftingEvent message) => Log.Debug($"Player {Username} crafted item on window 0x{message.windowId:X2} on type: {message.recipeType}");
	public void HandleMcpePlayerInput(McpePlayerInput message) => Log.Debug($"Player input: x={message.motionX}, z={message.motionZ}, jumping={message.jumping}, sneaking={message.sneaking}"); 
	public void HandleMcpeSetActorMotion(McpeSetActorMotion message) { }
	public void HandleMcpeLogin(McpeLogin message) { }
	public void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message) { }
	public void HandleMcpeRequestNetworkSettings(McpeRequestNetworkSettings message) { }
	public void HandleMcpeScriptCustomEvent(McpeScriptCustomEvent message) { }
	public void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message) { }
	public void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message) {}
	public void HandleMcpeLabTable(McpeLabTable message) {}
	public void HandleMcpeNetworkSettings(McpeNetworkSettings message) { }
	public void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message) { }
	public void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message) { }
	public void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message) { }
	public void HandleMcpeRequestAbility(McpeRequestAbility message) {}
	public void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message) { }
	public void HandleMcpeInventorySlot(McpeInventorySlot message) { }
	public void HandleMcpePlayerHotbar(McpePlayerHotbar message) { }
	public void HandleMcpeInventoryContent(McpeInventoryContent message) { }
	public void HandleMcpeEmoteList(McpeEmoteList message) { }
	public void HandleMcpePhotoTransfer(McpePhotoTransfer message) { }
	public void HandleMcpePurchaseReceipt(McpePurchaseReceipt message) { }
	public void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message) { }
	
	// Events

	public event EventHandler<PlayerEventArgs> PlayerJoining;

	private void OnPlayerJoining(PlayerEventArgs e)
	{
		PlayerJoining?.Invoke(this, e);
	}

	public event EventHandler<PlayerEventArgs> PlayerJoin;

	private void OnPlayerJoin(PlayerEventArgs e)
	{
		PlayerJoin?.Invoke(this, e);
	}

	public event EventHandler<PlayerEventArgs> LocalPlayerIsInitialized;

	private void OnLocalPlayerIsInitialized(PlayerEventArgs e)
	{
		LocalPlayerIsInitialized?.Invoke(this, e);
	}

	public event EventHandler<PlayerEventArgs> PlayerLeave;

	private void OnPlayerLeave(PlayerEventArgs e)
	{
		PlayerLeave?.Invoke(this, e);
	}

	public event EventHandler<PlayerEventArgs> Ticking;

	private void OnTicking(PlayerEventArgs e)
	{
		Ticking?.Invoke(this, e);
	}

	public event EventHandler<PlayerEventArgs> Ticked;

	private void OnTicked(PlayerEventArgs e)
	{
		Ticked?.Invoke(this, e);
	}

	public event EventHandler<PlayerDamageToPlayerEventArgs> PlayerDamageToPlayer;

	public bool OnPlayerDamageToPlayer(PlayerDamageToPlayerEventArgs e)
	{
		PlayerDamageToPlayer?.Invoke(this, e);
		return !e.Cancel;
	}

	public event EventHandler<ItemDropEventArgs> ItemDrop;

	private bool OnItemDrop(ItemDropEventArgs e)
	{
		ItemDrop?.Invoke(this, e);
		return !e.Cancel;
	}

	public event EventHandler<ItemTransactionEventArgs> ItemTransaction;

	public bool OnItemTransaction(ItemTransactionEventArgs e)
	{
		ItemTransaction?.Invoke(this, e);
		return !e.Cancel;
	}

	public void HandleMcpeNetworkStackLatency(McpeNetworkStackLatency message)
	{
		McpeNetworkStackLatency packet = McpeNetworkStackLatency.CreateObject();
		packet.timestamp = message.timestamp;
		packet.unknownFlag = 1;
		SendPacket(packet);
	}

	public void HandleMcpeSetInventoryOptions(McpeSetInventoryOptions message)
	{
		Log.Debug($"InventoryOptions: leftTab={message.leftTab}, rightTab={message.rightTab}, filtering={message.filtering}, inventoryLayout={message.inventoryLayout}, craftingLayout={message.craftingLayout}");
	}

	public void HandleMcpeAnvilDamage(McpeAnvilDamage message)
	{
		//TODO handle this.
		Log.Debug($"Damaged anvil at {message.coordinates.X} {message.coordinates.Y} {message.coordinates.Z} Amount = {message.damageAmount}");
	}

	public void HandleMcpeServerboundLoadingScreen(McpeServerboundLoadingScreen message)
	{
		Log.Debug($"Loading screen: {(message.ScreenType == 1 ? "Opened" : message.ScreenType == 2 ? "Closed" : "Unknown")} {message.ScreenId}");
	}

	public event EventHandler<PlayerShootEventArgs> PlayerShoot;

	public bool OnPlayerShoot(Player shooter, Item itemBase)
	{
		if (PlayerShoot == null)
			return false;

		var args = new PlayerShootEventArgs(shooter, shooter.Level, itemBase);
		PlayerShoot.Invoke(this, args);
		return args.Cancel;
	}
}

public class PlayerEventArgs(Player player) : EventArgs
{
	public Player Player { get; } = player;
	public Level Level { get; } = player?.Level;
}

public class PlayerDamageToPlayerEventArgs : LevelCancelEventArgs
{
	public Player Damager { get; }

	public PlayerDamageToPlayerEventArgs(Player player, Player damager) : base(player, player?.Level)
	{
		Player = player;
		Damager = damager;
		Level = player?.Level;
	}
}

public class PlayerShootEventArgs(Player shooter, Level level, Item itemBase) : LevelCancelEventArgs(shooter, level)
{
	public Player Shooter { get; } = shooter;
	public Item ItemBase { get; } = itemBase;
}

public class PlayerDamageToEntityEventArgs : LevelCancelEventArgs
{
	public Entity Entity { get; }
	public Player Damager { get; }

	public PlayerDamageToEntityEventArgs(Entity entity, Player damager) : base(damager, damager?.Level)
	{
		Entity = entity;
		Damager = damager;
		Level = entity?.Level;
	}
}

public class ItemDropEventArgs(Player player, Level level, Item item) : LevelCancelEventArgs(player, level)
{
	public Item Item { get; } = item;
}

public class ItemTransactionEventArgs(Player player, Level level, Item item, ItemStackAction action)
	: LevelCancelEventArgs(player, level)
{
	public Item Item { get; } = item;
	public ItemStackAction Action { get; } = action;
}