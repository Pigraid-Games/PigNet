namespace PigNet.Items;

public partial class ItemRabbit() : FoodItemBase(1, 0.6)
{
	public Item GetSmelt()
	{
		return new ItemCookedRabbit();
	}
}