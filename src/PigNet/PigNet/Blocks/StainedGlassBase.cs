namespace PigNet.Blocks;

public abstract class StainedGlassBase : Glass
{
	public StainedGlassBase()
	{
		IsTransparent = true;
		BlastResistance = 1.5f;
		Hardness = 0.3f;
		IsBlockingSkylight = false;
	}
}