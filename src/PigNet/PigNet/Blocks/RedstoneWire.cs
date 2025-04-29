using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class RedstoneWire : Block
{
	public RedstoneWire()
	{
		IsTransparent = true;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemRedstone()];
	}
}