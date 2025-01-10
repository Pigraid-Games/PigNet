namespace MiNET.Items;

public class ItemChorusFruit() : Item("minecraft:chorus_fruit", 432, canInteract: false)
{
	public override Item GetSmelt()
	{
		return new ItemPoppedChorusFruit();
	}
}