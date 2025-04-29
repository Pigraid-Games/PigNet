namespace PigNet.Blocks;

public partial class GlassPane : Block
{
	public GlassPane()
	{
		IsTransparent = true;
		BlastResistance = 1.5f;
		Hardness = 0.3f;
	}
}