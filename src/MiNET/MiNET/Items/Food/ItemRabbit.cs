namespace MiNET.Items.Food;

public class ItemRabbit() : FoodItem("minecraft:rabbit", 0, 1, 0.6)
{
	public override Item GetSmelt()
	{
		return new ItemCookedRabbit();
	}
}