namespace PigNet.Items;

public partial class ItemCod() : FoodItemBase(1, 0.6)
{
	public override Item GetSmelt(string block)
	{
		return new ItemCookedCod();
	}
}