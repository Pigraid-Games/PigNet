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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Net.EnumerationsTable;
using MiNET.Net.Packets.Mcpe;
using MiNET.Net.RakNet;
using MiNET.Plugins;
using MiNET.Sounds;
using MiNET.Utils;
using MiNET.Utils.Diagnostics;
using MiNET.Utils.IO;
using MiNET.Utils.Nbt;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
using static MiNET.WeatherManager;

namespace MiNET.Worlds;

public class Level : IBlockAccess
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Level));

	public static readonly BlockCoordinates Up = new(0, 1, 0);
	public static readonly BlockCoordinates Down = new(0, -1, 0);
	public static readonly BlockCoordinates South = new(0, 0, 1);
	public static readonly BlockCoordinates North = new(0, 0, -1);
	public static readonly BlockCoordinates East = new(1, 0, 0);
	public static readonly BlockCoordinates West = new(-1, 0, 0);

	private readonly object _playerWriteLock = new();

	private readonly int _worldDayCycleTime = 24000;
	private DateTime _lastBroadcast = DateTime.UtcNow;

	private DateTime _lastSendTime = DateTime.UtcNow;

	public Profiler _profiler = new();

	private HighPrecisionTimer _tickerHighPrecisionTimer;

	private object _tickSync = new();
	private Stopwatch _tickTimer = new();
	public long AvarageTickProcessingTime = 50;
	public long LastTickProcessingTime;

	public Level(LevelManager levelManager, string levelId, IWorldProvider worldProvider, EntityManager entityManager, GameMode gameMode = GameMode.Survival, Difficulty difficulty = Difficulty.Normal, int viewDistance = 11)
	{
		Random = new Random();

		LevelManager = levelManager;
		EntityManager = entityManager;
		InventoryManager = new InventoryManager(this);
		EntitySpawnManager = new EntitySpawnManager(this);
		WeatherManager = new WeatherManager(this);
		LevelId = levelId;
		GameMode = gameMode;
		Difficulty = difficulty;
		ViewDistance = viewDistance;
		TickDistance = ViewDistance / 2;
		WorldProvider = worldProvider;
	}

	public IWorldProvider WorldProvider { get; set; }

	public PlayerLocation SpawnPoint { get; set; }

	public ConcurrentDictionary<long, Player> Players { get; private set; } = new();

//TODO: Need to protect this, not threadsafe
	public ConcurrentDictionary<long, Entity> Entities { get; private set; } = new();

//TODO: Need to protect this, not threadsafe
	public List<BlockEntity> BlockEntities { get; private set; } = new();

//TODO: Need to protect this, not threadsafe
	public ConcurrentDictionary<BlockCoordinates, long> BlockWithTicks { get; private set; } = new();

