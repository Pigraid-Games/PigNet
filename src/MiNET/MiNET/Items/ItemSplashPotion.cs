using log4net;
using MiNET.Entities.Projectiles;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSplashPotion : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemSplashPotion));

		public ItemSplashPotion(short metadata = 0) : base("minecraft:splash_potion", 438)
		{
			Metadata = metadata;
			MaxStackSize = 64;
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

			var splashPotion = new SplashPotion(player, world, Metadata)
			{
				KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
			};
			splashPotion.KnownPosition.Y += 1.62f;
			splashPotion.Velocity = splashPotion.KnownPosition.GetDirection().Normalize() * Force;
			splashPotion.SpawnEntity();
			world.BroadcastSound(player.KnownPosition, LevelSoundEventType.Throw, "minecraft:player");
			Item itemInHand = player.Inventory.GetItemInHand();
			if (itemInHand.Count != 0)
			{
				byte newCount = (byte)(itemInHand.Count - 1);
				int slot = player.Inventory.InHandSlot;
				player.Inventory.SetInventorySlot(slot, new ItemSplashPotion() { Count = newCount, Metadata = 21 });
			} else
			{
				itemInHand.Count--;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand, true);
			}
		}
	}
}
