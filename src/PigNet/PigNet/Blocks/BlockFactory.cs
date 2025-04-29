using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using fNbt;
using log4net;
using Newtonsoft.Json;
using PigNet.Net;
using PigNet.Utils;

namespace PigNet.Blocks;

public interface ICustomBlockFactory
{
	Block GetBlockById(int blockId);
}

public class R12ToCurrentBlockMapEntry(string id, short meta, BlockStateContainer state)
{
	public string StringId { get; set; } = id;
	public short Meta { get; set; } = meta;
	public BlockStateContainer State { get; set; } = state;
}

public static class BlockFactory
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(BlockFactory));
	
	public static ICustomBlockFactory CustomBlockFactory { get; set; }
	public static readonly ConcurrentDictionary<int, byte> TransparentBlocks = new();
	public static readonly ConcurrentDictionary<int, byte> LuminousBlocks = new();
	private static readonly Dictionary<string, int> NameToId;
	public static readonly BlockPalette BlockPalette;
	public static HashSet<BlockStateContainer> BlockStates { get; set; }

	private static readonly ConcurrentDictionary<int, int> LegacyToRuntimeId = new();

	private static readonly IReadOnlyDictionary<int, Func<Block>> BlockFactories = new Dictionary<int, Func<Block>>
	{
		{ 0, () => new Air() },
		{ 1, () => new Stone() },
		{ 2, () => new GrassBlock() },
		{ 3, () => new Dirt() },
		{ 4, () => new Cobblestone() },
		{ 5, () => new PlanksBase() },
		{ 6, () => new SaplingBase() },
		{ 7, () => new Bedrock() },
		{ 8, () => new FlowingWater() },
		{ 9, () => new Water() },
		{ 10, () => new FlowingLava() },
		{ 11, () => new Lava() },
		{ 12, () => new Sand() },
		{ 13, () => new Gravel() },
		{ 14, () => new GoldOre() },
		{ 15, () => new IronOre() },
		{ 16, () => new CoalOre() },
		{ 17, () => new LogBase() },
		{ 18, () => new LeavesBase() },
		{ 19, () => new Sponge() },
		{ 20, () => new Glass() },
		{ 21, () => new LapisOre() },
		{ 22, () => new LapisBlock() },
		{ 23, () => new Dispenser() },
		{ 24, () => new Sandstone() },
		{ 25, () => new Noteblock() },
		{ 26, () => new Bed() },
		{ 27, () => new GoldenRail() },
		{ 28, () => new DetectorRail() },
		{ 29, () => new StickyPiston() },
		{ 30, () => new Web() },
		{ 31, () => new Tallgrass() },
		{ 32, () => new Deadbush() },
		{ 33, () => new Piston() },
		{ 34, () => new PistonArmCollision() },
		{ 35, () => new Wool() },
		{ 37, () => new YellowFlower() },
		{ 38, () => new RedFlower() },
		{ 39, () => new BrownMushroom() },
		{ 40, () => new RedMushroom() },
		{ 41, () => new GoldBlock() },
		{ 42, () => new IronBlock() },
		{ 43, () => new DoubleSlabBase() },
		{ 44, () => new StoneSlab() },
		{ 45, () => new BrickBlock() },
		{ 46, () => new Tnt() },
		{ 47, () => new Bookshelf() },
		{ 48, () => new MossyCobblestone() },
		{ 49, () => new Obsidian() },
		{ 50, () => new Torch() },
		{ 51, () => new Fire() },
		{ 52, () => new MobSpawner() },
		{ 53, () => new OakStairs() },
		{ 54, () => new Chest() },
		{ 55, () => new RedstoneWire() },
		{ 56, () => new DiamondOre() },
		{ 57, () => new DiamondBlock() },
		{ 58, () => new CraftingTable() },
		{ 59, () => new Wheat() },
		{ 60, () => new Farmland() },
		{ 61, () => new Furnace() },
		{ 62, () => new LitFurnace() },
		{ 63, () => new StandingSign() },
		{ 64, () => new WoodenDoor() },
		{ 65, () => new Ladder() },
		{ 66, () => new Rail() },
		{ 67, () => new StoneStairs() },
		{ 68, () => new WallSign() },
		{ 69, () => new Lever() },
		{ 70, () => new StonePressurePlate() },
		{ 71, () => new IronDoor() },
		{ 72, () => new WoodenPressurePlate() },
		{ 73, () => new RedstoneOre() },
		{ 74, () => new LitRedstoneOre() },
		{ 75, () => new UnlitRedstoneTorch() },
		{ 76, () => new RedstoneTorch() },
		{ 77, () => new StoneButton() },
		{ 78, () => new SnowLayer() },
		{ 79, () => new Ice() },
		{ 80, () => new Snow() },
		{ 81, () => new Cactus() },
		{ 82, () => new Clay() },
		{ 83, () => new Reeds() },
		{ 84, () => new Jukebox() },
		{ 85, () => new FenceBase() },
		{ 86, () => new Pumpkin() },
		{ 87, () => new Netherrack() },
		{ 88, () => new SoulSand() },
		{ 89, () => new Glowstone() },
		{ 90, () => new Portal() },
		{ 91, () => new LitPumpkin() },
		{ 92, () => new Cake() },
		{ 93, () => new UnpoweredRepeater() },
		{ 94, () => new PoweredRepeater() },
		{ 95, () => new InvisibleBedrock() },
		{ 96, () => new Trapdoor() },
		{ 97, () => new MonsterEgg() },
		{ 98, () => new Stonebrick() },
		{ 99, () => new BrownMushroomBlock() },
		{ 100, () => new RedMushroomBlock() },
		{ 101, () => new IronBars() },
		{ 102, () => new GlassPane() },
		{ 103, () => new MelonBlock() },
		{ 104, () => new PumpkinStem() },
		{ 105, () => new MelonStem() },
		{ 106, () => new Vine() },
		{ 107, () => new FenceGate() },
		{ 108, () => new BrickStairs() },
		{ 109, () => new StoneBrickStairs() },
		{ 110, () => new Mycelium() },
		{ 111, () => new Waterlily() },
		{ 112, () => new NetherBrick() },
		{ 113, () => new NetherBrickFence() },
		{ 114, () => new NetherBrickStairs() },
		{ 115, () => new NetherWart() },
		{ 116, () => new EnchantingTable() },
		{ 117, () => new BrewingStand() },
		{ 118, () => new Cauldron() },
		{ 119, () => new EndPortal() },
		{ 120, () => new EndPortalFrame() },
		{ 121, () => new EndStone() },
		{ 122, () => new DragonEgg() },
		{ 123, () => new RedstoneLamp() },
		{ 124, () => new LitRedstoneLamp() },
		{ 125, () => new Dropper() },
		{ 126, () => new ActivatorRail() },
		{ 127, () => new Cocoa() },
		{ 128, () => new SandstoneStairs() },
		{ 129, () => new EmeraldOre() },
		{ 130, () => new EnderChest() },
		{ 131, () => new TripwireHook() },
		{ 132, () => new TripWire() },
		{ 133, () => new EmeraldBlock() },
		{ 134, () => new SpruceStairs() },
		{ 135, () => new BirchStairs() },
		{ 136, () => new JungleStairs() },
		{ 137, () => new CommandBlock() },
		{ 138, () => new Beacon() },
		{ 139, () => new CobblestoneWall() },
		{ 140, () => new FlowerPot() },
		{ 141, () => new Carrots() },
		{ 142, () => new Potatoes() },
		{ 143, () => new WoodenButtonBase() },
		{ 144, () => new Skull() },
		{ 145, () => new Anvil() },
		{ 146, () => new TrappedChest() },
		{ 147, () => new LightWeightedPressurePlate() },
		{ 148, () => new HeavyWeightedPressurePlate() },
		{ 149, () => new UnpoweredComparator() },
		{ 150, () => new PoweredComparator() },
		{ 151, () => new DaylightDetector() },
		{ 152, () => new RedstoneBlock() },
		{ 153, () => new QuartzOre() },
		{ 154, () => new Hopper() },
		{ 155, () => new QuartzBlock() },
		{ 156, () => new QuartzStairs() },
		{ 157, () => new DoubleWoodenSlabBase() },
		{ 158, () => new WoodenSlab() },
		{ 159, () => new StainedHardenedClay() },
		{ 160, () => new StainedGlassPane() },
		{ 161, () => new Leaves2() },
		{ 162, () => new Log2() },
		{ 163, () => new AcaciaStairs() },
		{ 164, () => new DarkOakStairs() },
		{ 165, () => new Slime() },
		{ 167, () => new IronTrapdoor() },
		{ 168, () => new Prismarine() },
		{ 169, () => new SeaLantern() },
		{ 170, () => new HayBlock() },
		{ 171, () => new Carpet() },
		{ 172, () => new HardenedClay() },
		{ 173, () => new CoalBlock() },
		{ 174, () => new PackedIce() },
		{ 175, () => new DoublePlanBase() },
		{ 176, () => new StandingBanner() },
		{ 177, () => new WallBanner() },
		{ 178, () => new DaylightDetectorInverted() },
		{ 179, () => new RedSandstone() },
		{ 180, () => new RedSandstoneStairs() },
		{ 181, () => new DoubleStoneSlab2() },
		{ 182, () => new StoneSlab2() },
		{ 183, () => new SpruceFenceGate() },
		{ 184, () => new BirchFenceGate() },
		{ 185, () => new JungleFenceGate() },
		{ 186, () => new DarkOakFenceGate() },
		{ 187, () => new AcaciaFenceGate() },
		{ 188, () => new RepeatingCommandBlock() },
		{ 189, () => new ChainCommandBlock() },
		{ 193, () => new SpruceDoor() },
		{ 194, () => new BirchDoor() },
		{ 195, () => new JungleDoor() },
		{ 196, () => new AcaciaDoor() },
		{ 197, () => new DarkOakDoor() },
		{ 198, () => new GrassPath() },
		{ 199, () => new Frame() },
		{ 200, () => new ChorusFlower() },
		{ 201, () => new PurpurBlock() },
		{ 203, () => new PurpurStairs() },
		{ 205, () => new UndyedShulkerBox() },
		{ 206, () => new EndBricks() },
		{ 207, () => new FrostedIce() },
		{ 208, () => new EndRod() },
		{ 209, () => new EndGateway() },
		{ 210, () => new Allow() },
		{ 211, () => new Deny() },
		{ 212, () => new BorderBlock() },
		{ 213, () => new Magma() },
		{ 214, () => new NetherWartBlock() },
		{ 215, () => new RedNetherBrick() },
		{ 216, () => new BoneBlock() },
		{ 217, () => new StructureVoid() },
		{ 218, () => new ShulkerBox() },
		{ 219, () => new PurpleGlazedTerracottaBase() },
		{ 220, () => new WhiteGlazedTerracottaBase() },
		{ 221, () => new OrangeGlazedTerracottaBase() },
		{ 222, () => new MagentaGlazedTerracottaBase() },
		{ 223, () => new LightBlueGlazedTerracottaBase() },
		{ 224, () => new YellowGlazedTerracottaBase() },
		{ 225, () => new LimeGlazedTerracottaBase() },
		{ 226, () => new PinkGlazedTerracottaBase() },
		{ 227, () => new GrayGlazedTerracottaBase() },
		{ 228, () => new SilverGlazedTerracottaBase() },
		{ 229, () => new CyanGlazedTerracotta() },
		{ 231, () => new BlueGlazedTerracotta() },
		{ 232, () => new BrownGlazedTerracotta() },
		{ 233, () => new GreenGlazedTerracottaBase() },
		{ 234, () => new RedGlazedTerracottaBase() },
		{ 235, () => new BlackGlazedTerracottaBase() },
		{ 236, () => new ConcreteBase() },
		{ 237, () => new ConcretePowder() },
		{ 240, () => new ChorusPlant() },
		{ 241, () => new StainedGlassBase() },
		{ 243, () => new Podzol() },
		{ 244, () => new Beetroot() },
		{ 245, () => new Stonecutter() },
		{ 246, () => new Glowingobsidian() },
		{ 247, () => new Netherreactor() },
		{ 248, () => new InfoUpdate() },
		{ 249, () => new InfoUpdate2() },
		{ 251, () => new Observer() },
		{ 252, () => new StructureBlock() },
		{ 255, () => new Reserved6() },
		{ 256, () => new DoubleSlabBase() },
		{ 257, () => new PrismarineStairs() },
		{ 258, () => new DarkPrismarineStairs() },
		{ 259, () => new PrismarineBricksStairs() },
		{ 260, () => new StrippedSpruceLog() },
		{ 261, () => new StrippedBirchLog() },
		{ 262, () => new StrippedJungleLog() },
		{ 263, () => new StrippedAcaciaLog() },
		{ 264, () => new StrippedDarkOakLog() },
		{ 265, () => new StrippedOakLog() },
		{ 266, () => new BlueIce() },
		{ 385, () => new Seagrass() },
		{ 386, () => new Coral() },
		{ 387, () => new CoralBlock() },
		{ 388, () => new CoralFan() },
		{ 389, () => new CoralFanDead() },
		{ 390, () => new CoralFanHang() },
		{ 391, () => new CoralFanHang2() },
		{ 392, () => new CoralFanHang3() },
		{ 393, () => new Kelp() },
		{ 394, () => new DriedKelpBlock() },
		{ 395, () => new AcaciaButtonBase() },
		{ 396, () => new BirchButton() },
		{ 397, () => new DarkOakButtonBase() },
		{ 398, () => new JungleButtonBase() },
		{ 399, () => new SpruceButtonBase() },
		{ 400, () => new AcaciaTrapdoor() },
		{ 401, () => new BirchTrapdoor() },
		{ 402, () => new DarkOakTrapdoor() },
		{ 403, () => new JungleTrapdoor() },
		{ 404, () => new SpruceTrapdoor() },
		{ 405, () => new AcaciaPressurePlate() },
		{ 406, () => new BirchPressurePlate() },
		{ 407, () => new DarkOakPressurePlate() },
		{ 408, () => new JunglePressurePlate() },
		{ 409, () => new SprucePressurePlate() },
		{ 410, () => new CarvedPumpkin() },
		{ 411, () => new SeaPickle() },
		{ 412, () => new Conduit() },
		{ 414, () => new TurtleEgg() },
		{ 415, () => new BubbleColumn() },
		{ 416, () => new Barrier() },
		{ 417, () => new StoneSlab3() },
		{ 418, () => new Bamboo() },
		{ 419, () => new BambooSapling() },
		{ 420, () => new Scaffolding() },
		{ 421, () => new StoneSlab4() },
		{ 422, () => new DoubleStoneSlab3() },
		{ 423, () => new DoubleStoneSlab4() },
		{ 424, () => new GraniteStairs() },
		{ 425, () => new DioriteStairs() },
		{ 426, () => new AndesiteStairs() },
		{ 427, () => new PolishedGraniteStairs() },
		{ 428, () => new PolishedDioriteStairs() },
		{ 429, () => new PolishedAndesiteStairs() },
		{ 430, () => new MossyStoneBrickStairs() },
		{ 431, () => new SmoothRedSandstoneStairs() },
		{ 432, () => new SmoothSandstoneStairs() },
		{ 433, () => new EndBrickStairs() },
		{ 434, () => new MossyCobblestoneStairs() },
		{ 435, () => new NormalStoneStairs() },
		{ 436, () => new SpruceStandingSign() },
		{ 437, () => new SpruceWallSign() },
		{ 438, () => new SmoothStone() },
		{ 439, () => new RedNetherBrickStairs() },
		{ 440, () => new SmoothQuartzStairs() },
		{ 441, () => new BirchStandingSign() },
		{ 442, () => new BirchWallSign() },
		{ 443, () => new JungleStandingSign() },
		{ 444, () => new JungleWallSign() },
		{ 445, () => new AcaciaStandingSign() },
		{ 446, () => new AcaciaWallSign() },
		{ 447, () => new DarkoakStandingSign() },
		{ 448, () => new DarkoakWallSign() },
		{ 449, () => new Lectern() },
		{ 450, () => new Grindstone() },
		{ 451, () => new BlastFurnace() },
		{ 452, () => new StonecutterBlock() },
		{ 453, () => new Smoker() },
		{ 454, () => new LitSmoker() },
		{ 455, () => new CartographyTable() },
		{ 456, () => new FletchingTable() },
		{ 457, () => new SmithingTable() },
		{ 458, () => new Barrel() },
		{ 459, () => new Loom() },
		{ 461, () => new Bell() },
		{ 462, () => new SweetBerryBush() },
		{ 463, () => new Lantern() },
		{ 464, () => new Campfire() },
		{ 467, () => new Wood() },
		{ 468, () => new Composter() },
		{ 469, () => new LitBlastFurnace() },
		{ 471, () => new WitherRose() },
		{ 472, () => new StickyPistonArmCollision() },
		{ 541, () => new Chain() },
		{ 1220, () => new WitherSkull() },
		{ 1221, () => new ZombieHead() },
		{ 1222, () => new PlayerHead() },
		{ 1223, () => new CreeperHead() },
		{ 1224, () => new DragonHead() }
	};

	private static readonly object LockObj = new();
	
	private static readonly List<R12ToCurrentBlockMapEntry> LegacyStateMap = new();
	private static readonly Dictionary<string, int> IdMapping = new(StringComparer.OrdinalIgnoreCase);

	static BlockFactory()
	{
		for (int i = 0; i < byte.MaxValue * 16; ++i)
			LegacyToRuntimeId.TryAdd(i, -1);

		NameToId = BuildNameToId();

		var assembly = Assembly.GetAssembly(typeof(Block));

		lock (LockObj)
		{
			BlockPalette = new BlockPalette();

			if (assembly != null)
			{
				using Stream stream = assembly.GetManifestResourceStream("PigNet.Resources.blockstates.json");
				if (stream == null)
				{
					Log.Error("blockstates.json resource is missing.");
				}
				else
				{
					using var reader = new StreamReader(stream);
					BlockPalette = BlockPalette.FromJson(reader.ReadToEnd());
				}
			}
			else
			{
				Log.Error("Assembly containing blockstates.json could not be found.");
			}

			// Load block_id_map.json
			LoadIdMapping(assembly);

			// Load r12_to_current_block_map.bin
			LoadR12Mapping(assembly);

			// Fill LegacyToRuntimeId properly based on R12 mapping
			BuildLegacyMappings();

			// Cache NBT states per block
			CacheBlockStatesNbt();
		}

		BlockStates = [.. BlockPalette.Values.ToArray()];
	}
	
	private static void LoadIdMapping(Assembly assembly)
	{
		using Stream idMappingStream = assembly.GetManifestResourceStream("PigNet.Resources.block_id_map.json");
		if (idMappingStream == null)
		{
			Log.Error("block_id_map.json is missing.");
			return;
		}

		using var reader = new StreamReader(idMappingStream);
		var parsed = JsonConvert.DeserializeObject<Dictionary<string, int>>(reader.ReadToEnd());
		foreach (var pair in parsed)
		{
			IdMapping[pair.Key] = pair.Value;
		}
	}

	private static void LoadR12Mapping(Assembly assembly)
	{
		using Stream r12MapStream = assembly.GetManifestResourceStream("PigNet.Resources.r12_to_current_block_map.bin");
		if (r12MapStream == null)
		{
			Log.Error("r12_to_current_block_map.bin is missing.");
			return;
		}

		while (r12MapStream.Position < r12MapStream.Length)
		{
			var length = VarInt.ReadUInt32(r12MapStream);
			var stringBytes = new byte[length];
			r12MapStream.Read(stringBytes, 0, stringBytes.Length);
			var stringId = Encoding.UTF8.GetString(stringBytes);

			var metaBytes = new byte[2];
			r12MapStream.Read(metaBytes, 0, metaBytes.Length);
			var meta = BitConverter.ToInt16(metaBytes);

			var compound = Packet.ReadNbtCompound(r12MapStream, true);

			LegacyStateMap.Add(new R12ToCurrentBlockMapEntry(stringId, meta, GetBlockStateContainer(compound)));
		}
	}
	
	private static void BuildLegacyMappings()
	{
		var idToStatesMap = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

		foreach (var container in BlockPalette.Values)
		{
			if (!idToStatesMap.TryGetValue(container.Name, out var list))
			{
				list = new List<int>();
				idToStatesMap[container.Name] = list;
			}
			list.Add(container.RuntimeId);
		}

		foreach (var entry in LegacyStateMap)
		{
			if (!IdMapping.TryGetValue(entry.StringId, out int blockId))
				continue;

			if (!idToStatesMap.TryGetValue(entry.State.Name, out var matchingStates))
				continue;

			foreach (var match in matchingStates)
			{
				var networkState = BlockPalette[match];

				var thisStates = new HashSet<IBlockState>(entry.State.States);
				var otherStates = new HashSet<IBlockState>(networkState.States);

				otherStates.IntersectWith(thisStates);

				if (otherStates.Count != thisStates.Count) continue;
				BlockPalette[match].Id = blockId;
				BlockPalette[match].Data = entry.Meta;
				BlockPalette[match].ItemInstance = new ItemPickInstance
				{
					Id = (short)blockId,
					Metadata = entry.Meta,
					WantNbt = false
				};
				LegacyToRuntimeId.TryUpdate((blockId << 4) | (byte)entry.Meta, match);
				break;
			}
		}
	}

	private static void CacheBlockStatesNbt()
	{
		foreach (var record in BlockPalette.Values)
		{
			var states = new List<NbtTag>();
			foreach (IBlockState state in record.States)
			{
				NbtTag stateTag = state switch
				{
					BlockStateByte b => new NbtByte(b.Name, b.Value),
					BlockStateInt i => new NbtInt(i.Name, i.Value),
					BlockStateString s => new NbtString(s.Name, s.Value),
					_ => throw new ArgumentOutOfRangeException(nameof(state))
				};
				states.Add(stateTag);
			}

			var nbt = new NbtFile
			{
				BigEndian = false,
				UseVarInt = true,
				RootTag = new NbtCompound("states", states)
			};

			record.StatesCacheNbt = nbt.SaveToBuffer(NbtCompression.None);
		}
	}
	
	private static BlockStateContainer GetBlockStateContainer(NbtTag tag)
	{
		return new BlockStateContainer
		{
			Name = tag["name"].StringValue,
			States = GetBlockStates(tag)
		};
	}

	private static List<IBlockState> GetBlockStates(NbtTag tag)
	{
		var result = new List<IBlockState>();

		if (tag["states"] is NbtCompound compound)
		{
			foreach (var stateEntry in compound)
			{
				switch (stateEntry)
				{
					case NbtInt nbtInt:
						result.Add(new BlockStateInt { Name = nbtInt.Name, Value = nbtInt.Value });
						break;
					case NbtByte nbtByte:
						result.Add(new BlockStateByte { Name = nbtByte.Name, Value = nbtByte.Value });
						break;
					case NbtString nbtString:
						result.Add(new BlockStateString { Name = nbtString.Name, Value = nbtString.Value });
						break;
				}
			}
		}

		return result;
	}

	private static Dictionary<string, int> BuildNameToId()
	{
		var nameToId = new Dictionary<string, int>();
		for (int idx = 0; idx < 1000; idx++)
		{
			Block block = GetBlockById(idx);
			if (block == null || block.GetType().Name.ToLowerInvariant() == "block") continue;

			string blockName = block.GetType().Name.ToLowerInvariant();
			if (!nameToId.TryAdd(blockName, idx)) Log.Warn($"Duplicate block name detected: {blockName} for ID {idx}. Ignoring.");
		}

		return nameToId;
	}


	public static int GetBlockIdByName(string blockName)
	{
		blockName = blockName.ToLowerInvariant().Replace("_", "").Replace("minecraft:", "");
		return NameToId.GetValueOrDefault(blockName, 0);
	}

	public static Block GetBlockByName(string blockName)
	{
		if (string.IsNullOrEmpty(blockName)) return null;

		blockName = blockName.ToLowerInvariant().Replace("_", "").Replace("minecraft:", "");
		if (NameToId.TryGetValue(blockName, out int value)) return GetBlockById(value);

		Log.Warn($"Block name {blockName} not found in NameToId. Returning null.");
		return null;
	}


	public static Block GetBlockById(int blockId, byte metadata)
	{
		int runtimeId = (int) GetRuntimeId(blockId, metadata);
		if (!BlockPalette.TryGetValue(runtimeId, out BlockStateContainer blockState)) return null;

		Block block = GetBlockById(blockState.Id);
		block.SetState(blockState.States);
		return block;
	}

	public static Block GetBlockByRuntimeId(int runtimeId)
	{
		if (!BlockPalette.TryGetValue(runtimeId, out BlockStateContainer blockState)) return null;

		Block block = GetBlockById(blockState.Id);
		block.SetState(blockState.States);
		return block;
	}

	public static Block GetBlockById(int blockId)
	{
		if (blockId < 0 || blockId >= 10000) // Arbitrary upper limit for valid block IDs
		{
			Log.Warn($"Invalid Block ID {blockId}. Using a generic Block instance.");
			return new Block(blockId);
		}

		Block block = CustomBlockFactory?.GetBlockById(blockId);
		if (block != null) return block;

		return BlockFactories.TryGetValue(blockId, out Func<Block> factory)
			? factory()
			: LogMissingBlockId(blockId);
	}


	private static Block LogMissingBlockId(int blockId)
	{
		return new Block(blockId);
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetRuntimeId(int blockId, byte metadata)
	{
		int idx = TryGetRuntimeId(blockId, metadata);
		if (idx != -1) return (uint) idx;

		int fallbackIdx = TryGetRuntimeId(blockId, 0);
		if (fallbackIdx != -1) return (uint) fallbackIdx;

		return (uint) TryGetRuntimeId(0, 0); // Fallback to air block.
	}

	public static uint GetItemRuntimeId(int blockId, byte metadata)
	{
		if (blockId < 0) blockId = 255 + Math.Abs(blockId);
		int idx = TryGetRuntimeId(blockId, metadata);
		if (idx != -1) return (uint) idx;

		return (uint) TryGetRuntimeId(0, 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int TryGetRuntimeId(int blockId, byte metadata)
	{
		int key = (blockId << 4) | metadata;

		if (LegacyToRuntimeId.TryGetValue(key, out int runtimeId)) return runtimeId;

		return -1; // Indicate missing runtime ID.
	}

	public static string GetBlockColor(int blockId, byte metadata)
	{
		if (!BlockPalette.TryGetValue((int) GetRuntimeId(blockId, metadata), out BlockStateContainer states))
		{
			Log.Warn($"Block color not found for Block ID {blockId} with metadata {metadata}. Returning empty string.");
			return "";
		}

		foreach (IBlockState state in states.States)
			if (state is BlockStateString s && s.Name == "color")
				return s.Value;

		return "";
	}
}