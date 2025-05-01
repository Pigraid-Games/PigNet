using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class TripWire
{
	public TripWire()
	{
		IsTransparent = true;
		IsSolid = false;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return new[] { new ItemString() };
	}
}