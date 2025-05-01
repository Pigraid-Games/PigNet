using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class EnchantingTable
{
	public EnchantingTable()
	{
		FuelEfficiency = 15;
		IsTransparent = true;
		BlastResistance = 6000;
		Hardness = 5;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		var tableBlockEntity = new EnchantingTableBlockEntity {Coordinates = Coordinates};

		world.SetBlockEntity(tableBlockEntity);

		return false;
	}


	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		player.OpenInventory(blockCoordinates);

		return true;
	}
}