using System.Numerics;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items.Tools;

public abstract class ItemShovelBase : Item
{
	internal ItemShovelBase()
	{
		MaxStackSize = 1;
		ItemType = ItemType.Shovel;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block block = world.GetBlock(blockCoordinates);
		if (block is not GrassBlock) return false;
		var grassPath = new GrassPath
		{
			Coordinates = blockCoordinates,
		};
		world.SetBlock(grassPath);
		player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
		return true;

	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockBreak:
			{
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			case ItemDamageReason.BlockInteract:
			{
				if (block is not GrassBlock) return false;
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			case ItemDamageReason.EntityAttack:
			{
				Metadata += 2;
				return Metadata >= GetMaxUses() - 1;
			}
			default:
				return false;
		}
	}
}