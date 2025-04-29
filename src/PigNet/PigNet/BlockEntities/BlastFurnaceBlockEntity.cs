using PigNet.Blocks;

namespace PigNet.BlockEntities;

public class BlastFurnaceBlockEntity : FurnaceBlockEntityBase<BlastFurnace, LitBlastFurnace>
{
	public BlastFurnaceBlockEntity() : base(BlockEntityIds.BlastFurnace, 100)
	{
	}
}