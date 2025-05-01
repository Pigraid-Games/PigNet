using PigNet.Blocks;
using PigNet.Entities;

namespace PigNet.Items;

public abstract class ItemAxeBase : Item
{
	protected ItemAxeBase()
	{
		MaxStackSize = 1;
		ItemType = ItemType.Axe;
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockBreak:
			{
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			case ItemDamageReason.EntityAttack:
			{
				Metadata += 2;
				return Metadata >= GetMaxUses() - 1;
			}
			default:
				return false;
		}
	}
}