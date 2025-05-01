using PigNet.BlockEntities;

namespace PigNet.Blocks;

public abstract class BlastFurnaceBase : FurnaceBase
{
	protected override BlockEntity CreateBlockEntity()
	{
		return new BlastFurnaceBlockEntity { Coordinates = Coordinates };
	}
}