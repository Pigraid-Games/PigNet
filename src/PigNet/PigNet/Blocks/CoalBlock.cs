namespace PigNet.Blocks;

public partial class CoalBlock : Block
{
	public CoalBlock()
	{
		FuelEfficiency = 800;
		BlastResistance = 30;
		Hardness = 5;
		IsFlammable = true;
	}
}