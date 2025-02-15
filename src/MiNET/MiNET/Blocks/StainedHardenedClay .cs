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
		switch (Metadata)
		{
			case 0:
				return ItemFactory.GetItem(220);
			case 8:
				return ItemFactory.GetItem(228);
			case 7:
				return ItemFactory.GetItem(227);
			case 15:
				return ItemFactory.GetItem(235);
			case 12:
				return ItemFactory.GetItem(232);
			case 14:
				return ItemFactory.GetItem(234);
			case 1:
				return ItemFactory.GetItem(221);
			case 4:
				return ItemFactory.GetItem(224);
			case 5:
				return ItemFactory.GetItem(225);
			case 13:
				return ItemFactory.GetItem(233);
			case 9:
				return ItemFactory.GetItem(229);
			case 3:
				return ItemFactory.GetItem(223);
			case 11:
				return ItemFactory.GetItem(231);
			case 10:
				return ItemFactory.GetItem(219);
			case 2:
				return ItemFactory.GetItem(222);
			case 6:
				return ItemFactory.GetItem(226);
			default:
				return null;
		}
	}
}