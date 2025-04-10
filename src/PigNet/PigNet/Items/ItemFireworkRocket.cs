﻿#region LICENSE

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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Numerics;
using fNbt;
using log4net;
using PigNet.Entities.Projectiles;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public class ItemFireworkRocket() : Item("minecraft:firework_rocket", 401)
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFireworkRocket));

	public float Spread { get; set; } = 5f;

	public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		// Trigger the PlayerShootEvent
		if (player.OnPlayerShoot(player, this))
		{
			player.SendPlayerInventory();
			return;
		}
		var random = new Random();
		var rocket = new FireworksRocket(player, world, this, random) { KnownPosition = blockCoordinates };
		rocket.KnownPosition += faceCoords + new Vector3(0, 0.01f, 0);
		rocket.KnownPosition.Yaw = random.Next(360);
		rocket.KnownPosition.Pitch = -1 * (float) (90f + ((random.NextDouble() * Spread) - (Spread / 2)));
		rocket.BroadcastMovement = true;
		rocket.DespawnOnImpact = true;
		rocket.SpawnEntity();

		if (player.GameMode != GameMode.Survival) return;
		Item itemInHand = player.Inventory.GetItemInHand();
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (!player.IsGliding) return;
		Item itemInHand = player.Inventory.GetItemInHand();
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);

		player.Knockback(player.KnownPosition.GetDirection() * 1.70f);
		world.BroadcastSound(new LaunchSound(player.KnownPosition));
	}

	public static NbtCompound ToNbt(FireworksData data)
	{
		var explosions = new NbtList("Explosions", NbtTagType.Compound);
		foreach (FireworksExplosion explosion in data.Explosions)
			explosions.Add(new NbtCompound
			{
				new NbtByteArray("FireworkColor", explosion.FireworkColor),
				new NbtByteArray("FireworkFade", explosion.FireworkFade),
				new NbtByte("FireworkFlicker", (byte) (explosion.FireworkFlicker ? 1 : 0)),
				new NbtByte("FireworkTrail", (byte) (explosion.FireworkTrail ? 1 : 0)),
				new NbtByte("FireworkType", (byte) explosion.FireworkType)
			});

		var root = new NbtCompound
		{
			new NbtCompound("Fireworks")
			{
				explosions,
				new NbtByte("Flight", (byte) data.Flight)
			}
		};

		return root;
	}

	public class FireworksData
	{
		public int Flight { get; set; } = 1;
		public List<FireworksExplosion> Explosions { get; set; } = new();
	}

	public class FireworksExplosion
	{
		public byte[] FireworkColor { get; set; } = new byte[3];
		public byte[] FireworkFade { get; set; } = new byte[3];
		public bool FireworkFlicker { get; set; }
		public bool FireworkTrail { get; set; }
		public int FireworkType { get; set; }
	}
}