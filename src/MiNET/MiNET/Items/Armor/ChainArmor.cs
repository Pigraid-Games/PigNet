namespace MiNET.Items.Armor;

public class ItemChainmailHelmet : ArmorHelmetBase
{
	public ItemChainmailHelmet() : base("minecraft:chainmail_helmet")
	{
		ItemType = ItemType.Helmet;
		ItemMaterial = ItemMaterial.Chain;
		Durability = 165;
	}
}

public class ItemChainmailChestplate : ArmorChestplateBase
{
	public ItemChainmailChestplate() : base("minecraft:chainmail_chestplate")
	{
		ItemType = ItemType.Chestplate;
		ItemMaterial = ItemMaterial.Chain;
		Durability = 225;
	}
}

public class ItemChainmailLeggings : ArmorLeggingsBase
{
	public ItemChainmailLeggings() : base("minecraft:chainmail_leggings")
	{
		ItemType = ItemType.Leggings;
		ItemMaterial = ItemMaterial.Chain;
		Durability = 225;
	}
}

public class ItemChainmailBoots : ArmorBootsBase
{
	public ItemChainmailBoots() : base("minecraft:chainmail_boots")
	{
		ItemType = ItemType.Boots;
		ItemMaterial = ItemMaterial.Chain;
		Durability = 195;
	}
}