using System.Collections.Concurrent;
using System.Threading.Tasks;
using MiNET.Entities.Projectiles;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemWindCharge : Item
	{
		private static readonly ConcurrentDictionary<Player, bool> Cooldowns = new();

		public ItemWindCharge() : base("minecraft:wind_charge", 1046)
		{
			MaxStackSize = 64;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			if (IsInCooldown(player))
			{
				player.SendPlayerInventory();
				return;
			}

			// Trigger the PlayerShootEvent
			if (player.OnPlayerShoot(player, this))
			{
				player.SendPlayerInventory();
				return;
			}

			StartCooldown(player);

			float force = 1.5f;

			var windCharge = new WindCharge(player, world)
			{
				KnownPosition = (PlayerLocation) player.KnownPosition.Clone(),
				Velocity = player.KnownPosition.GetDirection().Normalize() * force
			};
			windCharge.KnownPosition.Y += 1.62f;
			windCharge.SpawnEntity();

			world.BroadcastSound(player.KnownPosition, LevelSoundEventType.Throw, "minecraft:player");
			var itemInHand = player.Inventory.GetItemInHand();
			itemInHand.Count--;
		}

		private static bool IsInCooldown(Player player) => Cooldowns.ContainsKey(player);

		private static void StartCooldown(Player player)
		{
			Cooldowns[player] = true;

			Task.Run(async () =>
			{
				await Task.Delay(500); // 0.5s
				Cooldowns.TryRemove(player, out _);
			});
		}
	}
}
