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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using fNbt;
using log4net;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items.Tools;

public class ItemHoe : Item
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemHoe));

	internal ItemHoe(string name, short id) : base(name, id)
	{
		MaxStackSize = 1;
		ItemType = ItemType.Hoe;
		ExtraData = [new NbtInt("Damage", 0), new NbtInt("RepairCost", 1)];
	}

	public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block block = world.GetBlock(blockCoordinates);
		switch (block)
		{
			case Grass:
			case Dirt { DirtType: "normal" }:
			case GrassPath:
			{
				var farmland = new Farmland { Coordinates = blockCoordinates };

				if (farmland.FindWater(world, blockCoordinates, [], 0))
				{
					Log.Warn("Found water source");
					farmland.MoisturizedAmount = 7;
				}

				world.SetBlock(farmland);
				player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
				break;
			}
			case Dirt { DirtType: "coarse" }:
			{
				var dirt = new Dirt { Coordinates = blockCoordinates };

				world.SetBlock(dirt);
				player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
				break;
			}
		}
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockInteract:
			{
				if (block is not Grass && block is not Dirt && block is not GrassPath) return false;
				Damage++;
				return Damage >= GetMaxUses() - 1;
			}
			case ItemDamageReason.EntityAttack:
			{
				Damage++;
				return Damage >= GetMaxUses() - 1;
			}
			case ItemDamageReason.BlockBreak:
			case ItemDamageReason.EntityInteract:
			case ItemDamageReason.ItemUse:
			default:
				return false;
		}
	}
}