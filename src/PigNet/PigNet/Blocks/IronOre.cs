using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class IronOre
{
	public IronOre()
	{
		BlastResistance = 15;
		Hardness = 3;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return tool.ItemMaterial < ItemMaterial.Stone ? [] : [new ItemRawIron()];
	}
}