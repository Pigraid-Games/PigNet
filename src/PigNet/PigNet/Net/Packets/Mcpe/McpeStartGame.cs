#region LICENSE
// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using System.Numerics;
using log4net;
using PigNet.Utils;
using PigNet.Utils.Nbt;

namespace PigNet.Net.Packets.Mcpe;

public class McpeStartGame : Packet<McpeStartGame>
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(McpeStartGame));
	public bool blockNetworkIdsAreHashes;
	public BlockPalette blockPalette;
	public ulong blockPaletteChecksum;
	public bool clientSideGenerationEnabled;
	public long currentTick;
	public bool enableNewBlockBreakSystem;
	public bool enableNewInventorySystem;
	public int enchantmentSeed;

	public long entityIdSelf;
	public bool isTrial;
	public string levelId;

	public LevelSettings levelSettings = new();
	public int movementRewindHistorySize;
	public int movementType;
	public string multiplayerCorrelationId;
	public int playerGamemode;
	public string premiumWorldTemplateId;
	public Nbt propertyData;
	public Vector2 rotation;
	public long runtimeEntityId;
	public string scenarioId;
	public string serverId;
	public string serverVersion;
	public Vector3 spawn;
	public string worldId;
	public string worldName;
	public UUID worldTemplateId;
	
	public McpeStartGame()
	{
		Id = 0x0b;
		IsMcpe = true;
	}
	
	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(entityIdSelf);
		WriteUnsignedVarLong(runtimeEntityId);
		WriteSignedVarInt(playerGamemode);
		Write(spawn);
		Write(rotation);
			
		LevelSettings s = levelSettings ?? new LevelSettings();
		s.Write(this);
		Write(serverId);
		Write(worldId);
		Write(scenarioId);
		Write(levelId);
		Write(worldName);
		Write(premiumWorldTemplateId);
		Write(isTrial);
			
		//Player movement settings
		WriteSignedVarInt(movementType);
		WriteSignedVarInt(movementRewindHistorySize);
		Write(enableNewBlockBreakSystem);
			
		Write(currentTick);
		WriteSignedVarInt(enchantmentSeed);
			
		Write(blockPalette);
			
		Write(multiplayerCorrelationId);
		Write(enableNewInventorySystem);
		Write(serverVersion);
		Write(propertyData);
		Write(blockPaletteChecksum);
		Write(worldTemplateId);
		Write(clientSideGenerationEnabled);
		Write(blockNetworkIdsAreHashes);
		Write(false);
	}
	
	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		entityIdSelf = ReadSignedVarLong();
		runtimeEntityId = ReadUnsignedVarLong();
		playerGamemode = ReadSignedVarInt();
		spawn = ReadVector3();
		rotation = ReadVector2();

		levelSettings = new LevelSettings();
		levelSettings.Read(this);

		serverId = ReadString();
		worldId = ReadString();
		scenarioId = ReadString();

		levelId = ReadString();
		worldName = ReadString();
		premiumWorldTemplateId = ReadString();
		isTrial = ReadBool();

		//Player movement settings
		movementType = ReadSignedVarInt();
		movementRewindHistorySize = ReadSignedVarInt();
		enableNewBlockBreakSystem = ReadBool();

		currentTick = ReadLong();
		enchantmentSeed = ReadSignedVarInt();

		try
		{
			blockPalette = ReadBlockPalette();
		}
		catch (Exception ex)
		{
			Log.Warn("Failed to read complete blockpallete", ex);
			return;
		}

		multiplayerCorrelationId = ReadString();
		enableNewInventorySystem = ReadBool();
		serverVersion = ReadString();
		propertyData = ReadNbt();
		blockPaletteChecksum = ReadUlong();
		worldTemplateId = ReadUUID();
		clientSideGenerationEnabled = ReadBool();
		blockNetworkIdsAreHashes = ReadBool();
		ReadBool();
	}

	public override void Reset()
	{
		entityIdSelf = default;
		runtimeEntityId = default;
		playerGamemode = default;
		spawn = default;
		rotation = default;
		levelSettings = default;
		serverId = default;
		worldId = default;
		scenarioId = default;
		levelId = default;
		worldName = default;
		premiumWorldTemplateId = default;
		isTrial = default;
		movementType = default;
		movementRewindHistorySize = default;
		enableNewBlockBreakSystem = default;
		currentTick = default;
		enchantmentSeed = default;
		blockPalette = default;
		multiplayerCorrelationId = default;
		enableNewInventorySystem = default;
		serverVersion = default;
		propertyData = default;
		worldTemplateId = default;
		clientSideGenerationEnabled = default;
		blockNetworkIdsAreHashes = default;
		base.Reset();
	}
}

