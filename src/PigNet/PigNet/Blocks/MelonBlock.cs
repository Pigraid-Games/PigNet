using System;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class MelonBlock
{
	public MelonBlock()
	{
		Hardness = 1;
		IsTransparent = true;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		return [new ItemMelonSlice { Count = (byte) (3 + rnd.Next(5)) }];
	}
}