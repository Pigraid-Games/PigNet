using System.Collections.Generic;

namespace PigNet.Inventories;

public class CreativeInventoryCategory
{
	public CreativeInventoryCategoryType Type { get; set; }

	public List<CreativeInventoryGroup> Groups { get; set; } = [];
}