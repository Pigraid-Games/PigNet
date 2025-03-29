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
using PigNet.Items;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class StoneSlab4 : Block
{
	public StoneSlab4() : base(421)
	{
		BlastResistance = 30;
		Hardness = 2;
		IsTransparent = true; // Partial - blocks light.
		IsBlockingSkylight = false; // Partial - blocks light.
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Item itemInHand = player.Inventory.GetItemInHand();

		TopSlotBit = faceCoords.Y > 0.5 && face != BlockFace.Up;

		StoneSlabType4 = itemInHand.Metadata switch
		{
			0 => "mossy_stone_brick",
			1 => "smooth_quartz",
			2 => "stone",
			3 => "cut_sandstone",
			4 => "cut_red_sandstone",
			_ => throw new ArgumentOutOfRangeException()
		};

		var slabcoordinates = new BlockCoordinates(Coordinates.X, Coordinates.Y - 1, Coordinates.Z);

		foreach (IBlockState state in world.GetBlock(slabcoordinates).GetState().States)
			if (state is BlockStateString s && s.Name == "stone_slab_type_4")
				if (world.GetBlock(slabcoordinates).Name == "minecraft:stone_slab4" && s.Value == StoneSlabType4)
				{
					world.SetBlock(new DoubleStoneSlab4
					{
						StoneSlabType4 = StoneSlabType4,
						TopSlotBit = true
					});
					return true;
				}
		return false;
	}
}