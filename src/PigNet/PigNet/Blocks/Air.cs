using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Air : Block
{
	public Air()
	{
		IsReplaceable = true;
		IsSolid = false;
		IsBuildable = false;
		IsTransparent = true;
		IsBlockingSkylight = false;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [];
	}
}