using System.Numerics;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Coral : Block
{
	public Coral() : base(386)
	{
		BlastResistance = 0;
		Hardness = 0;
		IsFlammable = false;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Item itemInHand = player.Inventory.GetItemInHand();
		//blockName = ItemFactory.Translator.GetNameByMeta("minecraft:coral", itemInHand.Metadata);
		return false;
	}
}