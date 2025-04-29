using fNbt.Serialization;

namespace PigNet.BlockEntities;

public class EnchantingTableBlockEntity() : ContainerBlockEntityBase(BlockEntityIds.EnchantTable, 2)
{
	[NbtProperty("rott")]
	public float BookRotation { get; set; }
}