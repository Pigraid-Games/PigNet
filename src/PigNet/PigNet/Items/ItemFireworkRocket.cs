using System;
using System.Collections.Generic;
using System.Numerics;
using fNbt;
using log4net;
using PigNet.Entities.Projectiles;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemFireworkRocket
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFireworkRocket));

	public float Spread { get; set; } = 5f;


	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		var random = new Random();
		var rocket = new FireworksRocket(player, world, this, random)
		{
			KnownPosition = blockCoordinates
		};
		rocket.KnownPosition += faceCoords + new Vector3(0, 0.01f, 0);
		rocket.KnownPosition.Yaw = random.Next(360);
		rocket.KnownPosition.Pitch = -1 * (float) (90f + ((random.NextDouble() * Spread) - (Spread / 2)));
		rocket.BroadcastMovement = true;
		rocket.DespawnOnImpact = true;
		rocket.SpawnEntity();

		if (player.GameMode != GameMode.Survival) return false;
		Item itemInHand = player.Inventory.GetItemInHand();
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
		return true;

	}

	//TODO: Enable this when we can figure out the difference between placing a block, and use item transactions :-(
	//
	//public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	//{
	//	Random random = new Random();
	//	var rocket = new FireworksRocket(player, world, this, random);
	//	rocket.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
	//	rocket.KnownPosition.Y += 1.62f;
	//	rocket.BroadcastMovement = true;
	//	rocket.DespawnOnImpact = true;
	//	rocket.SpawnEntity();
	//}

	//TAG_Compound: 1 entries {
	//	TAG_Compound("Fireworks"): 2 entries {
	//		TAG_List("Explosions"): 1 entries {
	//			TAG_Compound: 5 entries {
	//				TAG_Byte_Array("FireworkColor"): [1 bytes]
	//				TAG_Byte_Array("FireworkFade"): [0 bytes]
	//				TAG_Byte("FireworkFlicker"): 0
	//				TAG_Byte("FireworkTrail"): 0
	//				TAG_Byte("FireworkType"): 0
	//			}
	//		}
	//		TAG_Byte("Flight"): 1
	//	}
	//}

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
		public List<FireworksExplosion> Explosions { get; set; } = [];
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