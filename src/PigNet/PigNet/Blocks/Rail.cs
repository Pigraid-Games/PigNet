using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Rail
{
	public Rail()
	{
		IsTransparent = true;
		IsSolid = false;
		BlastResistance = 3.5f;
		Hardness = 0.7f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		// No special metadata
		return [ItemFactory.GetItem<Rail>()];
	}
}