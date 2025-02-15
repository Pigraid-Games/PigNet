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
using MiNET.Effects;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items.Food;

public class ItemEnchantedApple() : FoodItem("minecraft:enchanted_golden_apple", 466, 0, 4, 9.6)
{
	private bool _isUsing;

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (_isUsing)
		{
			Count--;
			player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, this);

			Consume(player);
			_isUsing = false;
			return;
		}
		if (player.HungerManager.CanEat()) _isUsing = true;
	}

	protected override void Consume(Player player)
	{
		base.Consume(player);
		var absorptionEffect = new Absorption
		{
			Duration = 2400,
			EffectId = EffectType.Absorption,
			Level = 4
		};
		var regenerationEffect = new Regeneration
		{
			Duration = 600,
			EffectId = EffectType.Regeneration,
			Level = 2
		};
		var fireResistanceEffect = new FireResistance
		{
			Duration = 3600,
			EffectId = EffectType.FireResistance,
			Level = 1
		};
		var resistanceEffect = new Resistance
		{
			Duration = 3600,
			EffectId = EffectType.Resistance,
			Level = 1
		};

		player.SetEffect(absorptionEffect);
		player.SetEffect(regenerationEffect);
		player.SetEffect(fireResistanceEffect);
		player.SetEffect(resistanceEffect);
		Console.WriteLine($"Send all the effects to the player {player.Username}");
	}
}