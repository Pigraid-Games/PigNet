using MiNET.Items.Food;

namespace MiNET.Items;

public class ItemKelp() : Item("minecraft:kelp", canInteract: false)
{
	public override Item GetSmelt()
	{
		return new ItemDriedKelp();
	}
}