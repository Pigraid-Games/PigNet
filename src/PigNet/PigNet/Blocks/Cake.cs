using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Cake
{
	public Cake()
	{
		IsTransparent = true;
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		if (BiteCounter == 0) return [new ItemCake()];
		return [];
	}
}