using PigNet.Blocks;

namespace PigNet.BlockEntities;

public class FurnaceBlockEntity : FurnaceBlockEntityBase<Furnace, LitFurnace>
{
	public FurnaceBlockEntity() : base(BlockEntityIds.Furnace, 200)
	{
	}
}