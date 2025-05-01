using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class DoubleSlabBase : SlabBase
{
	public override Item GetItem(Level world, bool blockItem = false)
	{
		ItemBlock item = ItemFactory.GetItem<ItemBlock>(DoubleSlabToSlabMap[Id]);
		item.Block.SetStates(this);
		return item;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		Item item = GetItem(world);
		if (item == null) return [];
		item.Count = 2;
		return [item]; 
	}
}