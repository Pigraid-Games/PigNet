namespace MiNET.Items.Tools;

public class ItemWoodenAxe : ItemAxe
{
	public ItemWoodenAxe() : base("minecraft:wooden_axe")
	{
		ItemMaterial = ItemMaterial.Wood;
		FuelEfficiency = 10;
	}
}

public class ItemWoodenHoe : ItemHoe
{
	public ItemWoodenHoe() : base("minecraft:wooden_hoe")
	{
		ItemMaterial = ItemMaterial.Wood;
		FuelEfficiency = 10;
	}
}

public class ItemWoodenPickaxe : ItemPickaxe
{
	public ItemWoodenPickaxe() : base("minecraft:wooden_pickaxe")
	{
		ItemMaterial = ItemMaterial.Wood;
		FuelEfficiency = 10;
	}
}

public class ItemWoodenShovel : ItemShovel
{
	public ItemWoodenShovel() : base("minecraft:wooden_shovel")
	{
		ItemMaterial = ItemMaterial.Wood;
		FuelEfficiency = 10;
	}
}