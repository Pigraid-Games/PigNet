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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Drawing;

namespace PigNet.Effects;

public class Invisibility : Effect
{
	public Invisibility() : base(EffectType.Invisibility)
	{
		ParticleColor = Color.FromArgb(0xf6, 0xf6, 0xf6);
	}

	public override void SendAdd(Player player)
	{
		player.IsInvisible = true;
		player.HideNameTag = true;
		player.BroadcastSetEntityData();

		base.SendAdd(player);
	}

	public override void SendUpdate(Player player)
	{
		player.IsInvisible = true;
		player.HideNameTag = true;
		player.BroadcastSetEntityData();

		base.SendUpdate(player);
	}

	public override void SendRemove(Player player)
	{
		player.IsInvisible = false;
		player.HideNameTag = false;
		player.BroadcastSetEntityData();

		base.SendRemove(player);
	}
}