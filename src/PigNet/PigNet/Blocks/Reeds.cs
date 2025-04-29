using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Reeds
{
	public Reeds()
	{
		IsSolid = false;
		IsTransparent = true;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return CanGrowOn(world, blockCoordinates) && world.GetBlock(blockCoordinates).IsReplaceable;
	}

	public override void DoPhysics(Level level)
	{
		level.ScheduleBlockTick(this, 1);
	}

	public override void OnTick(Level level, bool isRandom)
	{
		if (!CanGrowOn(level, Coordinates)) level.BreakBlock(this);
	}

	public override Item GetItem(Level world, bool blockItem = false)
	{
		return blockItem ? base.GetItem(world, blockItem) : new ItemSugarCane();
	}

	private bool CanGrowOn(Level world, BlockCoordinates blockCoordinates)
	{
		Block currentBlock = world.GetBlock(blockCoordinates);

		if (currentBlock is Stationary or Flowing)
			return false;

		BlockCoordinates targetCoordinates = blockCoordinates.BlockDown();
		Block targetBlock = world.GetBlock(targetCoordinates);

		if (targetBlock is Reeds) return true;

		if (targetBlock is not Sand
			&& targetBlock is not Dirt
			&& targetBlock is not DirtWithRoots
			&& targetBlock is not Mycelium
			&& targetBlock is not GrassBlock
			&& targetBlock is not Podzol)
			return false;

		foreach (var aroundCoords in targetCoordinates.Get2dAroundCoordinates())
		{
			Block aroundBlock = world.GetBlock(aroundCoords);
			if (aroundBlock is Water or FlowingWater) return true;
		}

		return false;
	}
}