namespace PigNet.Inventories;

public class InventoryClosedEventArgs : InventoryEventArgs
{
	public InventoryClosedEventArgs(Player player, IInventory inventory, bool closed) : base(player, inventory)
	{
		Closed = closed;
	}

	public bool Closed { get; }
}