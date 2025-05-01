using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class DarkOakLeaves
{
	public override Item[] GetDrops(Level world, Item tool)
	{
		return new Random().Next(200) == 0 ? [new ItemApple()] : base.GetDrops(world, tool);
	}
}