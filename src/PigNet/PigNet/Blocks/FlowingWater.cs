namespace PigNet.Blocks;

public partial class FlowingWater : Flowing
{
	public FlowingWater() : base(BlockFactory.GetIdByType<Water>())
	{
		BlastResistance = 500;
		Hardness = 100;
	}
}