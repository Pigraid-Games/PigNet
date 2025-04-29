using PigNet.Blocks.States;

namespace PigNet.Blocks;

public abstract class CoralWallFanBase : Block
{
	public abstract CoralDirection CoralDirection { get; set; }
}