namespace MiNET.Items.Armor;

public class ItemLeatherHelmet : ArmorHelmetBase
{
	public ItemLeatherHelmet() : base("minecraft:leather_helmet")
	{
		ItemType = ItemType.Helmet;
		ItemMaterial = ItemMaterial.Leather;
		Durability = 55;
	}
}

public class ItemLeatherChestplate : ArmorChestplateBase
{
	public ItemLeatherChestplate() : base("minecraft:leather_chestplate")
	{
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Leather;
		Durability = 80;
	}
}

public class ItemLeatherLeggings : ArmorLeggingsBase
{
	public ItemLeatherLeggings() : base("minecraft:leather_leggings")
	{
		ItemType = ItemType.Leggings;
		ItemMaterial = ItemMaterial.Leather;
		Durability = 75;
	}
}

public class ItemLeatherBoots : ArmorBootsBase
{
	public ItemLeatherBoots() : base("minecraft:leather_boots")
	{
		ItemType = ItemType.Boots;
		ItemMaterial = ItemMaterial.Leather;
		Durability = 65;
	}
}