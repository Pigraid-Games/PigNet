using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class PlanksBase : Block
{
	public PlanksBase()
	{
		FuelEfficiency = 15;
		BlastResistance = 15;
		Hardness = 2;
		IsFlammable = true;
	}

	public override Item GetItem(Level world, bool blockItem = false)
	{
		return ItemFactory.GetItem(this);
	}
}