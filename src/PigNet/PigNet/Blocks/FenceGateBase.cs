using PigNet.Blocks.States;

namespace PigNet.Blocks;

public abstract class FenceGateBase : Block
{
	public abstract OldDirection1 Direction { get; set; }
	public abstract bool InWallBit { get; set; }
	public abstract bool OpenBit { get; set; }

	public FenceGateBase()
	{
		FuelEfficiency = 15;
		IsTransparent = true;
		BlastResistance = 15;
		Hardness = 2;
		IsFlammable = true;
	}
}