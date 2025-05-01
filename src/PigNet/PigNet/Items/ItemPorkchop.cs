namespace PigNet.Items;

public partial class ItemPorkchop() : FoodItemBase(3, 0.6)
{
	public override Item GetSmelt(string block)
	{
		return new ItemCookedPorkchop();
	}
}