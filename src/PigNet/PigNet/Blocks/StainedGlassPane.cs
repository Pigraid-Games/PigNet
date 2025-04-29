using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class StainedGlassPaneBase : Block
{
	public StainedGlassPaneBase()
	{
		IsTransparent = true;
		BlastResistance = 1.5f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [];
	}
}