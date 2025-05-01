namespace PigNet.Items;

public partial class ItemChicken() : FoodItemBase(2, 1.2)
{
	public override Item GetSmelt(string block)
	{
		return new ItemCookedChicken();
	}
}