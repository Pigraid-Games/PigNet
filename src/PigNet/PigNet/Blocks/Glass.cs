namespace PigNet.Blocks;

public partial class Glass : Block
{
	public Glass()
	{
		IsTransparent = true;
		BlastResistance = 1.5f;
		Hardness = 0.3f;
		IsBlockingSkylight = false;
	}
}