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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion
using System;
using System.Numerics;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class CobblestoneWall : Block
	{
		public CobblestoneWall() : base(139)
		{
			IsTransparent = true;
			BlastResistance = 30;
			Hardness = 2;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var itemInHand = player.Inventory.GetItemInHand();
			WallBlockType = itemInHand.Metadata switch
			{
				0 => "cobblestone",
				1 => "mossy_cobblestone",
				2 => "granite",
				3 => "diorite",
				4 => "andesite",
				5 => "sandstone",
				6 => "red_sandstone",
				7 => "stone_brick",
				8 => "mossy_stone_brick",
				9 => "brick",
				10 => "nether_brick",
				11 => "red_nether_brick",
				12 => "end_stone",
				13 => "prismarine",
				_ => throw new ArgumentOutOfRangeException()
			};
			return false;
		}
	}
}