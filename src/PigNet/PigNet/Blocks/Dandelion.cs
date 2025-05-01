using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Dandelion
{
	public Dandelion()
	{
		IsSolid = false;
		IsTransparent = true;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		if (!base.CanPlace(world, player, blockCoordinates, targetCoordinates, face)) return false;
		Block under = world.GetBlock(Coordinates.BlockDown());
		return under is GrassBlock or Dirt;

	}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		if (Coordinates.BlockDown() != blockCoordinates) return;
		level.SetAir(Coordinates);
		UpdateBlocks(level);
	}
}