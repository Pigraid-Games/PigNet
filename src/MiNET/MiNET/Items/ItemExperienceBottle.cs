﻿using MiNET.Entities.Projectiles;
using MiNET.Sounds;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items;

public class ItemExperienceBottle : Item
{
	public ItemExperienceBottle() : base("minecraft:experience_bottle")
	{
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

		var experienceBottle = new ExperienceBottle(player, world)
		{
			KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
		};
		experienceBottle.KnownPosition.Y += 1.62f;
		experienceBottle.Velocity = experienceBottle.KnownPosition.GetDirection().Normalize() * Force;
		experienceBottle.SpawnEntity();
		world.BroadcastSound(new ThrowSound(player.KnownPosition), "minecraft:player");
		Item itemInHand = player.Inventory.GetItemInHand();
		if (player.GameMode == GameMode.Creative) return;
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand, true);
	}
}