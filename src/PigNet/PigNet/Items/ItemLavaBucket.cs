using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemLavaBucket
{
	public ItemLavaBucket()
	{
		MaxStackSize = 1;
		FuelEfficiency = 1000;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (!ItemFactory.GetItem<Lava>().PlaceBlock(world, player, blockCoordinates, face, faceCoords)) return false;
		player.Inventory.AddItem(new ItemBucket(), true);
		return true;

	}
}