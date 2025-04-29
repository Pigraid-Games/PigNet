using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Stone : Block
{
	public Stone()
	{
		BlastResistance = 30;
		Hardness = 1.5f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return tool.ItemType != ItemType.PickAxe ? [] : new[] { ItemFactory.GetItem<Cobblestone>() };
	}
}