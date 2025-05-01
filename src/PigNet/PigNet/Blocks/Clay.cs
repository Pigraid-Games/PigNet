using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Clay
{
	public Clay()
	{
		BlastResistance = 3;
		Hardness = 0.6f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemClayBall { Count = 4 }];
	}
}