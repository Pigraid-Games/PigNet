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
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public class ItemBucket : Item
{
	private bool _isUsing;

	public ItemBucket(short metadata) : base("minecraft:bucket", 325, metadata)
	{
		MaxStackSize = 1;
		FuelEfficiency = (short) (Metadata == 10 ? 1000 : 0);
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (_isUsing)
		{
			player.RemoveAllEffects();

			if (player.GameMode == GameMode.Survival || player.GameMode == GameMode.Adventure) player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, ItemFactory.GetItem(325), true);
			_isUsing = false;
			return;
		}

		_isUsing = true;
	}

	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		_isUsing = false;
	}

	public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		switch (Metadata)
		{
			//Prevent some kind of cheating...
			case 8 or 10:
			{
				var itemBlock = new ItemBlock(BlockFactory.GetBlockById((byte) Metadata));
				itemBlock.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
				break;
			}
			// Empty bucket
			case 0:
			{
				// Pick up water/lava
				Block block = world.GetBlock(blockCoordinates);
				switch (block)
				{
					case Stationary fluid:
					{
						if (fluid.LiquidDepth == 0) // Only source blocks
							world.SetAir(blockCoordinates);
						break;
					}
					case Flowing fluid:
					{
						if (fluid.LiquidDepth == 0) // Only source blocks
							world.SetAir(blockCoordinates);
						break;
					}
				}
				break;
			}
		}
		FuelEfficiency = (short) (Metadata == 10 ? 1000 : 0);
	}
}