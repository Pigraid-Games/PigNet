using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class CoalOre
{
	public CoalOre()
	{
		BlastResistance = 15;
		Hardness = 3;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return tool.ItemMaterial < ItemMaterial.Wood ? [] : [new ItemCoal()];
	}

	public override float GetExperiencePoints()
	{
		var random = new Random();
		return random.Next(0, 3);
	}
}