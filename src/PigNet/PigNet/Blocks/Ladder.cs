using System.Numerics;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Ladder : Block
{
	public Ladder()
	{
		IsTransparent = true;
		BlastResistance = 2;
		Hardness = 0.4f;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return base.CanPlace(world, player, blockCoordinates, targetCoordinates, face)
				&& !world.GetBlock(targetCoordinates).IsTransparent 
				&& face != BlockFace.Down 
				&& face != BlockFace.Up;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		FacingDirection = face;

		return false;
	}
}