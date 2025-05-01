using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class EnderChest
{
	public EnderChest()
	{
		IsTransparent = true;
		LightLevel = 7;
		BlastResistance = 3000;
		Hardness = 22.5f;
		FuelEfficiency = 0;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [ItemFactory.GetItem<Obsidian>(count: 8)];
	}
}