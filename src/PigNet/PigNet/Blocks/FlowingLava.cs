namespace PigNet.Blocks;

public partial class FlowingLava : Flowing
{
	public FlowingLava() : base(BlockFactory.GetIdByType<Lava>())
	{
		LightLevel = 15;
		BlastResistance = 500;
		Hardness = 100;
	}
}