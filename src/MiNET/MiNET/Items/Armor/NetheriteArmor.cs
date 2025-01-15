namespace MiNET.Items.Armor;

public class ItemNetheriteHelmet : ArmorHelmetBase
{
	public ItemNetheriteHelmet() : base("minecraft:netherite_helmet")
	{
		ItemType = ItemType.Helmet;
		ItemMaterial = ItemMaterial.Netherite;
		Durability = 407;
	}
}

public class ItemNetheriteChestplate : ArmorChestplateBase
{
	public ItemNetheriteChestplate() : base("minecraft:netherite_chestplate")
	{
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Netherite;
		Durability = 592;
	}
}

public class ItemNetheriteLeggings : ArmorLeggingsBase
{
	public ItemNetheriteLeggings() : base("minecraft:netherite_leggings")
	{
		ItemType = ItemType.Leggings;
		ItemMaterial = ItemMaterial.Netherite;
		Durability = 555;
	}
}

public class ItemNetheriteBoots : ArmorBootsBase
{
	public ItemNetheriteBoots() : base("minecraft:netherite_boots")
	{
		ItemType = ItemType.Boots;
		ItemMaterial = ItemMaterial.Netherite;
		Durability = 481;
	}
}