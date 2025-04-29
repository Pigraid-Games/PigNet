namespace PigNet.Blocks;

public abstract class WoolBase : Block
{
	public WoolBase()
	{
		BlastResistance = 4;
		Hardness = 0.8f;
		IsFlammable = true;
	}
}