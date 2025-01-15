namespace MiNET.Items.Armor;

public class ItemIronHelmet : ArmorHelmetBase
{
	public ItemIronHelmet() : base("minecraft:iron_helmet")
	{
		ItemType = ItemType.Helmet;
		ItemMaterial = ItemMaterial.Iron;
		Durability = 165;
	}
}

public class ItemIronChestplate : ArmorChestplateBase
{
	public ItemIronChestplate() : base("minecraft:iron_chestplate")
	{
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Iron;
		Durability = 240;
	}
}

public class ItemIronLeggings : ArmorLeggingsBase
{
	public ItemIronLeggings() : base("minecraft:iron_leggings")
	{
		ItemType = ItemType.Leggings;
		ItemMaterial = ItemMaterial.Iron;
		Durability = 225;
	}
}

public class ItemIronBoots : ArmorBootsBase
{
	public ItemIronBoots() : base("minecraft:iron_boots")
	{
		ItemType = ItemType.Boots;
		ItemMaterial = ItemMaterial.Iron;
		Durability = 195;
	}
}