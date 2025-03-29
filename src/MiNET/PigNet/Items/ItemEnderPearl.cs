using System.Collections.Concurrent;
using System.Threading.Tasks;
using PigNet.Entities.Projectiles;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public class ItemEnderPearl : Item
{
	private static readonly ConcurrentDictionary<Player, bool> Cooldowns = new();

	public ItemEnderPearl() : base("minecraft:ender_pearl", 368)
	{
		MaxStackSize = 16;
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

		var enderPearl = new Enderpearl(player, world)
		{
			KnownPosition = (PlayerLocation) player.KnownPosition.Clone(),
			Velocity = player.KnownPosition.GetDirection().Normalize() * Force
		};
		enderPearl.KnownPosition.Y += 1.62f;
		enderPearl.SpawnEntity();

		world.BroadcastSound(new ThrowSound(player.KnownPosition), "minecraft:player");
		if (player.GameMode == GameMode.Creative) return;
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
			await Task.Delay(1000);
			Cooldowns.TryRemove(player, out _);
		});
	}
}