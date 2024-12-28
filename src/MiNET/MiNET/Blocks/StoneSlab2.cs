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

using System;
using System.Numerics;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class StoneSlab2 : Block
	{
		public StoneSlab2() : base(182)
		{
			BlastResistance = 30;
			Hardness = 2;
			IsTransparent = true; // Partial - blocks light.
			IsBlockingSkylight = false; // Partial - blocks light.
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var itemInHand = player.Inventory.GetItemInHand();

			TopSlotBit = (faceCoords.Y > 0.5 && face != BlockFace.Up);

			StoneSlabType2 = itemInHand.Metadata switch
			{
				0 => "red_sandstone",
				1 => "purpur",
				2 => "prismarine_rough",
				3 => "prismarine_dark",
				4 => "prismarine_brick",
				5 => "mossy_cobblestone",
				6 => "smooth_sandstone",
				7 => "red_nether_brick",
				_ => throw new ArgumentOutOfRangeException()
			};

			var slabcoordinates = new BlockCoordinates(Coordinates.X, Coordinates.Y - 1, Coordinates.Z);

			foreach (var state in world.GetBlock(slabcoordinates).GetState().States)
			{
				if (state is BlockStateString s && s.Name == "stone_slab_type_2")
				{
					if (world.GetBlock(slabcoordinates).Name == "minecraft:stone_slab2" && s.Value == StoneSlabType2)
					{
						world.SetBlock(new DoubleStoneSlab2 { StoneSlabType2 = StoneSlabType2, TopSlotBit = true });
						return true;
					}
				}
			}
			return false;
		}
	}
}