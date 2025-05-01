namespace PigNet.Blocks;

public abstract class DoubleWoodenSlabBase : DoubleSlabBase
{
	public DoubleWoodenSlabBase()
	{
		BlastResistance = 15;
		Hardness = 2;
		IsFlammable = true;
	}
}