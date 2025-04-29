using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class RedMushroomBlock : Block
{
	public RedMushroomBlock()
	{
		BlastResistance = 1;
		Hardness = 0.2f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		int next = rnd.Next(3);
		if (next > 0) return new[] { ItemFactory.GetItem<RedMushroom>(count: next) };
		return [];
	}
}