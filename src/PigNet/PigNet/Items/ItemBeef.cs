namespace PigNet.Items;

public partial class ItemBeef() : FoodItemBase(3, 1.8)
{
	public override Item GetSmelt(string block)
	{
		return new ItemCookedBeef();
	}
}