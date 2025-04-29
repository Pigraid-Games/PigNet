
using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class RedstoneOre : Block
{
	public RedstoneOre()
	{
		BlastResistance = 15;
		Hardness = 3;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		if (tool.ItemMaterial < ItemMaterial.Iron) return [];

		var rnd = new Random();
		return [new ItemRedstone { Count = (byte) (4 + rnd.Next(1)) }];
	}

	public override float GetExperiencePoints()
	{
		var random = new Random();
		return random.Next(1, 6);
	}
}