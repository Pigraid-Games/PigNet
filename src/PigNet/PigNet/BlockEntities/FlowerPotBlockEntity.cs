using System.Collections.Generic;
using PigNet.Blocks;
using PigNet.Items;

namespace PigNet.BlockEntities;

public class FlowerPotBlockEntity() : BlockEntity(BlockEntityIds.FlowerPot)
{

	public Block PlantBlock { get; set; }

	public override List<Item> GetDrops()
	{
		return PlantBlock == null ? [] : [ItemFactory.GetItem(PlantBlock)];
	}
}