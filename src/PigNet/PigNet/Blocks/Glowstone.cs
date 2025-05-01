using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Glowstone
{
	public Glowstone()
	{
		LightLevel = 15;
		BlastResistance = 1.5f;
		Hardness = 0.3f;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		return [new ItemGlowstoneDust { Count = (byte) (2 + rnd.Next(2)) }];
	}
}