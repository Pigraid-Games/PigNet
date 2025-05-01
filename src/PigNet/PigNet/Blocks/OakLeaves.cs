using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class OakLeaves
{
	public override Item[] GetDrops(Level world, Item tool)
	{
		if (new Random().Next(200) == 0) return [new ItemApple()];
		return base.GetDrops(world, tool);
	}
}