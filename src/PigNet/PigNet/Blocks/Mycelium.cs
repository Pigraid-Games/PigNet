using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Mycelium
{
	public Mycelium()
	{
		BlastResistance = 2.5f;
		Hardness = 0.6f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [ItemFactory.GetItem<Dirt>()];
	}
}