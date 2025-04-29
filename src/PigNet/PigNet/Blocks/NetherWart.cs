using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class NetherWart : Block
{
	public NetherWart()
	{
		IsTransparent = true;
		IsSolid = false;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		if (Age != 3) return [new ItemNetherWart()];
		var rnd = new Random();
		return [new ItemNetherWart { Count = (byte) (2 + rnd.Next(3)) }];

	}
}