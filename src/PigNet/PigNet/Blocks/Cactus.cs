using PigNet.Blocks.States;
using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Cactus : Block
{
	public Cactus()
	{
		IsTransparent = true;
		BlastResistance = 2;
		Hardness = 0.4f;
	}
}