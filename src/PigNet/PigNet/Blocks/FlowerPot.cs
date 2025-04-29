using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class FlowerPot
{
	public FlowerPot()
	{
		IsTransparent = true;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		world.SetBlockEntity(new FlowerPotBlockEntity
		{
			Coordinates = Coordinates
		});

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		return true;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
	{
		var itemInHand = player.Inventory.GetItemInHand() as ItemBlock;
		var block = itemInHand?.Block;

		if (world.GetBlockEntity(Coordinates) is FlowerPotBlockEntity existingBlockEntity && existingBlockEntity.PlantBlock != null)
		{
			if (existingBlockEntity.PlantBlock.Id == block?.Id
				&& existingBlockEntity.PlantBlock.Data == block.Data)
			{
				return;
			}

			player.Inventory.SetFirstEmptySlot(ItemFactory.GetItem(existingBlockEntity.PlantBlock), true);

			UpdateBit = false;
			world.SetBlock(this);
		}
		else if(block != null)
		{
			UpdateBit = true;
			world.SetBlock(this);

			itemInHand.Count--;
			player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
		}

		world.SetBlockEntity(new FlowerPotBlockEntity
		{
			Coordinates = Coordinates,
			PlantBlock = block
		});
		return;
	}

	public override void BreakBlock(Level world, BlockFace face, bool silent = false)
	{
		base.BreakBlock(world, face, silent);

		world.RemoveBlockEntity(Coordinates);
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemFlowerPot()];
	}
}