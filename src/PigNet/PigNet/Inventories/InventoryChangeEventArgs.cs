using PigNet.Items;

namespace PigNet.Inventories;

public class InventoryChangeEventArgs : InventoryEventArgs
{
	public InventoryChangeEventArgs(Player player, IInventory inventory, byte slot, Item item) : base(player, inventory)
	{
		Slot = slot;
		Item = item;
	}

	public byte Slot { get; }

	public Item Item { get; }
}