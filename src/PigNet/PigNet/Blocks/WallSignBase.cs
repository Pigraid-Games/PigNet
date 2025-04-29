using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class WallSignBase : SignBase
{
	public abstract OldFacingDirection4 FacingDirection { get; set; }

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return world.GetBlock(blockCoordinates).IsReplaceable;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		FacingDirection = face;

		var blockEntity = new SignBlockEntity { Coordinates = Coordinates };
		world.SetBlockEntity(blockEntity);

		return base.PlaceBlock(world, player, targetCoordinates, face, faceCoords);
	}
}