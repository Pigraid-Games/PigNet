
using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class QuartzOre : Block
{
	public QuartzOre()
	{
		BlastResistance = 15;
		Hardness = 3;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemQuartz()];
	}

	public override float GetExperiencePoints()
	{
		var random = new Random();
		return random.Next(2, 6);
	}
}