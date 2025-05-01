using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemPotato() : FoodItemBase(1, 0.6)
{
	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return ItemFactory.GetItem<Potatoes>().PlaceBlock(world, player, targetCoordinates, face, faceCoords);
	}

	public Item GetSmelt()
	{
		return new ItemBakedPotato();
	}
}