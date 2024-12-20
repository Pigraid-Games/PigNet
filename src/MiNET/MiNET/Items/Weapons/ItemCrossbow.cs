using System;
using fNbt;
using MiNET.Entities.Projectiles;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items.Weapons;

public sealed class ItemCrossbow : Item
{
	private long _useTime;
	private bool _isLoading;
	private bool _hasReleased;

	public ItemCrossbow() : base("minecraft:crossbow", 471)
	{
		MaxStackSize = 1;
		ItemType = ItemType.Bow;
		ExtraData = [];
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (!_isLoading)
		{
			_useTime = Environment.TickCount64;
			_isLoading = true;
		}

		if (ExtraData is { } extraData && extraData["chargedItem"] is NbtCompound chargedItem)
		{
			if(!_hasReleased) return;

			string itemName = (chargedItem["Name"] as NbtString)?.Value;
			short damage = (chargedItem["Damage"] as NbtShort)?.Value ?? 0;
			byte count = (chargedItem["Count"] as NbtByte)?.Value ?? 0;

			Item itemInCrossbow = ItemFactory.GetItem(itemName);
			itemInCrossbow.Count = count;
			itemInCrossbow.Damage = damage;

			ExtraData.Clear();
			player.SendPlayerInventory();
			const float Force = 2.5f;
			if(itemInCrossbow is ItemArrow)
			{
				var arrow = new Arrow(player, world, 2, !(Force < 1.0))
				{
					PowerLevel = this.GetEnchantingLevel(EnchantingType.Power),
					KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
				};
				arrow.KnownPosition.Y += 1.62f;

				arrow.Velocity = arrow.KnownPosition.GetHeadDirection().Normalize() * (Force * 3.5f);
				arrow.KnownPosition.Yaw = (float) arrow.Velocity.GetYaw();
				arrow.KnownPosition.Pitch = (float) arrow.Velocity.GetPitch();
				arrow.BroadcastMovement = true;
				arrow.DespawnOnImpact = false;
				arrow.SpawnEntity();
			}
			player.Inventory.DamageItemInHand(ItemDamageReason.ItemUse, player, null);

			world.BroadcastSound(player.KnownPosition, LevelSoundEventType.CrossbowShoot);
				
			_isLoading = false;
		}
		else
		{
			long timeUsed = Environment.TickCount64 - _useTime;
			if (timeUsed >= 1100)
			{
				world.BroadcastSound(player.KnownPosition, LevelSoundEventType.CrossbowLoadingEnd);
				ExtraData =
				[
					new NbtCompound("chargedItem")
					{
						new NbtString("Name", "minecraft:arrow"),
						new NbtShort("Damage", 0),
						new NbtByte("Count", 1)
					}
				];
				player.SendPlayerInventory();
				_isLoading = false;
			} else world.BroadcastSound(player.KnownPosition, LevelSoundEventType.CrossbowLoadingStart);
		}
	}

	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		long timeUsed = Environment.TickCount64 - _useTime;
		if (timeUsed >= 1100)
		{
			world.BroadcastSound(player.KnownPosition, LevelSoundEventType.CrossbowLoadingEnd);
			ExtraData =
			[
				new NbtCompound("chargedItem")
				{
					new NbtString("Name", "minecraft:arrow"),
					new NbtShort("Damage", 0),
					new NbtByte("Count", 1)
				}
			];
			player.SendPlayerInventory();
		}
			
		_isLoading = false;
		_hasReleased = true;
	}
}