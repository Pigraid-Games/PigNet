namespace MiNET.Items.Food;

public class ItemCod() : FoodItem("minecraft:cod", 349, 0, 1, 0.6)
{
	public override Item GetSmelt()
	{
		return new ItemCookedCod();
	}
}