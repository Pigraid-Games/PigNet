using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class StandingSignBase : SignBase
{
	public abstract int GroundSignDirection { get; set; }

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return world.GetBlock(blockCoordinates).IsReplaceable;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		GroundSignDirection = player.KnownPosition.GetOppositeDirection16();

		var blockEntity = new SignBlockEntity { Coordinates = Coordinates };
		world.SetBlockEntity(blockEntity);

		return base.PlaceBlock(world, player, targetCoordinates, face, faceCoords);
	}


	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		return true;
	}
}