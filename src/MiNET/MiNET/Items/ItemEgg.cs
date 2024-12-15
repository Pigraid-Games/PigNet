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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using MiNET.Entities.Projectiles;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemEgg : Item
	{
		public ItemEgg() : base("minecraft:egg", 344)
		{
			MaxStackSize = 16;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{

			// Trigger the PlayerShootEvent
			if (player.OnPlayerShoot(player, this))
			{
				player.SendPlayerInventory();
				return;
			}
			float force = 1.5f;

			var egg = new Egg(player, world);
			egg.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			egg.KnownPosition.Y += 1.62f;
			egg.Velocity = egg.KnownPosition.GetDirection().Normalize() * (force);
			egg.SpawnEntity();
			world.BroadcastSound(player.KnownPosition, LevelSoundEventType.Throw, "minecraft:player");
			var itemInHand = player.Inventory.GetItemInHand();
			itemInHand.Count--;
		}
	}
}