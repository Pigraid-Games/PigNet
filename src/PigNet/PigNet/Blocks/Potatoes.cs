using System;
using PigNet.Items;
using PigNet.Items.Food;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Potatoes : Crops
{
	public override Item[] GetDrops(Level world, Item tool)
	{
		if (Growth != 7) return [new ItemPotato()];
		var random = new Random();
		return [new ItemPotato { Count = (byte) random.Next(1, 5) }];

	}
}