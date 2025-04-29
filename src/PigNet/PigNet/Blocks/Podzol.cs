using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Podzol : Block
{
	public Podzol()
	{
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return new[] { ItemFactory.GetItem<Dirt>() };
	}
}