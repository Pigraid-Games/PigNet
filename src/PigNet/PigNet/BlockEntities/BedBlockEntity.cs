using fNbt.Serialization;

namespace PigNet.BlockEntities;

public class BedBlockEntity() : BlockEntity(BlockEntityIds.Bed)
{
	[NbtProperty("color")]
	public byte Color { get; set; } = 0;
}