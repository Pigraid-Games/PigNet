using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class ChorusPlant
{
	public ChorusPlant()
	{
		IsTransparent = true;
		BlastResistance = 2;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		if (rnd.Next(2) > 0) return [new ItemChorusFruit()];
		return [];
	}
}