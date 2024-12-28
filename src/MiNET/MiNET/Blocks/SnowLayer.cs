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
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks;

public partial class SnowLayer : Block
{
	public SnowLayer() : base(78)
	{
		IsTransparent = true;
		BlastResistance = 0.5f;
		Hardness = 0.1f;
		IsReplaceable = true;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		Block down = world.GetBlock(Coordinates.BlockDown());
		switch (down)
		{
			case Air:
			case SnowLayer { Height: < 7 }:
				return false;
			default:
				return base.CanPlace(world, player, blockCoordinates, targetCoordinates, face);
		}
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (world.GetBlock(Coordinates) is not SnowLayer current) return false;
		if (current.Height < 6)
			Height = current.Height + 1;
		else
		{
			if (BlockFactory.GetBlockById(80) is not Snow snow) return false;
			snow.Coordinates = Coordinates;
			world.SetBlock(snow);
			return true;
		}

		return false;
	}

	public override Item[] GetDrops(Item tool)
	{
		// One per layer.
		return [ItemFactory.GetItem("minecraft:snowball", count: Height + 1)];
	}
}