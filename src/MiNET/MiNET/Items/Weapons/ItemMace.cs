namespace MiNET.Items.Weapons;

public class ItemMace : ItemSword
{
	public ItemMace() : base("minecraft:mace", 1047, canInteract: false)
	{
		ItemMaterial = ItemMaterial.Netherite;
	}
}