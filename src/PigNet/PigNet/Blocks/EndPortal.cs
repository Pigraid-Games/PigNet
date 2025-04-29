namespace PigNet.Blocks;

public partial class EndPortal : Block
{
	public EndPortal()
	{
		IsSolid = false;
		BlastResistance = 18000000;
		Hardness = -1;
		LightLevel = 15;
		IsTransparent = true;
	}
}