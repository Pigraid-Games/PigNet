using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class Stationary : Block
{
	public abstract int LiquidDepth { get; set; }

	public string FlowingId { get; }

	internal Stationary(string flowingId)
	{
		FlowingId = flowingId;

		IsSolid = false;
		IsBuildable = false;
		IsReplaceable = true;
		IsTransparent = true;
	}

	public override void DoPhysics(Level level)
	{
		CheckForHarden(level, Coordinates);
		if (level.GetBlock(Coordinates).Id == Id) SetToFlowing(level);
	}

	private void SetToFlowing(Level world)
	{
		var flowingBlock = (Flowing) BlockFactory.GetBlockById(FlowingId);
		flowingBlock.LiquidDepth = LiquidDepth;
		flowingBlock.Coordinates = Coordinates;
		world.SetBlock(flowingBlock, applyPhysics: false);
		world.ScheduleBlockTick(flowingBlock, 5);
	}

	private void CheckForHarden(Level world, BlockCoordinates coord)
	{
		var block = world.GetBlock(coord) as Stationary; // this

		bool harden = false;
		if (block is not Lava) return;
		if (IsWater(world, coord + BlockCoordinates.Backwards)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Forwards)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Left)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Right)) harden = true;
		if (harden || IsWater(world, coord + BlockCoordinates.Up)) harden = true;
		if (!harden) return;
			
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
	}

	private bool IsWater(Level world, BlockCoordinates coord)
	{
		Block block = world.GetBlock(coord);
		return block is FlowingWater or Water;
	}
}