namespace PigNet.Blocks;

public abstract class InfestedBlockBase : Block
{
	public InfestedBlockBase()
	{
		BlastResistance = 3.75f;
		Hardness = 0.75f;

		// TODO: Spawns silverfish on break.	
	}
}