using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Fire
{
	public Fire()
	{
		IsReplaceable = true;
		IsTransparent = true;
		LightLevel = 15;
		IsSolid = false;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [];
	}
}