//TODO: Need to protect this, not threadsafe
	public string LevelId { get; }

	public string LevelName { get; private set; }
	public Dimension Dimension { get; set; } = Dimension.Overworld;

	public GameMode GameMode { get; }
	public bool IsSurvival => GameMode == GameMode.Survival;
	public bool HaveDownfall { get; set; }
	public Difficulty Difficulty { get; set; }
	public bool AutoSmelt { get; set; } = false;
	public long WorldTime { get; set; }
	public long CurrentWorldCycleTime { get; private set; }
	public long TickTime { get; set; }
	public int SkylightSubtracted { get; set; }
	public long StartTimeInTicks { get; private set; }
	public bool EnableBlockTicking { get; set; }
	public bool EnableChunkTicking { get; set; }

	public bool AllowBuild { get; set; } = true;
	public bool AllowBreak { get; set; } = true;

	public EntityManager EntityManager { get; protected set; }
	public InventoryManager InventoryManager { get; protected set; }
	public EntitySpawnManager EntitySpawnManager { get; protected set; }
	public WeatherManager WeatherManager { get; protected set; }

	public int ViewDistance { get; set; }

	public int TickDistance { get; set; }

	public Random Random { get; }

	public int SaveInterval { get; set; } = 300;
	public int UnloadInterval { get; set; } = -1;

	public string fog { get; set; } = "";
	public weatherTypes Weather { get; set; } = weatherTypes.clear;
	public float rainLevel { get; set; }

	public LevelManager LevelManager { get; }
	public Level NetherLevel { get; set; }
	public Level TheEndLevel { get; set; }
	public Level OverworldLevel { get; set; }
	public int PlayerCount { get; private set; }

	public bool DrowningDamage { get; set; } = true;
	public bool CommandblockOutput { get; set; } = true;
	public bool DoTiledrops { get; set; } = true;
	public bool DoMobloot { get; set; } = true;
	public bool KeepInventory { get; set; } = true;
	public bool DoDaylightcycle { get; set; } = true;
	public bool DoMobspawning { get; set; } = true;
	public bool DoEntitydrops { get; set; } = true;
	public bool DoFiretick { get; set; } = true;
	public bool DoWeathercycle { get; set; } = true;
	public bool Pvp { get; set; } = true;
	public bool Falldamage { get; set; } = true;
	public bool Firedamage { get; set; } = true;
	public bool Mobgriefing { get; set; } = true;
	public bool ShowCoordinates { get; set; } = true;
	public bool NaturalRegeneration { get; set; } = true;
	public bool TntExplodes { get; set; } = true;
	public bool SendCommandfeedback { get; set; } = true;
	public int RandomTickSpeed { get; set; } = 3;
	public bool RedstoneEnabled { get; set; } = true;
	public bool DoShowDeathMessage { get; set; } = true;

	public Block GetBlock(BlockCoordinates coordinates, ChunkColumn tryChunk = null)
	{
		ChunkColumn chunk;

		var chunkCoordinates = new ChunkCoordinates(coordinates.X >> 4, coordinates.Z >> 4);
		if (tryChunk != null && tryChunk.X == chunkCoordinates.X && tryChunk.Z == chunkCoordinates.Z)
			chunk = tryChunk;
		else
			chunk = GetChunk(chunkCoordinates);
		if (chunk == null)
			return new Air
			{
				Coordinates = coordinates,
				SkyLight = 15
			};

		Block block = chunk.GetBlockObject(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f);
		byte blockLight = chunk.GetBlocklight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f);
		byte skyLight = chunk.GetSkylight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f);
		byte biomeId = chunk.GetBiome(coordinates.X & 0x0f, coordinates.Z & 0x0f);

		block.Coordinates = coordinates;
		block.BlockLight = blockLight;
		block.SkyLight = skyLight;
		block.BiomeId = biomeId;

		return block;
	}

	public int GetHeight(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);
		if (chunk == null) return 256;

		return chunk.GetHeight(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f);
	}

	public ChunkColumn GetChunk(BlockCoordinates blockCoordinates, bool cacheOnly = false)
	{
		return GetChunk((ChunkCoordinates) blockCoordinates, cacheOnly);
	}

	public ChunkColumn GetChunk(ChunkCoordinates chunkCoordinates, bool cacheOnly = false)
	{
		ChunkColumn chunk = WorldProvider.GenerateChunkColumn(chunkCoordinates, cacheOnly);
		if (chunk == null) Log.Debug($"Got <null> chunk at {chunkCoordinates}");
		return chunk;
	}

	public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true, ChunkColumn possibleChunk = null)
	{
		if (block.Coordinates.Y < 0) return;

		var chunkCoordinates = new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4);
		ChunkColumn chunk = possibleChunk != null && possibleChunk.X == chunkCoordinates.X && possibleChunk.Z == chunkCoordinates.Z ? possibleChunk : GetChunk(chunkCoordinates);


		if (!OnBlockPlace(new BlockPlaceEventArgs(null, this, block, null))) return;

		chunk.SetBlock(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, block);
		if (calculateLight && chunk.GetHeight(block.Coordinates.X & 0x0f, block.Coordinates.Z & 0x0f) <= block.Coordinates.Y + 1) chunk.RecalcHeight(block.Coordinates.X & 0x0f, block.Coordinates.Z & 0x0f, Math.Min(ChunkColumn.WorldHeight, block.Coordinates.Y + 1));

		if (applyPhysics) ApplyPhysics(block.Coordinates.X, block.Coordinates.Y, block.Coordinates.Z);

		// We should not ignore creative. Need to investigate.
		if (GameMode != GameMode.Creative && calculateLight /* && block.LightLevel > 0*/)
		{
			if (Dimension == Dimension.Overworld) new SkyLightCalculations().Calculate(this, block.Coordinates);

			block.BlockLight = (byte) block.LightLevel;
			chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, (byte) block.LightLevel);
			BlockLightCalculations.Calculate(this, block.Coordinates);
		}

		if (broadcast)
		{
			McpeUpdateBlock message = McpeUpdateBlock.CreateObject();
			message.blockRuntimeId = (uint) block.GetRuntimeId();
			message.coordinates = block.Coordinates;
			message.blockPriority = 0xb;
			RelayBroadcast(message);
		}

		block.BlockAdded(this);
	}

	public void SetSkyLight(BlockCoordinates coordinates, byte skyLight)
	{
		ChunkColumn chunk = GetChunk(coordinates);
		chunk?.SetSkyLight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f, skyLight);
	}

	public void Initialize()
	{
		//IsWorldTimeStarted = false;
		WorldProvider.Initialize();

		SpawnPoint ??= new PlayerLocation(WorldProvider.GetSpawnPoint());
		TickTime = WorldProvider.GetTime();
		WorldTime = WorldProvider.GetDayTime();
		LevelName = WorldProvider.GetName();

		if (WorldProvider.IsCaching)
		{
			var chunkLoading = Stopwatch.StartNew();

			// Pre-cache chunks for spawn coordinates
			int i = 0;
			if (Dimension == Dimension.Overworld)
			{
				ChunkCoordinates chunkCoordinates = new ChunkCoordinates(SpawnPoint) / 8;
				foreach (McpeWrapper chunk in GenerateChunks(chunkCoordinates, new Dictionary<ChunkCoordinates, McpeWrapper>(), 1))
					if (chunk != null)
						i++;

				Log.Info($"World pre-cache {i} chunks completed in {chunkLoading.ElapsedMilliseconds}ms");
			}
		}

		if (Dimension == Dimension.Overworld)
			if (Config.GetProperty("CheckForSafeSpawn", true))
			{
				SpawnPoint.Y = GetHeight((BlockCoordinates) SpawnPoint) + 1;
				Log.Debug("Checking for safe spawn");
			}

		StartTimeInTicks = DateTime.UtcNow.Ticks;

		_tickTimer = new Stopwatch();
		_tickTimer.Restart();
		_tickerHighPrecisionTimer = new HighPrecisionTimer(50, WorldTick, false, false);
	}

	public virtual void Close()
	{
		WorldProvider?.SaveChunks();

		_tickerHighPrecisionTimer?.Dispose();
		_tickerHighPrecisionTimer = null;

		foreach (Entity entity in Entities.Values.ToArray()) entity.DespawnEntity();

		Entities.Clear();

		foreach (Player player in Players.Values.ToArray()) player.Disconnect("Unexpected player lingering on close of level: " + player.Username);

		Players.Clear();

		BlockEntities.Clear();

		BlockWithTicks.Clear();
		BlockWithTicks = null;
		BlockEntities = null;
		Players = null;
		Entities = null;

		if (WorldProvider is AnvilWorldProvider provider)
			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> chunk in provider._chunkCache)
			{
				provider._chunkCache.TryRemove(chunk.Key, out ChunkColumn waste);
				if (waste == null) continue;

				foreach (SubChunk c in waste) c.PutPool();

				waste.ClearCache();
			}

		WorldProvider = null;

		Log.Info("Closed level: " + LevelId);
	}

	internal static McpeWrapper CreateMcpeBatch(byte[] bytes)
	{
		return BatchUtils.CreateBatchPacket(new Memory<byte>(bytes, 0, bytes.Length), CompressionLevel.Optimal, true);
	}

	public virtual void AddPlayer(Player newPlayer, bool spawn)
	{
		if (newPlayer.Username == null) throw new ArgumentNullException(nameof(newPlayer.Username));

		EntityManager.AddEntity(newPlayer);

		lock (_playerWriteLock)
		{
			if (!newPlayer.IsConnected)
			{
				Log.Error("Tried to add player that was already disconnected.");
				return;
			}

			if (Players.TryAdd(newPlayer.EntityId, newPlayer))
			{
				foreach (Entity entity in Entities.Values.ToArray()) entity.SpawnToPlayers([newPlayer]);
				SpawnToAll(newPlayer);
			}

			newPlayer.IsSpawned = spawn;
		}

		if (fog != "")
		{
			McpePlayerFog msg = McpePlayerFog.CreateObject();
			msg.fogstack = new fogStack(fog);
			newPlayer.SendPacket(msg);
		}

		OnPlayerAdded(new LevelEventArgs(newPlayer, this));
	}

	public event EventHandler<LevelEventArgs> PlayerAdded;

	protected virtual void OnPlayerAdded(LevelEventArgs e)
	{
		PlayerAdded?.Invoke(this, e);
	}

	public event EventHandler<LevelEventArgs> PlayerRemoved;

	protected virtual void OnPlayerRemoved(LevelEventArgs e)
	{
		PlayerRemoved?.Invoke(this, e);
	}

	public void SpawnToAll(Player newPlayer)
	{
		lock (_playerWriteLock)
		{
			// The player list keeps us from moving this completely to player.
			// It's simply to slow and bad.

			Player[] players = GetAllPlayers();
			var spawnedPlayers = players.ToList();
			spawnedPlayers.Add(newPlayer);

			newPlayer.SpawnToPlayers(players);

			foreach (Player spawnedPlayer in players) spawnedPlayer.SpawnToPlayers([newPlayer]);

			McpePlayerList playerListMessage = McpePlayerList.CreateObject();
			playerListMessage.records = new PlayerAddRecords(spawnedPlayers);
			newPlayer.SendPacket(playerListMessage);

			McpePlayerList playerList = McpePlayerList.CreateObject();
			playerList.records = new PlayerAddRecords { newPlayer };
			RelayBroadcast(playerList);
		}
	}

	public virtual void RemovePlayer(Player player, bool despawn = true)
	{
		if (Players == null) return; // Closing down the level sets players to null;
		if (Entities == null) return; // Closing down the level sets players to null;

		lock (_playerWriteLock)
		{
			Player removed;
			if (Players.TryRemove(player.EntityId, out removed))
			{
				player.IsSpawned = false;
				if (despawn) DespawnFromAll(player);

				foreach (Entity entity in Entities.Values.ToArray()) entity.DespawnFromPlayers(new[] { removed });
			}
		}

		OnPlayerRemoved(new LevelEventArgs(player, this));
	}

	public void DespawnFromAll(Player player)
	{
		lock (_playerWriteLock)
		{
			Player[] spawnedPlayers = GetAllPlayers();

			foreach (Player spawnedPlayer in spawnedPlayers) spawnedPlayer.DespawnFromPlayers(new[] { player });

			player.DespawnFromPlayers(spawnedPlayers);

			McpePlayerList playerListMessage = McpePlayerList.CreateObject();
			playerListMessage.records = new PlayerRemoveRecords(spawnedPlayers);
			player.SendPacket(CreateMcpeBatch(playerListMessage.Encode()));
			playerListMessage.records = null;
			playerListMessage.PutPool();

			McpePlayerList playerList = McpePlayerList.CreateObject();
			playerList.records = new PlayerRemoveRecords { player };
			RelayBroadcast(player, CreateMcpeBatch(playerList.Encode()));
			playerList.records = null;
			playerList.PutPool();
		}
	}

	public void AddEntity(Entity entity)
	{
		lock (Entities)
		{
			EntityManager.AddEntity(entity);

			if (Entities.TryAdd(entity.EntityId, entity))
				entity.SpawnToPlayers(GetAllPlayers());
			else
				throw new Exception("Entity existed in the players list when it should not");
		}
	}

	public void RemoveEntity(Entity entity)
	{
		lock (Entities)
		{
			if (!Entities.TryRemove(entity.EntityId, out entity)) return; // It's ok. Holograms destroy this play..
			entity.DespawnFromPlayers(GetAllPlayers());
		}
	}


	public void RemoveDuplicatePlayers(string username, long clientId)
	{
	}

	public virtual void BroadcastTitle(string text, TitleType type = TitleType.Title, int fadeIn = 6, int fadeOut = 6, int stayTime = 20, Player sender = null, Player[] sendList = null)
	{
		McpeSetTitle mcpeSetTitle = McpeSetTitle.CreateObject();
		mcpeSetTitle.fadeInTime = fadeIn;
		mcpeSetTitle.stayTime = stayTime;
		mcpeSetTitle.fadeOutTime = fadeOut;
		mcpeSetTitle.type = (int) type;
		mcpeSetTitle.text = text;

		RelayBroadcast(sender, sendList, mcpeSetTitle);
	}

	public virtual void BroadcastMessage(string text, TextPacketType type = TextPacketType.Chat, Player sender = null, Player[] sendList = null, bool needsTranslation = false, string[] parameters = null, string platformId = null)
	{
		if (type is TextPacketType.Chat or TextPacketType.Raw)
			foreach (string line in text.Split(["\n", Environment.NewLine], StringSplitOptions.RemoveEmptyEntries))
			{
				McpeText message = McpeText.CreateObject();
				message.type = type;
				message.source = sender == null ? "" : sender.NameTag;
				message.xuid = sender?.CertificateData.ExtraData.Xuid;
				message.platformChatId = platformId;
				message.message = line;
				message.needsTranslation = needsTranslation;
				message.parameters = parameters;
				RelayBroadcast(sendList, message);
			}
		else
		{
			McpeText message = McpeText.CreateObject();
			message.type = type;
			message.source = sender == null ? "" : sender.Username;
			message.xuid = sender?.CertificateData.ExtraData.Xuid;
			message.platformChatId = platformId;
			message.message = text;
			message.needsTranslation = needsTranslation;
			message.parameters = parameters;
			RelayBroadcast(sendList, message);
		}
	}

	private void WorldTick(object sender)
	{
		if (Log.IsDebugEnabled && _tickTimer.ElapsedMilliseconds >= 65) Log.Warn($"Time between world tick too long: {_tickTimer.ElapsedMilliseconds} ms. Last processing time={LastTickProcessingTime}, Avarage={AvarageTickProcessingTime}");

		Measurement worldTickMeasurement = _profiler.Begin("World tick");

		_tickTimer.Restart();

		try
		{
			TickTime++;

			WeatherManager.tick(TickTime);

			Player[] players = GetSpawnedPlayers();

			if (TickTime % 10 == 0)
				foreach (Player player in players)
					if (player.Level.GetBlock((int) player.KnownPosition.X, (int) player.KnownPosition.Y, (int) player.KnownPosition.Z) is PressurePlateBase)
						if (!player.Level.BlockWithTicks.ContainsKey(new BlockCoordinates((int) player.KnownPosition.X, (int) player.KnownPosition.Y, (int) player.KnownPosition.Z)))
							player.Level.ScheduleBlockTick(player.Level.GetBlock(new BlockCoordinates((int) player.KnownPosition.X, (int) player.KnownPosition.Y, (int) player.KnownPosition.Z)), 20);

			if (DoDaylightcycle) WorldTime++;

			CurrentWorldCycleTime = WorldTime % _worldDayCycleTime;

			if (DoDaylightcycle && TickTime % 100 == 0)
			{
				McpeSetTime message = McpeSetTime.CreateObject();
				message.time = (int) WorldTime;
				RelayBroadcast(message);
			}

			SkylightSubtracted = CalculateSkylightSubtracted(WorldTime);

			// Save dirty chunks
			if (TickTime % (SaveInterval * 20) == 0) WorldProvider.SaveChunks();
			if (UnloadInterval > 0 && TickTime % (UnloadInterval * 20) == 0)
			{
				var cacheProvider = WorldProvider as ICachingWorldProvider;
				int removed = 0;
				if (players.Length > 0)
					foreach (Player player in players)
					{
						var oldChunks = (ChunkCoordinates) (BlockCoordinates) player.KnownPosition;
						removed += cacheProvider?.UnloadChunks(players, oldChunks, ViewDistance) ?? 0;
					}
				else
					removed += cacheProvider?.UnloadChunks(players, (ChunkCoordinates) (BlockCoordinates) SpawnPoint, 1) ?? 0;
				if (removed > 0) Log.Warn($"Unloaded {removed} chunks, {cacheProvider?.GetCachedChunks().Length} chunks remain cached");
			}

			Measurement blockAndChunkTickMeasurement = worldTickMeasurement?.Begin("Block and chunk tick");

			Entity[] entities = Entities.Values.OrderBy(e => e.EntityId).ToArray();
			if (EnableChunkTicking || EnableBlockTicking)
			{
				if (DoMobspawning) EntitySpawnManager.DespawnMobs(TickTime);

				var chunksWithinRadiusOfPlayer = new List<EntitySpawnManager.SpawnState>();
				foreach (Player player in players)
				{
					var bCoord = (BlockCoordinates) player.KnownPosition;

					chunksWithinRadiusOfPlayer = GetChunkCoordinatesForTick(new ChunkCoordinates(bCoord), chunksWithinRadiusOfPlayer, TickDistance, Random);
				}

				if (chunksWithinRadiusOfPlayer.Count > 0)
				{
					bool canSpawnPassive = false;
					bool canSpawnHostile = false;

					if (DoMobspawning)
					{
						canSpawnPassive = TickTime % 400 == 0;

						int effectiveChunkCount = Math.Max(17 * 17, chunksWithinRadiusOfPlayer.Count);
						int entityPassiveCount = 0;
						int entityHostileCount = 0;
						foreach (Entity entity in entities)
							if (entity is PassiveMob)
								entityPassiveCount++;
							else if (entity is HostileMob) entityHostileCount++;


						float passiveCap = EntitySpawnManager.CapPassive * (effectiveChunkCount / 289f);
						canSpawnPassive = canSpawnPassive && entityPassiveCount < passiveCap;
						canSpawnPassive = canSpawnPassive || entityPassiveCount < passiveCap * 0.20; // Custom to get instant spawn when no mobs
						canSpawnHostile = entityHostileCount < EntitySpawnManager.CapHostile * (effectiveChunkCount / 289f);
					}

					List<EntitySpawnManager.SpawnState> state = chunksWithinRadiusOfPlayer;

					Parallel.ForEach(state, spawnState =>
					{
						var random = new Random(spawnState.Seed);

						ChunkColumn chunk = GetChunk(new ChunkCoordinates(spawnState.ChunkX, spawnState.ChunkZ), true);
						if (chunk == null) return; // Not loaded

						if (DoMobspawning)
						{
							int x = random.Next(16);
							int z = random.Next(16);

							short height = chunk.GetHeight(x, z);

							Measurement chunkTickMeasurement = blockAndChunkTickMeasurement?.Begin("Chunk tick");

							int maxValue = ((((height + 1) >> 4) + 1) * 16) - 1;
							int ySpawn = random.Next(maxValue);
							var spawnCoordinates = new BlockCoordinates(x + (spawnState.ChunkX * 16), ySpawn, z + (spawnState.ChunkZ * 16));
							Block spawnBlock = GetBlock(spawnCoordinates, chunk);
							if (spawnBlock.IsTransparent)
								// Entity spawning, only one attempt per chunk
								EntitySpawnManager.AttemptMobSpawn(spawnCoordinates, random, canSpawnPassive, canSpawnHostile);

							chunkTickMeasurement?.End();
						}

						if (!EnableBlockTicking || RandomTickSpeed <= 0) return;
						{
							for (int s = 0; s < 16; s++)
							for (int i = 0; i < RandomTickSpeed; i++)
							{
								int x = random.Next(16);
								int y = random.Next(16);
								int z = random.Next(16);

								Measurement blockTickMeasurement = blockAndChunkTickMeasurement?.Begin("Block tick");

								var blockCoordinates = new BlockCoordinates(x + (spawnState.ChunkX * 16), y + (s * 16), z + (spawnState.ChunkZ * 16));
								Block block = GetBlock(blockCoordinates, chunk);
								block.OnTick(this, true);
								blockTickMeasurement?.End();
							}
						}
					});
				}
			}

			blockAndChunkTickMeasurement?.End();

			Measurement blockUpdateMeasurement = worldTickMeasurement?.Begin("Block update tick");

			// Block updates
			foreach (KeyValuePair<BlockCoordinates, long> blockEvent in BlockWithTicks)
				try
				{
					if (blockEvent.Value > TickTime) continue;
					if (BlockWithTicks.TryRemove(blockEvent.Key, out _))
						GetBlock(blockEvent.Key).OnTick(this, false);
				}
				catch (Exception e)
				{
					Log.Warn("Block ticking", e);
				}

			blockUpdateMeasurement?.End();

			Measurement blockEntityMeasurement = worldTickMeasurement?.Begin("Block entity tick");
			// Block entity updates
			foreach (BlockEntity blockEntity in BlockEntities.ToArray()) blockEntity.OnTick(this);

			blockEntityMeasurement?.End();

			Measurement entityMeasurement = worldTickMeasurement?.Begin("Entity tick");

			// Entity updates
			foreach (Entity entity in entities) entity.OnTick(entities);

			entityMeasurement?.End();

			PlayerCount = players.Length;

			// Player tick
			Measurement playerMeasurement = worldTickMeasurement?.Begin("Player tick");

			foreach (Player player in players)
				if (player.IsSpawned)
					player.OnTick(entities);

			playerMeasurement?.End();

			// Send player movements
			BroadCastMovement(players, entities);

			var tasks = new List<Task>();
			foreach (Player player in players)
				if (player.NetworkHandler is RakSession session)
					tasks.Add(session.SendQueueAsync());
			Task.WhenAll(tasks).Wait();

			if (Log.IsDebugEnabled && _tickTimer.ElapsedMilliseconds >= 50) Log.Error($"World tick too too long: {_tickTimer.ElapsedMilliseconds} ms");
		}
		catch (Exception e)
		{
			Log.Error("World ticking", e);
		}
		finally
		{
			LastTickProcessingTime = _tickTimer.ElapsedMilliseconds;
			AvarageTickProcessingTime = ((AvarageTickProcessingTime * 9) + _tickTimer.ElapsedMilliseconds) / 10L;

			worldTickMeasurement?.End();
		}
	}

	public int GetSubtractedLight(BlockCoordinates coordinates)
	{
		return GetSubtractedLight(coordinates, SkylightSubtracted);
	}

	public int GetSubtractedLight(BlockCoordinates coordinates, int amount)
	{
		int skyLight = GetSkyLight(coordinates) - amount;
		byte blockLight = GetBlockLight(coordinates);

		return Math.Max(skyLight, blockLight);
	}

	public int CalculateSkylightSubtracted(long worldTime)
	{
		float f = CalculateCelestialAngle(worldTime);
		double f1 = 1.0F - ((Math.Cos(f * ((float) Math.PI * 2F)) * 2.0F) + 0.5F);
		f1 = BiomeUtils.Clamp((float) f1, 0.0F, 1.0F);
		f1 = 1.0F - f1;
		f1 = 1.0F - f1;
		return (int) (f1 * 11.0F);
	}

	public float CalculateCelestialAngle(long worldTime)
	{
		int i = (int) (worldTime % 24000L);
		float f = (i / 24000.0F) - 0.25F;

		if (f < 0.0F) ++f;

		if (f > 1.0F) --f;

		float f1 = 1.0F - (float) ((Math.Cos(f * Math.PI) + 1.0D) / 2.0D);
		f += ((f1 - f) / 3.0F);
		return f;
	}

	public Player[] GetSpawnedPlayers()
	{
		return Players == null ? [] : // HACK
			Players.Values.Where(player => player.IsSpawned).ToArray();
	}

	public Player[] GetAllPlayers()
	{
		return Players == null ? [] : // HACK
			Players.Values.ToArray();
	}

	public Entity[] GetEntities()
	{
		lock (Entities) return Entities.Values.ToArray();
	}

	private IEnumerable<Player> GetStaledPlayers(Player[] players)
	{
		DateTime now = DateTime.UtcNow;
		var span = TimeSpan.FromSeconds(300);
		return players.Where(player => now - player.LastUpdatedTime > span);
	}

	protected virtual void BroadCastMovement(Player[] players, Entity[] entities)
	{
		DateTime now = DateTime.UtcNow;

		switch (players.Length)
		{
			case 0:
			case <= 1 when entities.Length == 0:
				return;
		}

		DateTime lastSendTime = _lastSendTime;
		_lastSendTime = DateTime.UtcNow;

		int playerMoveCount = 0;

		var movePackets = new List<Packet>();

		foreach (Player player in players)
		{
			if (now - player.LastUpdatedTime <= now - lastSendTime && player.KnownPosition != player.LastSentPosition)
			{
				if (Vector3.Distance(player.KnownPosition, player.LastSentPosition) > ViewDistance)
				{
					var knownPosition = (PlayerLocation) player.KnownPosition.Clone();

					McpeMoveActor move = McpeMoveActor.CreateObject();
					move.runtimeEntityId = player.EntityId;
					move.flags = 2;
					move.position = knownPosition;
					move.position.Y += 1.62f;
					movePackets.Add(move);
				}
				else
				{
					var knownPosition = (PlayerLocation) player.KnownPosition.Clone();

					McpeMoveActorDelta move = McpeMoveActorDelta.CreateObject();
					move.runtimeEntityId = player.EntityId;
					move.prevSentPosition = player.LastSentPosition;
					move.currentPosition = knownPosition;
					move.currentPosition.Y += 1.62f;
					move.isOnGround = player.IsWalker && player.IsOnGround;
					if (move.SetFlags()) RelayBroadcast(move);
					movePackets.Add(move);
				}
				playerMoveCount++;
			}
			player.LastSentPosition = (PlayerLocation) player.KnownPosition.Clone();
		}

		if (playerMoveCount == 0) return;

		if (players.Length == 1) return;

		if (movePackets.Count == 0) return;

		McpeWrapper batch = McpeWrapper.CreateObject(players.Length);
		batch.ReliabilityHeader.Reliability = Reliability.ReliableOrdered;
		batch.payload = Compression.CompressPacketsForWrapper(movePackets, CompressionLevel.Fastest, true);
		batch.Encode();
		foreach (Player player in players) MiNetServer.FastThreadPool.QueueUserWorkItem(() => player.SendPacket(batch));
		_lastBroadcast = DateTime.UtcNow;
	}

	public void RelayBroadcast<T>(T message) where T : Packet<T>, new()
	{
		RelayBroadcast(null, GetAllPlayers(), message);
	}

	public void RelayBroadcast<T>(Player source, T message) where T : Packet<T>, new()
	{
		RelayBroadcast(source, GetAllPlayers(), message);
	}

	public void RelayBroadcast<T>(Player[] sendList, T message) where T : Packet<T>, new()
	{
		RelayBroadcast(null, sendList ?? GetAllPlayers(), message);
	}

	public void RelayBroadcast<T>(Player source, Player[] sendList, T message) where T : Packet<T>, new()
	{
		if (message == null) return;

		if (!message.IsPooled)
			try
			{
				throw new ArgumentException($"Trying to broadcast a message of type {message.GetType().Name} that isn't pooled. Please use CreateObject and not the constructor.");
			}
			catch (Exception e)
			{
				Log.Fatal("Broadcast", e);
				throw;
			}

		if (sendList == null || sendList.Length == 0)
		{
			message.PutPool();
			return;
		}

		if (message.ReferenceCounter == 1 && sendList.Length > 1) message.AddReferences(sendList.Length - 1);

		if (sendList.Length == 1)
		{
			Player player = sendList.First();

			if (source != null && player == source)
			{
				message.PutPool();
				return;
			}

			player.SendPacket(message);
		}
		else
			Parallel.ForEach(sendList, player =>
			{
				if (source != null && player == source)
				{
					message.PutPool();
					return;
				}

				player.SendPacket(message);
			});
	}

	public List<EntitySpawnManager.SpawnState> GetChunkCoordinatesForTick(ChunkCoordinates chunkPosition, List<EntitySpawnManager.SpawnState> chunksUsed, double radius, Random random)
	{
		{
			var newOrders = new List<EntitySpawnManager.SpawnState>();

			int centerX = chunkPosition.X;
			int centerZ = chunkPosition.Z;

			int halfRadius = (int) Math.Floor(radius / 2f);

			for (double x = -halfRadius; x <= halfRadius; ++x)
			for (double z = -halfRadius; z <= halfRadius; ++z)
			{
				int chunkX = (int) (x + centerX);
				int chunkZ = (int) (z + centerZ);
				var index = new EntitySpawnManager.SpawnState(chunkX, chunkZ, random.Next());
				newOrders.Add(index);
			}

			return newOrders.Union(chunksUsed).ToList();
		}
	}

	public IEnumerable<McpeWrapper> GenerateChunks(ChunkCoordinates chunkPosition, Dictionary<ChunkCoordinates, McpeWrapper> chunksUsed, double radius, Func<Vector3> getCurrentPositionAction = null)
	{
		lock (chunksUsed)
		{
			var newOrders = new Dictionary<ChunkCoordinates, double>();

			double radiusSquared = Math.Pow(radius, 2);

			int centerX = chunkPosition.X;
			int centerZ = chunkPosition.Z;

			for (double x = -radius; x <= radius; ++x)
			for (double z = -radius; z <= radius; ++z)
			{
				double distance = (x * x) + (z * z);
				if (distance > radiusSquared) continue;
				int chunkX = (int) (x + centerX);
				int chunkZ = (int) (z + centerZ);
				var index = new ChunkCoordinates(chunkX, chunkZ);
				newOrders[index] = distance;
			}

			foreach (ChunkCoordinates chunkKey in chunksUsed.Keys.ToArray())
				if (!newOrders.ContainsKey(chunkKey))
					chunksUsed.Remove(chunkKey);

			foreach (KeyValuePair<ChunkCoordinates, double> pair in newOrders.OrderBy(pair => pair.Value))
			{
				if (chunksUsed.ContainsKey(pair.Key)) continue;

				if (WorldProvider == null) continue;

				if (getCurrentPositionAction != null)
				{
					Vector3 currentPos = getCurrentPositionAction();
					var coords = new ChunkCoordinates(currentPos);
					if (coords.DistanceTo(pair.Key) > radius) continue;
				}
				ChunkColumn chunkColumn = GetChunk(pair.Key);
				McpeWrapper chunk = null;
				if (chunkColumn != null)
				{
					chunk = chunkColumn.GetBatch();
					chunksUsed.Add(pair.Key, chunk);
				}

				yield return chunk;
			}
		}
	}

	public Block GetBlock(PlayerLocation location)
	{
		return GetBlock((BlockCoordinates) location);
	}

	public Block GetBlock(int x, int y, int z)
	{
		return GetBlock(new BlockCoordinates(x, y, z));
	}


	public bool IsBlock(int x, int y, int z, int blockId)
	{
		return IsBlock(new BlockCoordinates(x, y, z), blockId);
	}

	public bool IsBlock(BlockCoordinates blockCoordinates, int blockId)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);
		if (chunk == null) return false;

		return chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f) == blockId;
	}

	public bool IsAir(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);
		if (chunk == null) return true;

		int bid = chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
		return bid == 0;
		//return bid == 0 || bid == 20 || bid == 241; // Need this for skylight calculations. Revise!
	}

	public bool IsNotBlockingSkylight(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);
		if (chunk == null) return true;

		int bid = chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
		return bid == 0 || bid == 20 || bid == 241; // Need this for skylight calculations. Revise!
	}

	public bool IsTransparent(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);
		if (chunk == null) return true;

		int bid = chunk.GetBlockId(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
		return BlockFactory.TransparentBlocks[bid] == 1;
	}

	public byte GetSkyLight(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);

		if (chunk == null) return 15;

		return chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
	}

	public byte GetBlockLight(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);

		if (chunk == null) return 15;

		return chunk.GetBlocklight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f);
	}

	public byte GetBiomeId(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(blockCoordinates);

		if (chunk == null) return 0;

		return chunk.GetBiome(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f);
	}

	private void CalculateSkyLight(int x, int y, int z)
	{
		DoLight(x, y, z);
		DoLight(x - 1, y, z);
		DoLight(x + 1, y, z);
		DoLight(x, y, z - 1);
		DoLight(x, y, z + 1);
		DoLight(x - 1, y, z - 1);
		DoLight(x - 1, y, z + 1);
		DoLight(x + 1, y, z - 1);
		DoLight(x + 1, y, z + 1);
	}

	private void DoLight(int x, int y, int z)
	{
		//Block block = GetBlock(x, y, z);
		//if (block is Air) return;
		//new SkyLightCalculations().Calculate(this, block);
	}

	public void SetBlockLight(Block block)
	{
		ChunkColumn chunk = GetChunk(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
		chunk.SetBlocklight(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, block.BlockLight);
	}

	public void SetBlockLight(BlockCoordinates coordinates, byte blockLight)
	{
		ChunkColumn chunk = GetChunk(coordinates);
		chunk?.SetBlocklight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f, blockLight);
	}

	public void SetBiomeId(BlockCoordinates coordinates, byte biomeId)
	{
		ChunkColumn chunk = GetChunk(coordinates);
		chunk?.SetBiome(coordinates.X & 0x0f, coordinates.Z & 0x0f, biomeId);
	}

	public void SetSkyLight(Block block)
	{
		ChunkColumn chunk = GetChunk(new ChunkCoordinates(block.Coordinates.X >> 4, block.Coordinates.Z >> 4));
		chunk.SetSkyLight(block.Coordinates.X & 0x0f, block.Coordinates.Y, block.Coordinates.Z & 0x0f, block.SkyLight);
	}

	public void SetAir(BlockCoordinates blockCoordinates, bool broadcast = true)
	{
		SetAir(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z, broadcast);
	}

	public void SetAir(int x, int y, int z, bool broadcast = true)
	{
		Block air = BlockFactory.GetBlockById(0);
		air.Coordinates = new BlockCoordinates(x, y, z);
		SetBlock(air, broadcast);
	}

	public BlockEntity GetBlockEntity(BlockCoordinates blockCoordinates)
	{
		BlockEntity blockEntity = BlockEntities.FirstOrDefault(entity => entity.Coordinates == blockCoordinates);
		if (blockEntity != null) return blockEntity;

		ChunkColumn chunk = GetChunk(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));

		NbtCompound nbt = chunk?.GetBlockEntity(blockCoordinates);
		if (nbt == null) return null;

		if (!nbt.TryGet("id", out NbtString idTag)) return null;

		blockEntity = BlockEntityFactory.GetBlockEntityById(idTag.StringValue);
		if (blockEntity == null) return null;

		blockEntity.Coordinates = blockCoordinates;
		blockEntity.SetCompound(nbt);

		return blockEntity;
	}

	public void SetBlockEntity(BlockEntity blockEntity, bool broadcast = true)
	{
		ChunkColumn chunk = GetChunk(new ChunkCoordinates(blockEntity.Coordinates.X >> 4, blockEntity.Coordinates.Z >> 4));
		chunk.SetBlockEntity(blockEntity.Coordinates, blockEntity.GetCompound());

		if (blockEntity.UpdatesOnTick)
		{
			BlockEntities.RemoveAll(entity => entity.Coordinates == blockEntity.Coordinates);
			BlockEntities.Add(blockEntity);
		}

		if (!broadcast) return;

		var nbt = new Nbt
		{
			NbtFile = new NbtFile
			{
				BigEndian = false,
				UseVarInt = true,
				RootTag = blockEntity.GetCompound()
			}
		};

		if (Log.IsDebugEnabled) Log.Debug($"Nbt: {nbt.NbtFile.RootTag}");

		McpeBlockActorData actorData = McpeBlockActorData.CreateObject();
		actorData.actorDataTags = nbt;
		actorData.blockPositin = blockEntity.Coordinates;

		RelayBroadcast(actorData);
	}

	public void RemoveBlockEntity(BlockCoordinates blockCoordinates)
	{
		ChunkColumn chunk = GetChunk(new ChunkCoordinates(blockCoordinates.X >> 4, blockCoordinates.Z >> 4));
		NbtCompound nbt = chunk.GetBlockEntity(blockCoordinates);
		if (nbt == null) return;

		BlockEntity blockEntity = BlockEntities.FirstOrDefault(entity => entity.Coordinates == blockCoordinates);
		if (blockEntity != null)
			BlockEntities.Remove(blockEntity);

		//Log.Error("NULL");
		chunk.RemoveBlockEntity(blockCoordinates);
	}

	public event EventHandler<BlockPlaceEventArgs> BlockPlace;

	public virtual bool OnBlockPlace(BlockPlaceEventArgs e)
	{
		BlockPlace?.Invoke(this, e);

		return !e.Cancel;
	}

	public void Interact(Player player, Item itemInHand, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block target = GetBlock(blockCoordinates);
		if (!player.IsSneaking && OnBlockInteract(new BlockInteractEventArgs(player, this, target)) && target.Interact(this, player, blockCoordinates, face, faceCoords)) return; // Handled in block interaction

		Log.Debug($"Item in hand: {itemInHand}");
		if (itemInHand.Id == 356) itemInHand = new ItemBlock(new UnpoweredRepeater()); //TODO: item translator
		if (itemInHand.Id == 379) itemInHand = new ItemBlock(new BrewingStand()); //TODO: item translator
		if (itemInHand.Id == 404) itemInHand = new ItemBlock(new UnpoweredComparator()); //TODO: item translator
		if (itemInHand.Id == 410) itemInHand = new ItemBlock(new Hopper()); //TODO: item translator
		if (itemInHand.Id == 720) itemInHand = new ItemBlock(new Campfire()); //TODO: item translator
		if (itemInHand.Id == 331) itemInHand = new ItemBlock(new RedstoneWire()); //TODO: item translator

		Block block = GetBlock(blockCoordinates);
		if (!block.IsReplaceable) block = GetBlock(itemInHand.GetNewCoordinatesFromFace(blockCoordinates, face));

		if (!AllowBuild || player.GameMode == GameMode.Spectator || !OnBlockPlace(new BlockPlaceEventArgs(player, this, target, block)))
		{
			if (!itemInHand.CanInteract)
				return;
			// Revert

			player.SendPlayerInventory();

			McpeUpdateBlock message = McpeUpdateBlock.CreateObject();
			message.blockRuntimeId = (uint) block.GetRuntimeId();
			message.coordinates = block.Coordinates;
			message.blockPriority = 0xb;
			player.SendPacket(message);

			return;
		}

		itemInHand.PlaceBlock(this, player, blockCoordinates, face, faceCoords);
	}

	public event EventHandler<BlockInteractEventArgs> BlockInteract;

	protected virtual bool OnBlockInteract(BlockInteractEventArgs e)
	{
		BlockInteract?.Invoke(this, e);

		return !e.Cancel;
	}

	public event EventHandler<BlockBreakEventArgs> BlockBreak;

	protected virtual bool OnBlockBreak(BlockBreakEventArgs e)
	{
		BlockBreak?.Invoke(this, e);

		return !e.Cancel;
	}

	public bool BreakBlock(Player player, BlockCoordinates blockCoordinates, BlockFace face = BlockFace.None)
	{
		Block block = GetBlock(blockCoordinates);
		BlockEntity blockEntity = GetBlockEntity(blockCoordinates);

		Item inHand = player.Inventory.GetItemInHand() ?? new ItemAir();

		bool canBreak = inHand.BreakBlock(this, player, block, blockEntity);

		int xpDrop = player.GameMode == GameMode.Survival ? (int) block.GetExperiencePoints() : 0;

		var eventArgs = new BlockBreakEventArgs(player, this, block, [.. block.GetDrops(inHand)]);

		if (!canBreak || !AllowBreak || player.GameMode == GameMode.Spectator || !OnBlockBreak(eventArgs))
		{
			RevertBlockAction(player, block, blockEntity);
			return false;
		}

		BreakBlock(player, block, blockEntity, inHand, face);

		player.Inventory.DamageItemInHand(ItemDamageReason.BlockBreak, null, block);

		player.HungerManager.IncreaseExhaustion(0.005f);

		if (xpDrop > 0) DropExperience(blockCoordinates, xpDrop);
		Log.Warn("The BreakBlock completed as - can be broken");
		return true;
	}

	private void DropExperience(BlockCoordinates coordinates, int amount)
	{
		var random = new Random();
		for (int i = 0; i < amount; i++)
		{
			var xpOrb = new ExperienceOrb(this)
			{
				KnownPosition = new PlayerLocation(
					coordinates.X + 0.5f + (float)(random.NextDouble() * 0.2 - 0.1),
					coordinates.Y + 0.5f + (float)(random.NextDouble() * 0.2 - 0.1),
					coordinates.Z + 0.5f + (float)(random.NextDouble() * 0.2 - 0.1)
				),
			};
			xpOrb.SpawnEntity();
		}
	}

	private void RevertBlockAction(Player player, Block block, BlockEntity blockEntity)
	{
		Log.Warn("The RevertBlockAction has been called");
		BlockCoordinates revertCoords = block.Coordinates;

		if (block is DoorBase doors) revertCoords = doors.UpperBlockBit ? revertCoords.BlockDown() : revertCoords.BlockUp();

		McpeUpdateBlock message = McpeUpdateBlock.CreateObject();
		message.blockRuntimeId = (uint) block.GetRuntimeId();
		message.coordinates = revertCoords;
		message.blockPriority = 0xb;
		player.SendPacket(message);

		if (blockEntity != null)
		{
			var nbt = new Nbt
			{
				NbtFile = new NbtFile
				{
					BigEndian = false,
					RootTag = blockEntity.GetCompound()
				}
			};

			McpeBlockActorData actorData = McpeBlockActorData.CreateObject();
			actorData.actorDataTags = nbt;
			actorData.blockPositin = blockEntity.Coordinates;
			player.SendPacket(actorData);
		}
	}

	public void BreakBlock(Player player, Block block, BlockEntity blockEntity = null, Item tool = null, BlockFace face = BlockFace.None)
	{
		block.BreakBlock(this, face);
		var drops = new List<Item>();
		drops.AddRange(block.GetDrops(tool ?? new ItemAir()));

		if (blockEntity != null)
		{
			RemoveBlockEntity(block.Coordinates);
			drops.AddRange(blockEntity.GetDrops());
		}

		if ((player != null && player.GameMode == GameMode.Survival && DoTiledrops) || (player == null && GameMode == GameMode.Survival && DoTiledrops))
			foreach (Item drop in drops)
				DropItem(block.Coordinates, drop);
	}


	public virtual void DropItem(Vector3 coordinates, Item drop)
	{
		if (GameMode == GameMode.Creative) return;

		if (drop == null) return;
		if (drop.Id == 0) return;
		if (drop.Count == 0) return;

		if (AutoSmelt) drop = drop.GetSmelt() ?? drop;

		var random = new Random();
		var itemEntity = new ItemActor(this, drop)
		{
			KnownPosition =
			{
				X = coordinates.X + 0.5f,
				Y = coordinates.Y + 0.5f,
				Z = coordinates.Z + 0.5f
			},
			Velocity = new Vector3((float) (random.NextDouble() * 0.005), (float) (random.NextDouble() * 0.20), (float) (random.NextDouble() * 0.005))
		};

		itemEntity.SpawnEntity();
	}

	public void ApplyPhysics(int x, int y, int z)
	{
		DoPhysics(x - 1, y, z);
		DoPhysics(x + 1, y, z);
		DoPhysics(x, y - 1, z);
		DoPhysics(x, y + 1, z);
		DoPhysics(x, y, z - 1);
		DoPhysics(x, y, z + 1);
	}

	private void DoPhysics(int x, int y, int z)
	{
		Block block = GetBlock(x, y, z);
		if (block is Air) return;
		block.DoPhysics(this);
	}

	public void ScheduleBlockTick(Block block, int tickRate)
	{
		if (BlockWithTicks.ContainsKey(block.Coordinates)) return;
		BlockWithTicks[block.Coordinates] = TickTime + tickRate;
	}

	public void CancelBlockTick(Block block)
	{
		BlockWithTicks.TryRemove(block.Coordinates, out _);
	}

	public bool TryGetEntity<T>(long targetEntityId, out T entity) where T : class
	{
		entity = null;

		if (Players.TryGetValue(targetEntityId, out Player player))
			entity = player as T;
		else if (Entities.TryGetValue(targetEntityId, out Entity ent)) entity = ent as T;

		return entity != null;
	}


	public ChunkColumn[] GetLoadedChunks() => WorldProvider is ICachingWorldProvider cacheProvider ? cacheProvider.GetCachedChunks() : [];
	

	public void ClearLoadedChunks()
	{
		var cacheProvider = WorldProvider as ICachingWorldProvider;
		cacheProvider?.ClearCachedChunks();
	}

	public void StrikeLightning(Vector3 position)
	{
		new Lightning(this) { KnownPosition = new PlayerLocation(position) }.SpawnEntity();
	}

	public void MakeSound(Sound sound)
	{
		sound.Spawn(this);
	}

	public virtual void BroadcastGameRules()
	{
		McpeGameRulesChanged gameRulesChanged = McpeGameRulesChanged.CreateObject();
		gameRulesChanged.rules = GetGameRules();
		RelayBroadcast(gameRulesChanged);
	}

	public void SetGameRule(GameRulesEnum rule, bool value)
	{
		switch (rule)
		{
			case GameRulesEnum.DrowningDamage:
				DrowningDamage = value;
				break;
			case GameRulesEnum.CommandblockOutput:
				CommandblockOutput = value;
				break;
			case GameRulesEnum.DoTiledrops:
				DoTiledrops = value;
				break;
			case GameRulesEnum.DoMobloot:
				DoMobloot = value;
				break;
			case GameRulesEnum.KeepInventory:
				KeepInventory = value;
				break;
			case GameRulesEnum.DoDaylightcycle:
				DoDaylightcycle = value;
				break;
			case GameRulesEnum.DoMobspawning:
				DoMobspawning = value;
				break;
			case GameRulesEnum.DoEntitydrops:
				DoEntitydrops = value;
				break;
			case GameRulesEnum.DoFiretick:
				DoFiretick = value;
				break;
			case GameRulesEnum.DoWeathercycle:
				DoWeathercycle = value;
				break;
			case GameRulesEnum.Pvp:
				Pvp = value;
				break;
			case GameRulesEnum.Falldamage:
				Falldamage = value;
				break;
			case GameRulesEnum.Firedamage:
				Firedamage = value;
				break;
			case GameRulesEnum.Mobgriefing:
				Mobgriefing = value;
				break;
			case GameRulesEnum.ShowCoordinates:
				ShowCoordinates = value;
				break;
			case GameRulesEnum.NaturalRegeneration:
				NaturalRegeneration = value;
				break;
			case GameRulesEnum.TntExplodes:
				TntExplodes = value;
				break;
			case GameRulesEnum.SendCommandfeedback:
				SendCommandfeedback = value;
				break;
		}
	}

	public void SetGameRule(GameRulesEnum rule, int value)
	{
		switch (rule)
		{
			case GameRulesEnum.DrowningDamage:
				RandomTickSpeed = value;
				break;
		}
	}


	public bool GetGameRule(GameRulesEnum rule)
	{
		switch (rule)
		{
			case GameRulesEnum.DrowningDamage:
				return DrowningDamage;
			case GameRulesEnum.CommandblockOutput:
				return CommandblockOutput;
			case GameRulesEnum.DoTiledrops:
				return DoTiledrops;
			case GameRulesEnum.DoMobloot:
				return DoMobloot;
			case GameRulesEnum.KeepInventory:
				return KeepInventory;
			case GameRulesEnum.DoDaylightcycle:
				return DoDaylightcycle;
			case GameRulesEnum.DoMobspawning:
				return DoMobspawning;
			case GameRulesEnum.DoEntitydrops:
				return DoEntitydrops;
			case GameRulesEnum.DoFiretick:
				return DoFiretick;
			case GameRulesEnum.DoWeathercycle:
				return DoWeathercycle;
			case GameRulesEnum.Pvp:
				return Pvp;
			case GameRulesEnum.Falldamage:
				return Falldamage;
			case GameRulesEnum.Firedamage:
				return Firedamage;
			case GameRulesEnum.Mobgriefing:
				return Mobgriefing;
			case GameRulesEnum.ShowCoordinates:
				return ShowCoordinates;
			case GameRulesEnum.NaturalRegeneration:
				return NaturalRegeneration;
			case GameRulesEnum.TntExplodes:
				return TntExplodes;
			case GameRulesEnum.SendCommandfeedback:
				return SendCommandfeedback;
		}

		return false;
	}

	public virtual GameRules GetGameRules()
	{
		var rules = new GameRules
		{
			new GameRule<bool>(GameRulesEnum.DrowningDamage, DrowningDamage),
			new GameRule<bool>(GameRulesEnum.CommandblockOutput, CommandblockOutput),
			new GameRule<bool>(GameRulesEnum.DoTiledrops, DoTiledrops),
			new GameRule<bool>(GameRulesEnum.DoMobloot, DoMobloot),
			new GameRule<bool>(GameRulesEnum.KeepInventory, KeepInventory),
			new GameRule<bool>(GameRulesEnum.DoDaylightcycle, DoDaylightcycle),
			new GameRule<bool>(GameRulesEnum.DoMobspawning, DoMobspawning),
			new GameRule<bool>(GameRulesEnum.DoEntitydrops, DoEntitydrops),
			new GameRule<bool>(GameRulesEnum.DoFiretick, DoFiretick),
			new GameRule<bool>(GameRulesEnum.DoWeathercycle, DoWeathercycle),
			new GameRule<bool>(GameRulesEnum.Pvp, Pvp),
			new GameRule<bool>(GameRulesEnum.Falldamage, Falldamage),
			new GameRule<bool>(GameRulesEnum.Firedamage, Firedamage),
			new GameRule<bool>(GameRulesEnum.Mobgriefing, Mobgriefing),
			new GameRule<bool>(GameRulesEnum.ShowCoordinates, ShowCoordinates),
			new GameRule<bool>(GameRulesEnum.NaturalRegeneration, NaturalRegeneration),
			new GameRule<bool>(GameRulesEnum.TntExplodes, TntExplodes),
			new GameRule<bool>(GameRulesEnum.SendCommandfeedback, SendCommandfeedback),
			new GameRule<bool>(GameRulesEnum.ExperimentalGameplay, true)
		};
		return rules;
	}

	public void BroadcastSound(Sound sound, Player[] receivers)
	{
		McpeLevelEvent packet = McpeLevelEvent.CreateObject();
		packet.eventId = (LevelEventType) sound.Id;
		packet.data = sound.Pitch * 1000;
		packet.position = sound.Position;
		foreach (Player player in receivers) player.SendPacket(packet);
	}

	public void BroadcastSound(Sound sound, string entityType = null)
	{
		if (entityType != null)
		{
			BroadcastSound(new PlayerLocation(sound.Position), (LevelSoundEventType) sound.Id, entityType);
			return;
		}
		BroadcastSound(sound.Position, (LevelSoundEventType) sound.Id);
	}

	public void BroadcastSound(BlockCoordinates position, LevelSoundEventType sound, int blockId = 0, Player sender = null)
	{
		McpeLevelSoundEvent packet = McpeLevelSoundEvent.CreateObject();
		packet.position = position;
		packet.soundId = (uint) sound;
		packet.extraData = blockId;
		RelayBroadcast(sender, packet);
	}

	public void BroadcastSound(PlayerLocation position, LevelSoundEventType sound, string entityType = null, bool isBabyMob = false) //todo use this for all entities
	{
		McpeLevelSoundEvent packet = McpeLevelSoundEvent.CreateObject();
		packet.position = position;
		packet.soundId = (uint) sound;
		packet.entityType = entityType;
		packet.isBabyMob = isBabyMob;
		RelayBroadcast(packet);
	}
}

