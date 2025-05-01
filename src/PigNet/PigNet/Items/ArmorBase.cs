using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public enum ArmorType
{
	Helmet,
	Chestplate,
	Leggings,
	Boots
}

public abstract class ArmorBase : Item
{
	protected ArmorType ArmorType { get; set; }

	protected ArmorBase(ArmorType armorType)
	{
		ArmorType = armorType;
		Durability = CalculateDurability();
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		SwithItem(player);
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		return ++Metadata >= Durability;
	}

	private int CalculateDurability()
	{
		int armor = ArmorType switch
		{
			ArmorType.Helmet => 11,
			ArmorType.Chestplate => 16,
			ArmorType.Leggings => 15,
			ArmorType.Boots => 13,
			_ => 0
		};

		int material = ItemMaterial switch
		{
			ItemMaterial.Leather => 5,
			ItemMaterial.Gold => 7,
			ItemMaterial.Chain => 15,
			ItemMaterial.Iron => 15,
			ItemMaterial.Turtle => 25,
			ItemMaterial.Diamond => 33,
			ItemMaterial.Netherite => 37,
			_ => 0
		};

		return armor * material;
	}

	private void SwithItem(Player player)
	{
		byte slot = (byte) player.Inventory.Slots.IndexOf(this);
		player.Inventory.SetInventorySlot(slot, player.Inventory.GetArmorSlot(ArmorType));

		UniqueId = GetUniqueId();
		player.Inventory.SetArmorSlot(ArmorType, this);

		PlayEquipSound(player);
	}

	private void PlayEquipSound(Player player)
	{
		LevelSoundEventType soundType = (ItemMaterial, ItemType) switch
		{
			(ItemMaterial.Leather, _) => LevelSoundEventType.ArmorEquipLeather,
			(ItemMaterial.Chain, _) => LevelSoundEventType.ArmorEquipChain,
			(ItemMaterial.Gold, _) => LevelSoundEventType.ArmorEquipGold,
			(ItemMaterial.Iron, _) => LevelSoundEventType.ArmorEquipIron,
			(ItemMaterial.Diamond, _) => LevelSoundEventType.ArmorEquipDiamond,
			(ItemMaterial.Netherite, _) => LevelSoundEventType.ArmorEquipNetherite,
			(_, ItemType.Elytra) => LevelSoundEventType.ArmorEquipElytra,
			_ => LevelSoundEventType.ArmorEquipGeneric
		};

		player.Level.BroadcastSound(player.GetEyesPosition(), soundType);
	}
}

public abstract class ItemArmorHelmetBase() : ArmorBase(ArmorType.Helmet);

public abstract class ItemArmorChestplateBase() : ArmorBase(ArmorType.Chestplate);

public abstract class ItemArmorLeggingsBase() : ArmorBase(ArmorType.Leggings);

public abstract class ItemArmorBootsBase() : ArmorBase(ArmorType.Boots);