using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class FurnaceBase : Block
{
	public abstract CardinalDirection CardinalDirection { get; set; }

	protected FurnaceBase()
	{
		BlastResistance = 17.5f;
		Hardness = 3.5f;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		CardinalDirection = player.KnownPosition.GetDirection();

		var blockEntity = CreateBlockEntity();
		world.SetBlockEntity(blockEntity);

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		player.OpenInventory(blockCoordinates);

		return true;
	}

	protected virtual BlockEntity CreateBlockEntity()
	{
		return new FurnaceBlockEntity { Coordinates = Coordinates };
	}
}