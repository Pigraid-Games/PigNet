
using System;

namespace PigNet.Net.EnumerationsTable;

public enum InventoryLayout
{
	None = 0,
	Survival = 1,
	RecipeBook = 2,
	Creative = 3,
	Count = 4
}

public enum InventoryLeftTabIndex
{
	None = 0,
	RecipeConstruction = 1,
	RecipeEquipment = 2,
	RecipeItems = 3,
	RecipeNature = 4,
	RecipeSearch = 5,
	Survival = 6,
	Count = 7
}

public enum InventoryRightTabIndex
{
	None = 0,
	FullScreen = 1,
	Crafting = 2,
	Armor = 3,
	Count = 4
}

[Flags]
public enum InventorySourceFlags
{
	NoFlag = 0,
	WorldInteraction_Random = 1
}

public enum InventorySourceType : uint
{
	InvalidInventory = uint.MaxValue,
	ContainerInventory = 0,
	GlobalInventory = 1,
	WorldInteraction = 2,
	CreativeInventory = 3,
	NonImplementedFeatureTODO = 99999
}