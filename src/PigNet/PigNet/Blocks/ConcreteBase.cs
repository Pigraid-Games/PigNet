using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class ConcreteBase : Block
{
	public ConcreteBase()
	{
		BlastResistance = 15;
		Hardness = 3;
	}

	public override Item GetItem(Level world, bool blockItem = false)
	{
		return ItemFactory.GetItem(this);
	}
}