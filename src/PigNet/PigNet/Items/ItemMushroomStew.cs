namespace PigNet.Items;

public partial class ItemMushroomStew() : FoodItemBase(6, 7.2)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);
		var bowl = new ItemBowl();
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, bowl, true);
	}
}