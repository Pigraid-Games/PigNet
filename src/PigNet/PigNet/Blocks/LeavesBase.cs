using System;
using System.Collections.Generic;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class LeavesBase : Block
{
	public abstract bool PersistentBit { get; set; }

	public abstract bool UpdateBit { get; set; }

	public LeavesBase()
	{
		IsTransparent = true;
		BlastResistance = 1;
		Hardness = 0.2f;
		IsFlammable = true;
	}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		// No decay
		if (PersistentBit) return;
		if (UpdateBit) return;

		UpdateBit = true;

		level.SetBlock(this, false, false, false);
	}

	public override void OnTick(Level level, bool isRandom)
	{
		if (!isRandom) return;
		if (PersistentBit) return;
		if (!UpdateBit) return;

		if (FindLog(level, Coordinates, [], 0))
		{
			UpdateBit = false;
			level.SetBlock(this, false, false, false);
			return;
		}

		Item[] drops = GetDrops(level, null);
		BreakBlock(level, BlockFace.None, drops.Length == 0);
		foreach (Item drop in drops) level.DropItem(Coordinates, drop);
	}

	public override Item GetItem(Level world, bool blockItem = false)
	{
		return ItemFactory.GetItem(this);
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		if (rnd.Next(20) == 0)
		{
			return this switch
			{
				OakLeaves => [ItemFactory.GetItem<OakSapling>()],
				SpruceLeaves => [ItemFactory.GetItem<SpruceSapling>()],
				BirchLeaves => [ItemFactory.GetItem<BirchSapling>()],
				JungleLeaves => [ItemFactory.GetItem<JungleSapling>()],
				AcaciaLeaves => [ItemFactory.GetItem<AcaciaSapling>()],
				DarkOakLeaves => [ItemFactory.GetItem<DarkOakSapling>()]
			};
		}

		return [];
	}

	private bool FindLog(Level level, BlockCoordinates coord, List<BlockCoordinates> visited, int distance)
	{
		if (visited.Contains(coord)) return false;

		var block = level.GetBlock(coord);
		if (block is LogBase) return true;

		visited.Add(coord);

		if (distance >= 4) return false;

		if (block.GetType() != GetType()) return false;

		// check down
		if (FindLog(level, coord.BlockDown(), visited, distance + 1)) return true;
		// check west
		if (FindLog(level, coord.BlockWest(), visited, distance + 1)) return true;
		// check east
		if (FindLog(level, coord.BlockEast(), visited, distance + 1)) return true;
		// check south
		if (FindLog(level, coord.BlockSouth(), visited, distance + 1)) return true;
		// check north
		if (FindLog(level, coord.BlockNorth(), visited, distance + 1)) return true;
		// check up
		if (FindLog(level, coord.BlockUp(), visited, distance + 1)) return true;

		return false;
	}
}