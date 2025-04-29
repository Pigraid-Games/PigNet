using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class SeaLantern : Block
{
	public SeaLantern()
	{
		LightLevel = 15;
		BlastResistance = 1.5f;
		Hardness = 0.3f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		return [new ItemPrismarineShard { Count = (byte) (rnd.Next(2, 3)) }];
	}
}