namespace MiNET.Items;

public class ItemChorusFruit() : Item("minecraft:chorus_fruit", 432)
{
	public override Item GetSmelt()
	{
		return new ItemPoppedChorusFruit();
	}
}