public class LevelEventArgs : EventArgs
{
	public LevelEventArgs(Player player, Level level)
	{
		Player = player;
		Level = level;
	}

	public Player Player { get; set; }
	public Level Level { get; set; }
}

public class LevelCancelEventArgs : LevelEventArgs
{
	public LevelCancelEventArgs(Player player, Level level) : base(player, level)
	{
	}

	public bool Cancel { get; set; }
}

public class BlockPlaceEventArgs : LevelCancelEventArgs
{
	public BlockPlaceEventArgs(Player player, Level level, Block targetBlock, Block existingBlock) : base(player, level)
	{
		TargetBlock = targetBlock;
		ExistingBlock = existingBlock;
	}

	public Block TargetBlock { get; private set; }
	public Block ExistingBlock { get; private set; }
}

public class BlockBreakEventArgs : LevelCancelEventArgs
{
	public BlockBreakEventArgs(Player player, Level level, Block block, List<Item> drops) : base(player, level)
	{
		Block = block;
		Drops = drops;
	}

	public Block Block { get; private set; }
	public List<Item> Drops { get; private set; }
}

public class BlockInteractEventArgs : LevelCancelEventArgs
{
	public BlockInteractEventArgs(Player player, Level level, Block block) : base(player, level)
	{
		Block = block;
	}

	public Block Block { get; private set; }
}