using System;
using System.Numerics;
using fNbt;
using log4net;
using Microsoft.CodeAnalysis;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Entities.Projectiles;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemBow
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemBow));

	public ItemBow()
	{
		MaxStackSize = 1;
		ItemType = ItemType.Bow;
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		//TODO: This is now NBT
		switch (reason)
		{
			case ItemDamageReason.ItemUse:
			{
				Metadata++;
				return Metadata >= GetMaxUses();
			}
			default:
				return false;
		}
	}

	protected override int GetMaxUses()
	{
		return 385;
	}

	private long _useTime = 0;

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		_useTime = world.TickTime;
	}

	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		long timeUsed = world.TickTime - _useTime;
		if (timeUsed < 3) // questionable, but we go with it for now.
		{
			player.SendPlayerInventory(); // Need to reset inventory, because we don't know what the client did here
			return;
		}

		var inventory = player.Inventory;

		bool isInfinity = this.GetEnchantingLevel(EnchantingType.Infinity) > 0;
		bool haveArrow = player.GameMode == GameMode.Creative;
		if (!haveArrow)
		{
			// Try off-hand first
			Item item = inventory.OffHand;
			if (item is ItemArrow)
			{
				haveArrow = true;
				if (!isInfinity)
				{
					item.Count -= 1;
					item.UniqueId = GetUniqueId();
					if (item.Count <= 0) inventory.OffHand = new ItemAir();

					player.SendPlayerInventory();
				}
			}
		}
		if (!haveArrow)
		{
			//TODO: Consume arrows properly
			//TODO: Make sure we deal with arrows based on "potions"
			for (byte i = 0; i < inventory.Slots.Length; i++)
			{
				Item itemStack = inventory.Slots[i];
				if (itemStack is not ItemArrow) continue;
				haveArrow = true;
				if (!isInfinity) inventory.RemoveItems(ItemFactory.GetIdByType<ItemArrow>(), 1);
				break;
			}
		}

		if (!haveArrow) return;

		float force = CalculateForce(timeUsed);
		if (force < 0.04D) return;

		var arrow = new Arrow(player, world, 2, force >= 1.0)
		{
			PowerLevel = this.GetEnchantingLevel(EnchantingType.Power),
			KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
		};
		arrow.KnownPosition.Y += 1.50f;

		var vector = arrow.KnownPosition.GetHeadDirectionVector().Normalize();
		//arrow.KnownPosition += vector * 0.5f + vector * force;

		arrow.Velocity = vector * force * 3;
		arrow.KnownPosition.Yaw = (float) arrow.Velocity.GetYaw();
		arrow.KnownPosition.HeadYaw = arrow.KnownPosition.Yaw;
		arrow.KnownPosition.Pitch = (float) arrow.Velocity.GetPitch();
		arrow.BroadcastMovement = true;
		arrow.DespawnOnImpact = false;

		arrow.SpawnEntity();
		player.Inventory.DamageItemInHand(ItemDamageReason.ItemUse, player, null);
	}

	private float CalculateForce(long timeUsed)
	{
		float force = timeUsed / 20.0F;

		force = ((force * force) + (force * 2.0F)) / 3.0F;
		if (force < 0.1D)
		{
			return 0;
		}

		if (force > 1.0F)
		{
			force = 1.0F;
		}

		return force;
	}

	public Vector3 GetShootVector(double motX, double motY, double motZ, double f, double f1)
	{
		double f2 = Math.Sqrt(motX * motX + motY * motY + motZ * motZ);

		motX /= f2;
		motY /= f2;
		motZ /= f2;
		//motX += this.random.nextGaussian() * (double)(this.random.nextBoolean() ? -1 : 1) * 0.007499999832361937D * (double)f1;
		//motY += this.random.nextGaussian() * (double)(this.random.nextBoolean() ? -1 : 1) * 0.007499999832361937D * (double)f1;
		//motZ += this.random.nextGaussian() * (double)(this.random.nextBoolean() ? -1 : 1) * 0.007499999832361937D * (double)f1;
		motX *= f;
		motY *= f;
		motZ *= f;
		return new Vector3((float) motX, (float) motY, (float) motZ);
		//thismotX = motX;
		//thismotY = motY;
		//thismotZ = motZ;
		//double f3 = Math.Sqrt(motX * motX + motZ * motZ);

		//thislastYaw = this.yaw = (float)(Math.atan2(motX, motZ) * 180.0D / 3.1415927410125732D);
		//this.lastPitch = this.pitch = (float)(Math.atan2(motY, (double)f3) * 180.0D / 3.1415927410125732D);
		//this.ttl = 0;
	}
}