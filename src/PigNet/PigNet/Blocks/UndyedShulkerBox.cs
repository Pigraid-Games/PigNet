using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class UndyedShulkerBox : Block
{
	public UndyedShulkerBox()
	{
		IsTransparent = true;
		BlastResistance = 30f;
		Hardness = 6f;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		var shulkerBoxBlockEntity = new ShulkerBoxBlockEntity
		{
			Coordinates = Coordinates,
			Facing = (byte) face
		};

		world.SetBlockEntity(shulkerBoxBlockEntity);

		return false;
	}


	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		player.OpenInventory(blockCoordinates);

		return true;
	}
}