#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items;

//A door specifies its hinge side in the block data of its upper block, 
// and its facing and opened status in the block data of its lower block
public class ItemWoodenDoor(string name = "minecraft:wooden_door", short itemId = 324, byte blockId = 64)
	: ItemBlock(name, itemId)
{
	public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		byte direction = player.GetDirection();

		BlockCoordinates coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

		// Base block, meta sets orientation
		var block = (DoorBase) BlockFactory.GetBlockById(blockId);
		block.Coordinates = coordinates;
		block.Direction = direction;
		block.UpperBlockBit = false;

		int x = blockCoordinates.X;
		int y = blockCoordinates.Y;
		int z = blockCoordinates.Z;

		int xd = 0;
		int zd = 0;

		switch (direction)
		{
			case 0:
				zd = 1;
				break;
			case 1:
				xd = -1;
				break;
			case 2:
				zd = -1;
				break;
			case 3:
				xd = 1;
				break;
		}

		int i1 = (world.GetBlock(x - xd, y, z - zd).IsSolid ? 1 : 0) + (world.GetBlock(x - xd, y + 1, z - zd).IsSolid ? 1 : 0);
		int j1 = (world.GetBlock(x + xd, y, z + zd).IsSolid ? 1 : 0) + (world.GetBlock(x + xd, y + 1, z + zd).IsSolid ? 1 : 0);
		bool flag = world.GetBlock(x - xd, y, z - zd).Id == block.Id || world.GetBlock(x - xd, y + 1, z - zd).Id == block.Id;
		bool flag1 = world.GetBlock(x + xd, y, z + zd).Id == block.Id || world.GetBlock(x + xd, y + 1, z + zd).Id == block.Id;
		bool flag2 = false;

		if (flag && !flag1)
			flag2 = true;
		else if (j1 > i1) flag2 = true;

		if (!block.CanPlace(world, player, blockCoordinates, face)) return;

		block.DoorHingeBit = flag2;

		// The upper door block, meta marks upper and
		// sets orientation based on adjacent blocks
		var blockUpper = (DoorBase) BlockFactory.GetBlockById(blockId);
		blockUpper.Coordinates = coordinates + Level.Up;
		blockUpper.Direction = direction;
		blockUpper.UpperBlockBit = true;
		blockUpper.DoorHingeBit = flag2;

		world.SetBlock(block);
		world.SetBlock(blockUpper);

		if (player.GameMode != GameMode.Survival) return;
		Item itemInHand = player.Inventory.GetItemInHand();
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
	}
}

public class ItemSpruceDoor() : ItemWoodenDoor("minecraft:spruce_door", 427, 193);

public class ItemBirchDoor() : ItemWoodenDoor("minecraft:birch_door", 428, 194);

public class ItemJungleDoor() : ItemWoodenDoor("minecraft:jungle_door", 429, 195);

public class ItemAcaciaDoor() : ItemWoodenDoor("minecraft:acacia_door", 430, 196);

public class ItemDarkOakDoor() : ItemWoodenDoor("minecraft:dark_oak_door", 431, 197);

public class ItemIronDoor() : ItemWoodenDoor("minecraft:iron_door", 330, 71);