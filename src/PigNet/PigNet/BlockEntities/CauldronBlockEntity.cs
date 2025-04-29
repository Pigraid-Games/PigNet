using PigNet.Items;

namespace PigNet.BlockEntities;

public class CauldronBlockEntity() : BlockEntity(BlockEntityIds.Cauldron)
{
	public int? CustomColor { get; set; }
	public Item[] Items { get; set; } = [ new ItemAir() ];
	public short PotionId { get; set; } = -1;
	public short PotionType { get; set; } = -1;
}