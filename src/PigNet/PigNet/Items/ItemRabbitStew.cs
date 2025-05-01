namespace PigNet.Items;

public partial class ItemRabbitStew() : FoodItemBase(10, 12)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);
		var bowl = new ItemBowl();
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, bowl, true);
	}
}