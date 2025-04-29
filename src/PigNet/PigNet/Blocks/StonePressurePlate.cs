namespace PigNet.Blocks;

public partial class StonePressurePlate : Block
{
	public StonePressurePlate()
	{
		IsTransparent = true;
		IsSolid = false;
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}
}