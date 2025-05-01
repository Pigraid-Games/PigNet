using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class BrownMushroomBlock
{
	public BrownMushroomBlock()
	{
		BlastResistance = 1;
		Hardness = 0.2f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		int next = rnd.Next(3);
		if (next > 0) return [ItemFactory.GetItem<BrownMushroom>()];
		return [];
	}
}