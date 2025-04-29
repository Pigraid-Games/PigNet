
using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class DiamondOre : Block
{
	public DiamondOre()
	{
		BlastResistance = 15;
		Hardness = 3;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return tool.ItemMaterial < ItemMaterial.Iron ? [] : [new ItemDiamond()];
	}

	public override float GetExperiencePoints()
	{
		var random = new Random();
		return random.Next(3, 8);
	}
}