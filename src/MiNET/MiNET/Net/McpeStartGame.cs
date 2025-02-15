using System;
using System.Numerics;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Nbt;

namespace MiNET.Net;

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
	public bool bonusChest; // = null;
	public bool broadcastToLan; // = null;
	public byte chatRestrictionLevel; // = null;
	public bool createdInEditorMode; // = null;
	public int difficulty; // = null;
	public bool editorWorld;
	public int eduOffer; // = null;
	public string eduProductUuid; // = null;
	public EducationUriResource eduSharedUriResource;
	public bool emoteChatMuted; // = null;
	public bool enableCommands; // = null;
	public bool experimentalGameplayOverride; // = null;
	public Experiments experiments;
	public bool exportedFromEditorMode; // = null;
	public int gamemode; // = null;
	public GameRules gamerules; // = null;
	public string gameVersion; // = null;

	public int generator; // = null;
	public bool hardcoreEnabled; // = null;
	public bool hasAchievementsDisabled; // = null;
	public bool hasConfirmedPlatformLockedContent; // = null;
	public bool hasEduFeaturesEnabled; // = null;
	public bool hasLockedBehaviorPack; // = null;
	public bool hasLockedResourcePack; // = null;
	public bool isDisablePlayerInteractions; // = null;
	public bool isDisablingCustomSkins; // = null;
	public bool isDisablingPersonas; // = null;
	public bool isFromLockedWorldTemplate; // = null;
	public bool isFromWorldTemplate; // = null;
	public bool isMultiplayer; // = null;
	public bool isNewNether; // = null;
	public bool isTexturepacksRequired; // = null;
	public bool isWorldTemplateOptionLocked; // = null;
	public float lightningLevel; // = null;
	public int limitedWorldLength; // = null;
	public int limitedWorldWidth; // = null;
	public bool mapEnabled; // = null;
	public bool onlySpawnV1Villagers; // = null;
	public byte permissionLevel; // = null;
	public int platformBroadcastMode; // = null;
	public float rainLevel; // = null;
	public long seed; // = null;
	public int serverChunkTickRange; // = null;
	public SpawnSettings spawnSettings;
	public int time; // = null;
	public bool useMsaGamertagsOnly; // = null;
	public int x; // = null;
	public int xboxLiveBroadcastMode; // = null;
	public int y; // = null;
	public int z; // = null;


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

		if (packet.ReadBool())
			experimentalGameplayOverride = packet.ReadBool();
		else
			experimentalGameplayOverride = false;
		chatRestrictionLevel = packet.ReadByte();
		isDisablePlayerInteractions = packet.ReadBool();
	}
}

public partial class McpeStartGame : Packet<McpeStartGame>
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(McpeStartGame));
	public bool blockNetworkIdsAreHashes;
	public BlockPalette blockPalette; // = null;
	public ulong blockPaletteChecksum;
	public bool clientSideGenerationEnabled;
	public long currentTick; // = null;
	public bool enableNewBlockBreakSystem; // = null;
	public bool enableNewInventorySystem; // = null;
	public int enchantmentSeed; // = null;

	public long entityIdSelf; // = null;
	public bool isTrial; // = null;
	public string levelId; // = null;

	public LevelSettings levelSettings = new();
	public int movementRewindHistorySize; // = null;
	public int movementType; // = null;
	public string multiplayerCorrelationId; // = null;
	public int playerGamemode; // = null;
	public string premiumWorldTemplateId; // = null;
	public Nbt propertyData;
	public Vector2 rotation; // = null;
	public long runtimeEntityId; // = null;
	public string scenarioId; // = null;
	public string serverId; // = null;
	public string serverVersion; // = null;
	public Vector3 spawn; // = null;
	public string worldId; // = null;
	public string worldName; // = null;
	public UUID worldTemplateId;

	partial void AfterEncode()
	{
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

	partial void AfterDecode()
	{
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

	/// <inheritdoc />
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