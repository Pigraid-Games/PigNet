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
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using fNbt;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Entities.World;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items.Tools;

public sealed class ItemFlintAndSteel : Item
{
	private const int MaxPortalHeight = 30;
	private const int MaxPortalWidth = 30;

	public ItemFlintAndSteel() : base("minecraft:flint_and_steel", 259)
	{
		MaxStackSize = 1;
		ItemType = ItemType.FlintAndSteel;
		ExtraData = [new NbtInt("Damage", 0), new NbtInt("RepairCost", 1)];
		Durability = 384;
	}

	public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block block = world.GetBlock(blockCoordinates);
		world.BroadcastSound(blockCoordinates, LevelSoundEventType.Ignite);
		switch (block)
		{
			case Tnt:
			{
				world.SetAir(block.Coordinates);
				var sound = new Sound((short) LevelEventType.SoundFuse, blockCoordinates);
				sound.Spawn(world);
				new PrimedTnt(world)
				{
					KnownPosition = new PlayerLocation
					{
						X = blockCoordinates.X + 0.5f,
						Y = blockCoordinates.Y + 0.5f,
						Z = blockCoordinates.Z + 0.5f
					},
					Fuse = 80
				}.SpawnEntity();
				player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
				break;
			}
			case Obsidian:
			{
				Block affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, face));
				if (affectedBlock.Id == 0)
				{
					List<Block> blocks = Fill(world, affectedBlock.Coordinates, BlockFace.West);
					if (blocks.Count == 0) blocks = Fill(world, affectedBlock.Coordinates, BlockFace.North);

					if (blocks.Count > 0)
						foreach (Block portal in blocks.FindAll(b => b is Portal))
							world.SetBlock(portal);
					else
					{
						if (face == BlockFace.Up)
						{
							affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
							if (affectedBlock.Id == 0)
							{
								var fire = new Fire { Coordinates = affectedBlock.Coordinates };
								world.SetBlock(fire);
							}
						}
					}
				}
				player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
				break;
			}
			default:
			{
				if (block.IsSolid)
				{
					Block affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
					if (affectedBlock.Id == 0)
					{
						var fire = new Fire { Coordinates = affectedBlock.Coordinates };
						world.SetBlock(fire);
					}
					player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
				}
				break;
			}
		}
	}

	private static List<Block> Fill(Level level, BlockCoordinates origin, BlockFace direction)
	{
		var blocks = new List<Block>();
		float length = new Vector2(MaxPortalHeight, MaxPortalWidth).Length();

		var visits = new Queue<BlockCoordinates>();

		visits.Enqueue(origin); // Kick it off with some good stuff

		while (visits.Count > 0)
		{
			BlockCoordinates coordinates = visits.Dequeue();

			if (origin.DistanceTo(coordinates) >= length) return [];

			if (level.IsAir(coordinates) && blocks.FirstOrDefault(b => b.Coordinates.Equals(coordinates)) == null)
			{
				Visit(coordinates, blocks, direction);

				switch (direction)
				{
					case BlockFace.West:
						visits.Enqueue(coordinates + Level.North);
						visits.Enqueue(coordinates + Level.South);
						break;
					case BlockFace.North:
						visits.Enqueue(coordinates + Level.West);
						visits.Enqueue(coordinates + Level.East);
						break;
					case BlockFace.Down:
					case BlockFace.Up:
					case BlockFace.South:
					case BlockFace.East:
					case BlockFace.None:
					default:
						throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
				}

				visits.Enqueue(coordinates + Level.Up);
				visits.Enqueue(coordinates + Level.Down);
			}
			else
			{
				Block block = level.GetBlock(coordinates);
				if (!IsValid(block, blocks)) return [];
			}
		}

		return blocks;
	}

	private static void Visit(BlockCoordinates coordinates, List<Block> blocks, BlockFace direction)
	{
		BlockAxis dir = direction switch
		{
			BlockFace.Down => BlockAxis.X,
			BlockFace.Up => BlockAxis.X,
			BlockFace.North => BlockAxis.X,
			BlockFace.South => BlockAxis.X,
			BlockFace.West => BlockAxis.Z,
			BlockFace.East => BlockAxis.Z,
			BlockFace.None => default,
			_ => default
		};

		blocks.Add(new Portal
		{
			Coordinates = coordinates,
			PortalAxis = dir.ToString().ToLower()
		});
	}

	private static bool IsValid(Block block, List<Block> portals)
	{
		return block is Obsidian || portals.FirstOrDefault(b => b.Coordinates.Equals(block.Coordinates) && b is Portal) != null;
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockInteract:
			{
				Damage++;
				return Damage >= GetMaxUses() - 1;
			}
			case ItemDamageReason.BlockBreak:
			case ItemDamageReason.EntityAttack:
			case ItemDamageReason.EntityInteract:
			case ItemDamageReason.ItemUse:
			default:
				return false;
		}
	}

	public override int GetMaxUses()
	{
		return 65;
	}
}