public class SpawnSettings
{
	public short BiomeType { get; set; }
	public string BiomeName { get; set; }
	public int Dimension { get; set; }

	public void Read(Packet packet)
	{
		BiomeType = packet.ReadShort();
		BiomeName = packet.ReadString();
		Dimension = packet.ReadVarInt();
	}

	public void Write(Packet packet)
	{
		packet.Write(BiomeType);
		packet.Write(BiomeName);
		packet.WriteVarInt(Dimension);
	}
}

public class LevelSettings
{
	public bool bonusChest;
	public bool broadcastToLan;
	public byte chatRestrictionLevel;
	public bool createdInEditorMode;
	public int difficulty;
	public bool editorWorld;
	public int eduOffer;
	public string eduProductUuid;
	public EducationUriResource eduSharedUriResource;
	public bool emoteChatMuted;
	public bool enableCommands;
	public bool experimentalGameplayOverride;
	public Experiments experiments;
	public bool exportedFromEditorMode;
	public int gamemode;
	public GameRules gamerules;
	public string gameVersion;

	public int generator;
	public bool hardcoreEnabled;
	public bool hasAchievementsDisabled;
	public bool hasConfirmedPlatformLockedContent;
	public bool hasEduFeaturesEnabled;
	public bool hasLockedBehaviorPack;
	public bool hasLockedResourcePack;
	public bool isDisablePlayerInteractions;
	public bool isDisablingCustomSkins;
	public bool isDisablingPersonas;
	public bool isFromLockedWorldTemplate;
	public bool isFromWorldTemplate;
	public bool isMultiplayer;
	public bool isNewNether;
	public bool isTexturepacksRequired;
	public bool isWorldTemplateOptionLocked;
	public float lightningLevel;
	public int limitedWorldLength;
	public int limitedWorldWidth;
	public bool mapEnabled;
	public bool onlySpawnV1Villagers;
	public byte permissionLevel;
	public int platformBroadcastMode;
	public float rainLevel;
	public long seed;
	public int serverChunkTickRange;
	public SpawnSettings spawnSettings;
	public int time;
	public bool useMsaGamertagsOnly;
	public int x;
	public int xboxLiveBroadcastMode;
	public int y;
	public int z;


