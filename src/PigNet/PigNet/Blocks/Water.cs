namespace PigNet.Blocks;

public partial class Water : Stationary
{
	public Water() : base(BlockFactory.GetIdByType<FlowingWater>())
	{
	}
}