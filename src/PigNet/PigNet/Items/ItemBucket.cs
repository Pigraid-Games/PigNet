using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemBucket
{
	public ItemBucket()
	{
		MaxStackSize = 16;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		// Pick up water/lava
		Block block = world.GetBlock(blockCoordinates);
		switch (block)
		{
			case Stationary fluid:
				if (fluid.LiquidDepth != 0) return true; // Only source blocks
				switch (block)
				{
					case Lava:
						player.Inventory.AddItem(new ItemLavaBucket(), true);
						break;
					case Water:
						player.Inventory.AddItem(new ItemWaterBucket(), true);
						break;
					default: return false;
				}

				world.SetAir(blockCoordinates);
				Count--;
				return true;
			case Flowing fluid:
				if (fluid.LiquidDepth != 0) return true; // Only source blocks
				switch (block)
				{
					case FlowingLava:
						player.Inventory.AddItem(new ItemLavaBucket(), true);
						break;
					case FlowingWater:
						player.Inventory.AddItem(new ItemWaterBucket(), true);
						break;
					default:
						return false;
				}

				world.SetAir(blockCoordinates);
				Count--;
				return true;
		}

		return false;
	}
}