using PigNet.Blocks.States;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class LitFurnace : FurnaceBase
{
	public LitFurnace()
	{
		LightLevel = 13;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [ItemFactory.GetItem<Furnace>()];
	}
}