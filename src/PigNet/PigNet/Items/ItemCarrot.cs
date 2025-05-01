using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemCarrot() : FoodItemBase(3, 4.8)
{
	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return ItemFactory.GetItem<Carrots>().PlaceBlock(world, player, blockCoordinates, face, faceCoords);
	}
}