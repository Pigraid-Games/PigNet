using PigNet.Blocks;
using PigNet.Entities;

namespace PigNet.Items;

public abstract class ItemSwordBase : Item
{
	internal ItemSwordBase()
	{
		MaxStackSize = 1;
		ItemType = ItemType.Sword;
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockBreak:
			{
				Metadata += 2;
				return Metadata >= GetMaxUses() - 1;
			}
			case ItemDamageReason.EntityAttack:
			{
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			default:
				return false;
		}
	}
}