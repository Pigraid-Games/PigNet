using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Wheat : Crops
{
	public override Item[] GetDrops(Level world, Item tool)
	{
		if (Growth != 7) return [new ItemWheatSeeds()];
		var rnd = new Random();
		int count = rnd.Next(4);
		return count > 0 ? [new ItemWheat(), new ItemWheatSeeds { Count = (byte) count }] : [new ItemWheat()];
	}
}