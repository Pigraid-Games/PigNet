using PigNet.Blocks.States;

namespace PigNet.Blocks;

public abstract class CoralFanBase : Block
{
	public abstract CoralFanDirection CoralFanDirection { get; set; }
}