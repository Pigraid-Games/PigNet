namespace PigNet.Items.Weapons;

public class ItemMace : ItemSword
{
	public ItemMace() : base("minecraft:mace", 1047, false)
	{
		ItemMaterial = ItemMaterial.Netherite;
	}
}