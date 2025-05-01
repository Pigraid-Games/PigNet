using PigNet.Items;
using PigNet.Utils;

namespace PigNet;

public class CursorInventory
{
	public ItemStacks Slots { get; } = ItemStacks.CreateAir((byte) UIInventorySlot.SlotsCount);

	public Item Cursor
	{
		get => Slots[0];
		set => Slots[0] = value;
	}

	public CursorInventory()
	{

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