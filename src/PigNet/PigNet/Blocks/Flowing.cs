using System;
using System.Numerics;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class Flowing : Block
{
	public abstract int LiquidDepth { get; set; }

	public string StationeryId { get; }

	private int _adjacentSources;
	private int[] _flowCost = new int[4];
	private bool[] _optimalFlowDirections = new bool[4];

	protected Flowing(string stationeryId)
	{
		StationeryId = stationeryId;

		IsSolid = false;
		IsBuildable = false;
		IsReplaceable = true;
		IsTransparent = true;
	}

	public override void BlockAdded(Level level)
	{
		if (!CheckForHarden(level, Coordinates)) level.ScheduleBlockTick(this, TickRate());
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (!CheckForHarden(world, blockCoordinates)) world.ScheduleBlockTick(this, TickRate());

		return false;
	}

	public override void DoPhysics(Level level)
	{
		CheckForHarden(level, Coordinates);
		level.ScheduleBlockTick(this, TickRate());
	}

	public override void OnTick(Level world, bool isRandom)
	{
		if (isRandom) return;

		var random = new Random();

		int x = Coordinates.X;
		int y = Coordinates.Y;
		int z = Coordinates.Z;
		BlockCoordinates current = Coordinates;
		int currentDecay = LiquidDepth;
		byte multiplier = 1;

		if (this is FlowingLava) multiplier = 2;

		int tickRate = TickRate();

		if (currentDecay > 0)
		{
			int smallestFlowDecay = -100;
			_adjacentSources = 0;
			smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Left, smallestFlowDecay);
			smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Right, smallestFlowDecay);
			smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Backwards, smallestFlowDecay);
			smallestFlowDecay = GetSmallestFlowDecay(world, current + BlockCoordinates.Forwards, smallestFlowDecay);
			int newDecay = smallestFlowDecay + multiplier;
			if (newDecay >= 8 || smallestFlowDecay < 0) newDecay = -1;

			if (GetFlowDecay(world, current + BlockCoordinates.Up) >= 0)
			{
				int topFlowDecay = GetFlowDecay(world, current + BlockCoordinates.Up);

				if (topFlowDecay >= 8) newDecay = topFlowDecay;
				else newDecay = topFlowDecay + 8;
			}

			if (_adjacentSources >= 2 && this is FlowingWater)
			{
				if (world.GetBlock(current + BlockCoordinates.Down).IsSolid) newDecay = 0;
				else if (IsSameMaterial(world.GetBlock(current + BlockCoordinates.Down)) && 
						GetLiquidDepth(world.GetBlock(current + BlockCoordinates.Down)) == 0) newDecay = 0;
			}

			if (this is FlowingLava && currentDecay < 8 && newDecay < 8 && newDecay > currentDecay && random.Next(4) != 0) tickRate *= 4;

			if (newDecay == currentDecay)
				SetToStill(world, current);
			else
			{
				currentDecay = newDecay;
				if (newDecay < 0) world.SetAir(current);
				else
				{
					LiquidDepth = newDecay;
					world.SetBlock(this);
					world.ApplyPhysics(x, y, z);
					world.ScheduleBlockTick(this, tickRate);
				}
			}
		}
		else SetToStill(world, current);

		if (CanBeFlownInto(world, current + BlockCoordinates.Down))
		{
			if (this is FlowingLava && (world.GetBlock(x, y - 1, z) is FlowingWater || world.GetBlock(x, y - 1, z) is Water))
			{
				world.SetBlock(new Cobblestone { Coordinates = new BlockCoordinates(x, y - 1, z) });
				return;
			}

			if (currentDecay >= 8) Flow(world, current + BlockCoordinates.Down, currentDecay);
			else Flow(world, current + BlockCoordinates.Down, currentDecay + 8);
		}
		else if (currentDecay >= 0 && (currentDecay == 0 || BlocksFluid(world, x, y - 1, z)))
		{
			bool[] optimalFlowDirections = GetOptimalFlowDirections(world, x, y, z);

			int newDecay = currentDecay + multiplier;
			if (currentDecay >= 8) newDecay = 1;
			if (newDecay >= 8) return;
			if (optimalFlowDirections[0]) Flow(world, current + BlockCoordinates.Left, newDecay);
			if (optimalFlowDirections[1]) Flow(world, current + BlockCoordinates.Right, newDecay);
			if (optimalFlowDirections[2]) Flow(world, current + BlockCoordinates.Backwards, newDecay);
			if (optimalFlowDirections[3]) Flow(world, current + BlockCoordinates.Forwards, newDecay);
		}
	}

	private bool[] GetOptimalFlowDirections(Level world, int x, int y, int z)
	{
		int l;
		int x2;

		for (l = 0; l < 4; ++l)
		{
			_flowCost[l] = 1000;
			x2 = x;
			int z2 = z;

			switch (l)
			{
				case 0:
					x2 = x - 1;
					break;
				case 1:
					++x2;
					break;
				case 2:
					z2 = z - 1;
					break;
				case 3:
					++z2;
					break;
			}

			if (BlocksFluid(world, x2, y, z2) || (IsSameMaterial(world.GetBlock(x2, y, z2)) && GetLiquidDepth(world.GetBlock(x2, y, z2)) == 0)) continue;
			if (BlocksFluid(world, x2, y - 1, z2)) _flowCost[l] = CalculateFlowCost(world, x2, y, z2, 1, l);
			else _flowCost[l] = 0;
		}

		l = _flowCost[0];

		for (x2 = 1; x2 < 4; ++x2) if (_flowCost[x2] < l) l = _flowCost[x2];
		for (x2 = 0; x2 < 4; ++x2) _optimalFlowDirections[x2] = _flowCost[x2] == l;

		return _optimalFlowDirections;
	}

	private int GetLiquidDepth(Block block)
	{
		return block switch
		{
			Flowing flowing => flowing.LiquidDepth,
			Stationary stationary => stationary.LiquidDepth,
			_ => -1
		};
	}

	private int CalculateFlowCost(Level world, int x, int y, int z, int accumulatedCost, int prevDirection)
	{
		int cost = 1000;

		for (int direction = 0; direction < 4; ++direction)
		{
			if ((direction == 0 && prevDirection == 1)
				|| (direction == 1 && prevDirection == 0)
				|| (direction == 2 && prevDirection == 3)
				|| (direction == 3 && prevDirection == 2)) continue;
			int x2 = x;
			int z2 = z;

			switch (direction)
			{
				case 0:
					x2 = x - 1;
					break;
				case 1:
					++x2;
					break;
				case 2:
					z2 = z - 1;
					break;
				case 3:
					++z2;
					break;
			}

			if (BlocksFluid(world, x2, y, z2) || (IsSameMaterial(world.GetBlock(x2, y, z2)) && GetLiquidDepth(world.GetBlock(x2, y, z2)) == 0)) continue;
			if (!BlocksFluid(world, x2, y - 1, z2)) return accumulatedCost;

			if (accumulatedCost >= 4) continue;
			int j2 = CalculateFlowCost(world, x2, y, z2, accumulatedCost + 1, direction);

			if (j2 < cost) cost = j2;
		}

		return cost;
	}

	private void Flow(Level world, BlockCoordinates coord, int decay)
	{
		if (!CanBeFlownInto(world, coord)) return;
		var newBlock = (Flowing) BlockFactory.GetBlockById(Id);
		newBlock.Coordinates = new BlockCoordinates(coord);
		newBlock.LiquidDepth = decay;
		world.SetBlock(newBlock, applyPhysics: true);
		world.ScheduleBlockTick(newBlock, TickRate());
	}

	private bool CanBeFlownInto(Level world, BlockCoordinates coord)
	{
		Block block = world.GetBlock(coord);

		return !IsSameMaterial(block) && (!(block is FlowingLava) && !(block is Lava)) && !BlocksFluid(block);
	}


	private bool BlocksFluid(Level world, int x, int y, int z)
	{
		Block block = world.GetBlock(x, y, z);

		return BlocksFluid(block);
	}

	private bool BlocksFluid(Block block)
	{
		return block.IsSolid;
	}


	private void SetToStill(Level world, BlockCoordinates coord)
	{
		var block = (Flowing) world.GetBlock(coord);

		var stillBlock = (Stationary) BlockFactory.GetBlockById(StationeryId);
		stillBlock.LiquidDepth = block.LiquidDepth;
		stillBlock.Coordinates = new BlockCoordinates(coord);
		world.SetBlock(stillBlock, applyPhysics: false);
	}

	private int GetSmallestFlowDecay(Level world, BlockCoordinates coord, int decay)
	{
		int blockDecay = GetFlowDecay(world, coord);

		switch (blockDecay)
		{
			case < 0:
				return decay;
			case 0:
				++_adjacentSources;
				break;
			case >= 8:
				blockDecay = 0;
				break;
		}

		return decay >= 0 && blockDecay >= decay ? decay : blockDecay;
	}

	private int GetFlowDecay(Level world, BlockCoordinates coord)
	{
		Block block = world.GetBlock(coord);

		int liquidDepth;
		switch (block)
		{
			case Flowing flowing:
				liquidDepth = flowing.LiquidDepth;
				break;
			case Stationary stationary:
				liquidDepth = stationary.LiquidDepth;
				break;
			default:
				return -1;
		}

		return IsSameMaterial(block) ? liquidDepth : -1;
	}


	private bool IsSameMaterial(Block block)
	{
		if (this is FlowingWater && block is FlowingWater or Water) return true;
		return this is FlowingLava && block is FlowingLava or Lava;
	}

	private int TickRate()
	{
		return this is FlowingWater ? 5 : (this is FlowingLava ? 30 : 0);
	}

	private bool CheckForHarden(Level world, BlockCoordinates coord)
	{
		var block = world.GetBlock(coord) as Flowing;

		bool harden = false;
		if (block is not FlowingLava) return false;
		if (IsWater(world, coord + BlockCoordinates.Backwards)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Forwards)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Left)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Right)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Up)) harden = true;

		if (!harden) return false;
		int liquidDepth = block.LiquidDepth;

		switch (liquidDepth)
		{
			case 0:
				world.SetBlock(new Obsidian { Coordinates = new BlockCoordinates(coord) }, true, false);
				break;
			case <= 4:
				world.SetBlock(new Cobblestone { Coordinates = new BlockCoordinates(coord) }, true, false);
				break;
		}

		return true;

	}

	private bool IsWater(Level world, BlockCoordinates coord)
	{
		Block block = world.GetBlock(coord);
		return block is FlowingWater or Water;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [];
	}
}