#region LICENSE

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

using fNbt;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Entities.Passive;

namespace PigNet.Items;

public sealed class ItemShears : Item
{
	public ItemShears() : base("minecraft:shears", 359)
	{
		MaxStackSize = 1;
		ItemType = ItemType.Sheers;
		ExtraData = [new NbtInt("Damage", 0), new NbtInt("RepairCost", 1)];
		Durability = 238;
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockBreak:
			{
				if (block is not Web && block is not Leaves && block is not Leaves2 && block is not Wool && block is not Vine) return false;
				Damage++;
				return Damage >= GetMaxUses() - 1;
			}
			case ItemDamageReason.EntityInteract:
			{
				if (target is not Sheep) return false;
				Damage++;
				return Damage >= GetMaxUses() - 1;
			}
			case ItemDamageReason.BlockInteract:
			case ItemDamageReason.EntityAttack:
			case ItemDamageReason.ItemUse:
			default:
				return false;
		}
	}

	public override int GetMaxUses()
	{
		return 238;
	}
}