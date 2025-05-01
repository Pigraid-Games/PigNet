using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Entities.Passive;

namespace PigNet.Items;

public partial class ItemShears
{
	public ItemShears()
	{
		MaxStackSize = 1;
		ItemType = ItemType.Sheers;
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockBreak:
			{
				if (block is not Web && block is not LeavesBase && block is not WoolBase && block is not Vine) return false;
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			case ItemDamageReason.EntityInteract:
			{
				if (target is not Sheep) return false;
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			default:
				return false;
		}
	}

	protected override int GetMaxUses()
	{
		return 238;
	}
}