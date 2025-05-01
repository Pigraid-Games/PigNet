namespace PigNet.Items;

public partial class ItemMutton() : FoodItemBase(3, 1.8)
{
	public Item GetSmelt()
	{
		return new ItemCookedMutton();
	}
}