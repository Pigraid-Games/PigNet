#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using fNbt;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Entities.Projectiles;
using PigNet.Sounds;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items.Weapons;

public sealed class ItemBow : Item
{
	private long _useTime;

	public ItemBow() : base("minecraft:bow", 261)
	{
		MaxStackSize = 1;
		ItemType = ItemType.Bow;
		ExtraData = [new NbtInt("Damage", 0), new NbtInt("RepairCost", 1)];
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.ItemUse:
			{
				Damage++;
				return Damage >= GetMaxUses();
			}
			case ItemDamageReason.BlockBreak:
			case ItemDamageReason.BlockInteract:
			case ItemDamageReason.EntityAttack:
			case ItemDamageReason.EntityInteract:
			default:
				return false;
		}
	}

	public override int GetMaxUses()
	{
		return 385;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		_useTime = world.TickTime;
	}

	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		long timeUsed = world.TickTime - _useTime;
		if (timeUsed < 6)
		{
			// Prevent unintended actions for very short use durations
			player.SendPlayerInventory();
			return;
		}

		// Trigger the PlayerShootEvent
		if (player.OnPlayerShoot(player, this))
		{
			player.SendPlayerInventory();
			return;
		}

		PlayerInventory inventory = player.Inventory;

		bool isInfinity = this.GetEnchantingLevel(EnchantingType.Infinity) > 0;
		bool haveArrow = false;
		byte effect = 0;

		// Check for arrows in off-hand
		Item item = inventory.OffHandInventory.GetItem();
		if (item is ItemArrow)
		{
			haveArrow = true;
			effect = (byte) item.Metadata;
			if (!isInfinity && player.GameMode != GameMode.Creative)
			{
				item.Count -= 1;
				item.UniqueId = Environment.TickCount;
				if (item.Count <= 0)
					inventory.OffHandInventory.SetItem(new ItemAir());
			}
		}

		// Check for arrows in inventory if none in off-hand
		if (!haveArrow)
			for (byte i = 0; i < inventory.Slots.Count; i++)
			{
				Item itemStack = inventory.Slots[i];
				if (itemStack is not ItemArrow) continue;
				haveArrow = true;
				effect = (byte) itemStack.Metadata;
				if (!isInfinity && player.GameMode != GameMode.Creative)
				{
					itemStack.Count--;
					inventory.SetInventorySlot(i, itemStack);
				}
				break;
			}

		if (!haveArrow) return;

		float force = CalculateForce(timeUsed);
		if (force < 0.1D) return;

		var arrow = new Arrow(player, world, 2, !(force < 1.0))
		{
			PowerLevel = this.GetEnchantingLevel(EnchantingType.Power),
			EffectValue = effect,
			isFlame = this.GetEnchantingLevel(EnchantingType.Flame) > 0,
			KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
		};
		arrow.KnownPosition.Y += 1.62f;
		arrow.Velocity = arrow.KnownPosition.GetHeadDirection().Normalize() * (force * 3);
		arrow.KnownPosition.Yaw = (float) arrow.Velocity.GetYaw();
		arrow.KnownPosition.Pitch = (float) arrow.Velocity.GetPitch();
		arrow.BroadcastMovement = true;
		arrow.DespawnOnImpact = false;

		world.BroadcastSound(new BowSound(player.KnownPosition));
		arrow.SpawnEntity();

		inventory.DamageItemInHand(ItemDamageReason.ItemUse, player, null);

		player.SendPlayerInventory();
	}


	private static float CalculateForce(long timeUsed)
	{
		float force = timeUsed / 20.0F;

		force = ((force * force) + (force * 2.0F)) / 3.0F;
		if (force < 0.1D) return 0;
		if (force > 1.0F) force = 1.0F;
		return force;
	}

	public Vector3 GetShootVector(double motX, double motY, double motZ, double f, double f1)
	{
		double f2 = Math.Sqrt((motX * motX) + (motY * motY) + (motZ * motZ));

		motX /= f2;
		motY /= f2;
		motZ /= f2;
		motX *= f;
		motY *= f;
		motZ *= f;
		return new Vector3((float) motX, (float) motY, (float) motZ);
	}
}