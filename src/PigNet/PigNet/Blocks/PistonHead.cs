using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class PistonArmCollision
{
	public PistonArmCollision()
	{
		BlastResistance = 2.5f;
		IsTransparent = true;
		// runtime id: 1580 0x62C, data: 0
		// runtime id: 2117 0x845, data: 1
		// runtime id: 71 0x47, data: 2
		// runtime id: 1499 0x5DB, data: 3
		// runtime id: 2605 0xA2D, data: 4
		// runtime id: 124 0x7C, data: 5
		// runtime id: 580 0x244, data: 6
		// runtime id: 175 0xAF, data: 7
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [];
	}
}