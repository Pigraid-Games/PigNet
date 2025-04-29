using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class GrassPath : Block
{
	public GrassPath()
	{
		BlastResistance = 3.25f;
		Hardness = 0.6f;
		IsTransparent = true;
		IsBlockingSkylight = false;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return new[] { ItemFactory.GetItem<Dirt>() };
	}
}