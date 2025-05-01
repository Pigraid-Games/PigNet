using PigNet.Entities.Projectiles;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemExperienceBottle
{
	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		// Trigger the PlayerShootEvent
		if (player.OnPlayerShoot(player, this))
		{
			player.SendPlayerInventory();
			return;
		}

		const float Force = 1.5f;

		var experienceBottle = new ExperienceBottle(player, world) { KnownPosition = (PlayerLocation) player.KnownPosition.Clone() };
		experienceBottle.KnownPosition.Y += 1.62f;
		experienceBottle.Velocity = experienceBottle.KnownPosition.ToVector3() * Force;
		experienceBottle.SpawnEntity();
		world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.Throw);
		Item itemInHand = player.Inventory.GetItemInHand();
		if (player.GameMode == GameMode.Creative) return;
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand, true);
	}
}