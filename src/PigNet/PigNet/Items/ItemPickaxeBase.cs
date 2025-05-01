using PigNet.Blocks;
using PigNet.Entities;

namespace PigNet.Items.Tools;

public abstract class ItemPickaxeBase : Item
{
	internal ItemPickaxeBase()
	{
		MaxStackSize = 1;
		ItemType = ItemType.PickAxe;
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