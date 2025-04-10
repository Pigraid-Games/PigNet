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

public class ItemSignBase : ItemBlock
{
	private readonly int _standingId;
	private readonly int _wallId;

	public ItemSignBase(string name, short id, int standingId, int wallId) : base(name, id)
	{
		_standingId = standingId;
		_wallId = wallId;
		MaxStackSize = 16;
	}

	public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (!SetupSignBlock(face)) return;
		var standingSign = new StandingSign { Coordinates = blockCoordinates, Metadata = (byte)Metadata };
		standingSign.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
	}

	protected virtual bool SetupSignBlock(BlockFace face)
	{
		return face != BlockFace.Down;
	}
}

public class ItemSign() : ItemSignBase("minecraft:oak_sign", 323, 63, 68);

public class ItemAcaciaSign() : ItemSignBase("minecraft:acacia_sign", 475, 445, 456);

public class ItemSpruceSign() : ItemSignBase("minecraft:spruce_sign", 472, 436, 437);

public class ItemBirchSign() : ItemSignBase("minecraft:birch_sign", 473, 441, 442);

public class ItemJungleSign() : ItemSignBase("minecraft:jungle_sign", 474, 443, 444);

public class ItemDarkoakSign() : ItemSignBase("minecraft:dark_oak_sign", 476, 447, 448);

public class ItemMangroveSign() : ItemSignBase("minecraft:mangrove_sign", 1005, 63, 68); // TODO: Fix standingId & wallId

public class ItemCherrySign() : ItemSignBase("minecraft:cherry_sign", 1056, 63, 68); // TODO: Fix standingId & wallId

public class ItemPaleOakSign() : ItemSignBase("minecraft:pale_oak_sign", 1057, 63, 68); // TODO: Fix standingId & wallId

public class ItemBambooSign() : ItemSignBase("minecraft:bamboo_sign", 1058, 63, 68); // TODO: Fix standingId & wallId

public class ItemCrimsonSign() : ItemSignBase("minecraft:crimson_sign", 753, 505, 507);

public class ItemWarpedSign() : ItemSignBase("minecraft:warped_sign", 754, 63, 68); // TODO: Fix standingId & wallId