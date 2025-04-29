namespace PigNet.Blocks;

public partial class HeavyWeightedPressurePlate : Block
{
	public HeavyWeightedPressurePlate()
	{
		IsSolid = false;
		IsTransparent = true;
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}
}