using System.Numerics;
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks;

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