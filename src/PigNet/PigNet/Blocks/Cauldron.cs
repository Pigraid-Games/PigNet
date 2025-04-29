using System.Numerics;
using log4net;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Cauldron
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Cauldron));

	public Cauldron()
	{
		IsTransparent = true;
		BlastResistance = 10;
		Hardness = 2;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		Item itemInHand = player.Inventory.GetItemInHand();

		if (itemInHand is not ItemBucket) return true; // Handled
		switch (itemInHand.Metadata)
		{
			case 8 when FillLevel >= 8:
				return true; // Handled
			case 8:
				FillLevel = 8;
				world.SetBlock(this, applyPhysics: false);
				itemInHand.Metadata = 0;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
				break;
			case 0 when FillLevel <= 0:
				return true; // Handled
			case 0:
				FillLevel = 0;
				world.SetBlock(this, applyPhysics: false);
				itemInHand.Metadata = 8;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
				break;
		}

		return true; // Handled
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemCauldron()];
	}
}