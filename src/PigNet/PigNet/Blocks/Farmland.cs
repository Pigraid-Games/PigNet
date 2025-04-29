using System;
using System.Collections.Generic;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Farmland : Block
{

	public Farmland()
	{
		IsTransparent = true;
		IsBlockingSkylight = false;.
		BlastResistance = 3;
		Hardness = 0.6f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return new[] { ItemFactory.GetItem<Dirt>() };
	}

	public override void OnTick(Level level, bool isRandom)
	{
		int data = MoisturizedAmount;
		MoisturizedAmount = FindWater(level, Coordinates, [], 0) ? 7 : Math.Max(0, MoisturizedAmount - 1);
		if (data != MoisturizedAmount) level.SetBlock(this);
	}

	public bool FindWater(Level level, BlockCoordinates coord, List<BlockCoordinates> visited, int distance)
	{
		if (visited.Contains(coord)) return false;

		Block block = level.GetBlock(coord);
		if (block is Water or FlowingWater) return true;

		visited.Add(coord);

		if (distance >= 4) return false;

		// check down
		//if (FindWater(level, coord + BlockCoordinates.Down, visited, distance + 1)) return true;
		// check west
		if (FindWater(level, coord.BlockWest(), visited, distance + 1)) return true;
		// check east
		if (FindWater(level, coord.BlockEast(), visited, distance + 1)) return true;
		// check south
		if (FindWater(level, coord.BlockSouth(), visited, distance + 1)) return true;
		// check north
		if (FindWater(level, coord.BlockNorth(), visited, distance + 1)) return true;
		// check up
		//if (FindWater(level, coord + BlockCoordinates.Up, visited, distance + 1)) return true;

		return false;
	}
}