namespace PigNet.Blocks;

public abstract class CarpetBase : Block
{
	public CarpetBase()
	{
		IsTransparent = true;
		BlastResistance = 0.5f;
		Hardness = 0.1f;
		IsFlammable = true;
	}
}