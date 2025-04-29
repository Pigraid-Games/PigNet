
namespace PigNet.Blocks;

public partial class Lava : Stationary
{
	public Lava() : base(BlockFactory.GetIdByType<FlowingLava>())
	{
		LightLevel = 15;
		BlastResistance = 500;
		Hardness = 100;
	}
}