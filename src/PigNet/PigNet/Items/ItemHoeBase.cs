using System.Numerics;
using log4net;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items.Tools;

public abstract class ItemHoeBase : Item
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemHoeBase));

	internal ItemHoeBase()
	{
		MaxStackSize = 1;
		ItemType = ItemType.Hoe;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block block = world.GetBlock(blockCoordinates);
		if (block is GrassBlock || block is Dirt normalDirt || block is GrassPath)
		{
			var farmland = new Farmland
			{
				Coordinates = blockCoordinates,
			};

			if (farmland.FindWater(world, blockCoordinates, [], 0))
			{
				Log.Warn("Found water source");
				farmland.MoisturizedAmount = 7;
			}

			world.SetBlock(farmland);
			player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);

			return true;
		}
		if (block is not CoarseDirt) return false;
		var dirt = new CoarseDirt { Coordinates = blockCoordinates };

		world.SetBlock(dirt);
		player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);

		return true;

	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockInteract:
			{
				if (block is not GrassBlock && block is not Dirt && block is not GrassPath) return false;
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			case ItemDamageReason.EntityAttack:
			{
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			default:
				return false;
		}
	}
}