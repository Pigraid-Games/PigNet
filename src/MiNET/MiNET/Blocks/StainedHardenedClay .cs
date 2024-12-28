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

using System.Numerics;
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks;

public partial class StainedHardenedClay : Block
{
	public StainedHardenedClay() : base(159)
	{
		BlastResistance = 30;
		Hardness = 1.25f;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Color = BlockFactory.GetBlockColor(player.Inventory.GetItemInHand().Id, (byte) player.Inventory.GetItemInHand().Metadata);
		return false;
	}

	public override Item GetSmelt()
	{
		return Metadata switch
		{
			0 => ItemFactory.GetItem("minecraft:white_glazed_terracotta"),
			1 => ItemFactory.GetItem("minecraft:orange_glazed_terracotta"),
			2 => ItemFactory.GetItem("minecraft:magenta_glazed_terracotta"),
			3 => ItemFactory.GetItem("minecraft:light_blue_glazed_terracotta"),
			4 => ItemFactory.GetItem("minecraft:yellow_glazed_terracotta"),
			5 => ItemFactory.GetItem("minecraft:lime_glazed_terracotta"),
			6 => ItemFactory.GetItem("minecraft:pink_glazed_terracotta"),
			7 => ItemFactory.GetItem("minecraft:gray_glazed_terracotta"),
			8 => ItemFactory.GetItem("minecraft:silver_glazed_terracotta"),
			9 => ItemFactory.GetItem("minecraft:cyan_glazed_terracotta"),
			10 => ItemFactory.GetItem("minecraft:purple_glazed_terracotta"),
			11 => ItemFactory.GetItem("minecraft:blue_glazed_terracotta"),
			12 => ItemFactory.GetItem("minecraft:brown_glazed_terracotta"),
			13 => ItemFactory.GetItem("minecraft:green_glazed_terracotta"),
			14 => ItemFactory.GetItem("minecraft:red_glazed_terracotta"),
			15 => ItemFactory.GetItem("minecraft:black_glazed_terracotta"),
			_ => null
		};
	}
}