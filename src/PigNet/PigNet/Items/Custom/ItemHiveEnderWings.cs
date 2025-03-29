namespace PigNet.Items.Custom;

public class ItemHiveEnderWings : ArmorChestplateBase
{
	public ItemHiveEnderWings() : base("hivebackbling:ender_wings", 1114)
	{
		MaxStackSize = 1;
		Durability = 1;
		ItemMaterial = ItemMaterial.Diamond;
		ItemType = ItemType.Chestplate;
	}
}