using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class LapisOre
{
	public LapisOre()
	{
		BlastResistance = 15;
		Hardness = 3;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		if (tool.ItemMaterial < ItemMaterial.Stone) return [];

		// Random between 4-8
		var rnd = new Random();
		int plus = rnd.Next(4);
		return [new ItemLapisLazuli { Count = (byte) (4 + plus) }];
	}

	public override float GetExperiencePoints()
	{
		var random = new Random();
		return random.Next(2, 6);
	}
}