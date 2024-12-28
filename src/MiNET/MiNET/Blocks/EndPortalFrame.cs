﻿#region LICENSE

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

using log4net;
using MiNET.Items;
using MiNET.Sounds;
using MiNET.Utils.Vectors;
using MiNET.Worlds;
using System.Numerics;

namespace MiNET.Blocks;

public partial class EndPortalFrame : Block
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Block));

	public EndPortalFrame() : base(120)
	{
		LightLevel = 1;
		BlastResistance = 18000000;
		Hardness = 60000;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Direction = player.GetDirection();
		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		Item itemInHand = player.Inventory.GetItemInHand();
		if (itemInHand.Name != "minecraft:ender_eye" || EndPortalEyeBit != false) return true;
		EndPortalEyeBit = true;
		world.SetBlock(this);
		// TODO: Add the sound
		world.BroadcastSound(blockCoordinates, LevelSoundEventType.BlockEndPortalFrameFill);
		if (player.GameMode != GameMode.Survival) return true;
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
		return true;
	}
}