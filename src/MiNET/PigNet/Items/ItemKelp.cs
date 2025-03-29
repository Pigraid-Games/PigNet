using PigNet.Items.Food;

namespace PigNet.Items;

public class ItemKelp() : Item("minecraft:kelp", 335, canInteract: false)
{
	public override Item GetSmelt()
	{
		return new ItemDriedKelp();
	}
}