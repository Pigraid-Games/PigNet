using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Web : Block
{
	public Web()
	{
		IsSolid = false;
		IsTransparent = true;
		BlastResistance = 20;
		Hardness = 4;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		if (tool is ItemShears) return new[] { ItemFactory.GetItem<Web>() };
		return tool.ItemType == ItemType.Sword ? [new ItemString()] : [];
	}
}