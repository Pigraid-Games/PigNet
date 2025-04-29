namespace PigNet.Blocks;

public abstract class FenceBase : Block
{
	public FenceBase()
	{
		FuelEfficiency = 15;
		IsTransparent = true;
		BlastResistance = 15;
		Hardness = 2;
		IsFlammable = true;
	}
}