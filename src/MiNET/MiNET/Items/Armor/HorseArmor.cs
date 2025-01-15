namespace MiNET.Items.Armor;

public class ItemLeatherHorseArmor : Item
{
	public ItemLeatherHorseArmor() : base("minecraft:leather_horse_armor")
	{
		MaxStackSize = 1;
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Leather;
	}
}

public class ItemIronHorseArmor : Item
{
	public ItemIronHorseArmor() : base("minecraft:iron_horse_armor")
	{
		MaxStackSize = 1;
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Iron;
	}
}

public class ItemGoldenHorseArmor : Item
{
	public ItemGoldenHorseArmor() : base("minecraft:golden_horse_armor")
	{
		MaxStackSize = 1;
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Gold;
	}
}

public class ItemDiamondHorseArmor : Item
{
	public ItemDiamondHorseArmor() : base("minecraft:diamond_horse_armor")
	{
		MaxStackSize = 1;
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Diamond;
	}
}