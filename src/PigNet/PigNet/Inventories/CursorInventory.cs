using log4net;
using PigNet.Items;
using PigNet.Utils;

namespace PigNet.Inventories;

public class CursorInventory
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(CursorInventory));

	public ItemStacks Slots { get; } = ItemStacks.CreateAir((byte) UIInventorySlot.SlotsCount);

	public Item Cursor
	{
		get => Slots[0];
		set => Slots[0] = value;
	}

	public ItemStacks GetSlots()
	{
		return new ItemStacks(Slots);
	}

	public void Clear()
	{
		Slots.Reset();
	}
}