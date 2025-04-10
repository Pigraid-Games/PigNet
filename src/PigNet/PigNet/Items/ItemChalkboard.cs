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

public class ItemSlate : ItemBlock
{
	public ItemSlate(short size = 0) : base("minecraft:board", 454, size)
	{
		MaxStackSize = 16;
	}

	public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		// block 230, data 32-35 (rotations) Slate, Poster or Board

		if (face == BlockFace.Down) // At the bottom of block
			// Doesn't work, ignore if that happen. 
			return;
		Block = BlockFactory.GetBlockById(230);
		Block.Metadata = (byte) Metadata;

		base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
	}
}

public class ItemPoster() : ItemSlate(1);

public class ItemBoard() : ItemSlate(2);