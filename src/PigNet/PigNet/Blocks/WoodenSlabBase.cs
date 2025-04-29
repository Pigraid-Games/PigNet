namespace PigNet.Blocks;

public abstract class WoodenSlabBase : SlabBase
{
	public WoodenSlabBase()
	{
		BlastResistance = 15;
		IsFlammable = true;
	}

	protected override bool AreSameType(Block obj)
	{
		if (!base.AreSameType(obj)) return false;
		return obj is WoodenSlabBase;
	}
}