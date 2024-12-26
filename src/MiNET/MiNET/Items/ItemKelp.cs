namespace MiNET.Items;

public class ItemKelp() : Item("minecraft:kelp", 335)
{
	public override Item GetSmelt()
	{
		return new ItemDriedKelp();
	}
}