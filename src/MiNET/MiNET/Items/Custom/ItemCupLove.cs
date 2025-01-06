using System;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items.Custom;

public class ItemCupLove : ArmorChestplateBase
{
	public ItemCupLove() : base("pigraid:cuplove", 1113)
	{
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Diamond;
		MaxStackSize = 1;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		byte slot = (byte) player.Inventory.Slots.IndexOf(this);
		player.Inventory.SetInventorySlot(slot, player.Inventory.Chest);

		UniqueId = Environment.TickCount;
		player.Inventory.Chest = this;
		player.SendArmorForPlayer();
	}
}