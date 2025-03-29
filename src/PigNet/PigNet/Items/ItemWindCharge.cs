using System.Collections.Concurrent;
using System.Threading.Tasks;
using PigNet.Entities.Projectiles;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

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

		const float Force = 1.5f;

		var windCharge = new WindCharge(player, world)
		{
			KnownPosition = (PlayerLocation) player.KnownPosition.Clone(),
			Velocity = player.KnownPosition.GetDirection().Normalize() * Force
		};
		windCharge.KnownPosition.Y += 1.62f;
		windCharge.SpawnEntity();

		world.BroadcastSound(new ThrowSound(player.KnownPosition), "minecraft:player");
		Item itemInHand = player.Inventory.GetItemInHand();
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand, true);
	}

	private static bool IsInCooldown(Player player)
	{
		return Cooldowns.ContainsKey(player);
	}

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