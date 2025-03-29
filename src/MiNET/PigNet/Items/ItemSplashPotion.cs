using PigNet.Entities.Projectiles;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public class ItemSplashPotion : Item
{
	public ItemSplashPotion(short metadata = 0) : base("minecraft:splash_potion", 438)
	{
		Metadata = metadata;
		MaxStackSize = 1;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		// Trigger the PlayerShootEvent
		if (player.OnPlayerShoot(player, this))
		{
			player.SendPlayerInventory();
			return;
		}
		const float Force = 1.5f;

		var splashPotion = new SplashPotion(player, world, Metadata) { KnownPosition = (PlayerLocation) player.KnownPosition.Clone() };
		splashPotion.KnownPosition.Y += 1.62f;
		splashPotion.Velocity = splashPotion.KnownPosition.GetDirection().Normalize() * Force;
		splashPotion.SpawnEntity();
		world.BroadcastSound(new ThrowSound(player.KnownPosition), "minecraft:player");
		Item itemInHand = player.Inventory.GetItemInHand();
		if (itemInHand.Count != 0)
		{
			byte newCount = (byte) (itemInHand.Count - 1);
			int slot = player.Inventory.InHandSlot;
			player.Inventory.SetInventorySlot(slot, new ItemSplashPotion
			{
				Count = newCount,
				Metadata = 21
			});
		}
		else
		{
			itemInHand.Count--;
			player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand, true);
		}
	}
}