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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2024 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using System.Collections.Generic;
using MiNET.Sounds;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items;

public enum GoatHornType
{
	Ponder,
	Sing,
	Seek,
	Feel,
	Admire,
	Call,
	Yearn,
	Dream
}

public sealed class ItemGoatHorn : Item
{
	private static readonly Dictionary<Player, DateTime> CooldownTracker = [];

	public ItemGoatHorn(GoatHornType goatHornType = GoatHornType.Ponder) : base("minecraft:goat_horn")
	{
		Metadata = (short)goatHornType;
		MaxStackSize = 1;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (CooldownTracker.TryGetValue(player, out DateTime lastUsed))
		{
			TimeSpan timeSinceLastUse = DateTime.UtcNow - lastUsed;
			if (timeSinceLastUse.TotalSeconds < 7) return;
		}

		CooldownTracker[player] = DateTime.UtcNow;

		switch ((GoatHornType)Metadata)
		{
			case GoatHornType.Ponder:
				world.BroadcastSound(new HornCallPonderSound(player.KnownPosition));
				break;
			case GoatHornType.Sing:
				world.BroadcastSound(new HornCallSingSound(player.KnownPosition));
				break;
			case GoatHornType.Seek:
				world.BroadcastSound(new HornCallSeekSound(player.KnownPosition));
				break;
			case GoatHornType.Feel:
				world.BroadcastSound(new HornCallFeelSound(player.KnownPosition));
				break;
			case GoatHornType.Admire:
				world.BroadcastSound(new HornCallAdmireSound(player.KnownPosition));
				break;
			case GoatHornType.Call:
				world.BroadcastSound(new HornCallCallSound(player.KnownPosition));
				break;
			case GoatHornType.Yearn:
				world.BroadcastSound(new HornCallYearnSound(player.KnownPosition));
				break;
			case GoatHornType.Dream:
				world.BroadcastSound(new HornCallDreamSound(player.KnownPosition));
				break;
			default:
				return;
		}
	}
}