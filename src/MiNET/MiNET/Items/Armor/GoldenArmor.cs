namespace MiNET.Items.Armor;

public class ItemGoldenHelmet : ArmorHelmetBase
{
	public ItemGoldenHelmet() : base("minecraft:golden_helmet")
	{
		ItemType = ItemType.Helmet;
		ItemMaterial = ItemMaterial.Gold;
		Durability = 77;
	}
}

public class ItemGoldenChestplate : ArmorChestplateBase
{
	public ItemGoldenChestplate() : base("minecraft:golden_chestplate")
	{
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Gold;
		Durability = 112;
	}
}

public class ItemGoldenLeggings : ArmorLeggingsBase
{
	public ItemGoldenLeggings() : base("minecraft:golden_leggings")
	{
		ItemType = ItemType.Leggings;
		ItemMaterial = ItemMaterial.Gold;
		Durability = 105;
	}
}

public class ItemGoldenBoots : ArmorBootsBase
{
	public ItemGoldenBoots() : base("minecraft:golden_boots")
	{
		ItemType = ItemType.Boots;
		ItemMaterial = ItemMaterial.Gold;
		Durability = 91;
	}
}