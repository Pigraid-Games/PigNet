namespace MiNET.Items.Armor;

public class ItemDiamondHelmet : ArmorHelmetBase
{
	public ItemDiamondHelmet() : base("minecraft:diamond_helmet")
	{
		ItemType = ItemType.Helmet;
		ItemMaterial = ItemMaterial.Diamond;
		Durability = 363;
	}
}

public class ItemDiamondChestplate : ArmorChestplateBase
{
	public ItemDiamondChestplate() : base("minecraft:diamond_chestplate")
	{
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Diamond;
		Durability = 528;
	}
}

public class ItemDiamondLeggings : ArmorLeggingsBase
{
	public ItemDiamondLeggings() : base("minecraft:diamond_leggings")
	{
		ItemType = ItemType.Leggings;
		ItemMaterial = ItemMaterial.Diamond;
		Durability = 495;
	}
}

public class ItemDiamondBoots : ArmorBootsBase
{
	public ItemDiamondBoots() : base("minecraft:diamond_boots")
	{
		ItemType = ItemType.Boots;
		ItemMaterial = ItemMaterial.Diamond;
		Durability = 429;
	}
}