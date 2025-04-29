namespace PigNet.Blocks;

public partial class DetectorRail : Block
{
	public DetectorRail()
	{
		IsSolid = false;
		IsTransparent = true;
		BlastResistance = 3.5f;
		Hardness = 0.7f;
	}
}