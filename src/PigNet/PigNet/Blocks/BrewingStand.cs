using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class BrewingStand
{
	public BrewingStand()
	{
		IsTransparent = true;
		LightLevel = 1;
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemBrewingStand()];
	}
}