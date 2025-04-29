namespace PigNet.Blocks;

public partial class ActivatorRail : Block
{
	public ActivatorRail()
	{
		IsSolid = false;
		IsTransparent = true;
		BlastResistance = 3.5f;
		Hardness = 0.7f;
	}
}