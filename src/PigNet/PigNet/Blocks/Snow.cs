using PigNet.Items;
using PigNet.Items.Weapons;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Snow : Block
{
	public Snow()
	{
		BlastResistance = 1;
		Hardness = 0.2f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemSnowball { Count = 4 }];
	}
}