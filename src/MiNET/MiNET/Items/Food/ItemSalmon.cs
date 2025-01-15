namespace MiNET.Items.Food;

public class ItemSalmon() : FoodItem("minecraft:salmon", 0, 1, 0.6)
{
	public override Item GetSmelt()
	{
		return new ItemCookedSalmon();
	}
}