	public void Write(Packet packet)
	{
		packet.Write(seed);

		SpawnSettings s = spawnSettings ?? new SpawnSettings();
		s.Write(packet);

		packet.WriteSignedVarInt(generator);
		packet.WriteSignedVarInt(gamemode);
		packet.Write(false); //hardcore
		packet.WriteSignedVarInt(difficulty);

		packet.WriteSignedVarInt(x);
		packet.WriteVarInt(y);
		packet.WriteSignedVarInt(z);

		packet.Write(hasAchievementsDisabled);
		packet.Write(editorWorld);
		packet.Write(createdInEditorMode);
		packet.Write(exportedFromEditorMode);
		packet.WriteSignedVarInt(time);
		packet.WriteSignedVarInt(eduOffer);
		packet.Write(hasEduFeaturesEnabled);
		packet.Write(eduProductUuid);
		packet.Write(rainLevel);
		packet.Write(lightningLevel);
		packet.Write(hasConfirmedPlatformLockedContent);
		packet.Write(isMultiplayer);
		packet.Write(broadcastToLan);
		packet.WriteVarInt(xboxLiveBroadcastMode);
		packet.WriteVarInt(platformBroadcastMode);
		packet.Write(enableCommands);
		packet.Write(isTexturepacksRequired);
		packet.Write(gamerules);
		packet.Write(experiments);
		packet.Write(false); //ExperimentsPreviouslyToggled
		packet.Write(bonusChest);
		packet.Write(mapEnabled);
		packet.Write(permissionLevel);
		packet.Write(serverChunkTickRange);
		packet.Write(hasLockedBehaviorPack);
		packet.Write(hasLockedResourcePack);
		packet.Write(isFromLockedWorldTemplate);
		packet.Write(useMsaGamertagsOnly);
		packet.Write(isFromWorldTemplate);
		packet.Write(isWorldTemplateOptionLocked);
		packet.Write(onlySpawnV1Villagers);
		packet.Write(isDisablingPersonas);
		packet.Write(isDisablingCustomSkins);
		packet.Write(emoteChatMuted);
		packet.Write(gameVersion);
		packet.Write(limitedWorldWidth);
		packet.Write(limitedWorldLength);
		packet.Write(isNewNether);
		packet.Write(eduSharedUriResource ?? new EducationUriResource("", ""));
		packet.Write(false);
		packet.Write(chatRestrictionLevel);
		packet.Write(isDisablePlayerInteractions);
	}

	public void Read(Packet packet)
	{
		seed = packet.ReadLong();

		spawnSettings = new SpawnSettings();
		spawnSettings.Read(packet);

		generator = packet.ReadSignedVarInt();
		gamemode = packet.ReadSignedVarInt();
		hardcoreEnabled = packet.ReadBool();
		difficulty = packet.ReadSignedVarInt();

		x = packet.ReadSignedVarInt();
		y = packet.ReadVarInt();
		z = packet.ReadSignedVarInt();

		hasAchievementsDisabled = packet.ReadBool();
		editorWorld = packet.ReadBool();
		createdInEditorMode = packet.ReadBool();
		exportedFromEditorMode = packet.ReadBool();
		time = packet.ReadSignedVarInt();
		eduOffer = packet.ReadSignedVarInt();
		hasEduFeaturesEnabled = packet.ReadBool();
		eduProductUuid = packet.ReadString();
		rainLevel = packet.ReadFloat();
		lightningLevel = packet.ReadFloat();
		hasConfirmedPlatformLockedContent = packet.ReadBool();
		isMultiplayer = packet.ReadBool();
		broadcastToLan = packet.ReadBool();
		xboxLiveBroadcastMode = packet.ReadVarInt();
		platformBroadcastMode = packet.ReadVarInt();
		enableCommands = packet.ReadBool();
		isTexturepacksRequired = packet.ReadBool();
		gamerules = packet.ReadGameRules();
		experiments = packet.ReadExperiments();
		packet.ReadBool();
		bonusChest = packet.ReadBool();
		mapEnabled = packet.ReadBool();
		permissionLevel = packet.ReadByte();
		serverChunkTickRange = packet.ReadInt();
		hasLockedBehaviorPack = packet.ReadBool();
		hasLockedResourcePack = packet.ReadBool();
		isFromLockedWorldTemplate = packet.ReadBool();
		useMsaGamertagsOnly = packet.ReadBool();
		isFromWorldTemplate = packet.ReadBool();
		isWorldTemplateOptionLocked = packet.ReadBool();
		onlySpawnV1Villagers = packet.ReadBool();
		isDisablingPersonas = packet.ReadBool();
		isDisablingCustomSkins = packet.ReadBool();
		emoteChatMuted = packet.ReadBool();
		gameVersion = packet.ReadString();

		limitedWorldWidth = packet.ReadInt();
		limitedWorldLength = packet.ReadInt();
		isNewNether = packet.ReadBool();
		eduSharedUriResource = packet.ReadEducationUriResource();

		experimentalGameplayOverride = packet.ReadBool() && packet.ReadBool();
		chatRestrictionLevel = packet.ReadByte();
		isDisablePlayerInteractions = packet.ReadBool();
	}
}