namespace PigNet.Blocks;

public abstract class PressurePlateBase : Block
{
	protected PressurePlateBase()
	{
		IsTransparent = true;
		IsSolid = false;
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}
}