using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using fNbt;
using log4net;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Net.Packets.Mcpe;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Inventories;

public enum ArmorSlots
{
	Head = 0,
	Chest = 1,
	Legs = 2,
	Feet = 3
}

public enum MobHeads
{
	SkeletonHead = 1059,
	WitherSkull = -965,
	ZombieHead = -966,
	PlayerHead = -967,
	CreeperHead = -968,
	DragonHead = -969,
	PiglinHead = 1060
}

public class ArmorInventory
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ArmorInventory));

	public Entity Entity { get; set; }
	private readonly ConcurrentDictionary<ArmorSlots, Item> _inventoryItems = [];

	private static readonly Random RandomGenerator = new();

	/// <summary>
	/// This list is used to create exception items, for example, custom items.
	/// If in the exception list, it'll send the item to the specified ArmorSlots
	/// </summary>
	public static readonly ConcurrentDictionary<ArmorSlots, List<int>> ExceptionItems = [];

	/// <summary>
	/// Adds an item id to the exception list for a given armor slot
	/// </summary>
	/// <param name="slotType">The type of the slot</param>
	/// <param name="itemId">The id of the item</param>
	/// <returns></returns>
	public static bool AddItemToExceptionList(ArmorSlots slotType, int itemId)
	{
		ExceptionItems.TryGetValue(slotType, out List<int> list);
		list ??= [];
		list.Add(itemId);
		return ExceptionItems.TryAdd(slotType, list);
	}


	/// <summary>
	/// Removes the item id from the exception list for a given armor slot
	/// </summary>
	/// <param name="slotType">The type of the slot</param>
	/// <param name="itemId">The id of the item</param>
	/// <returns></returns>
	public static bool RemoveItemFromExceptionList(ArmorSlots slotType, int itemId)
	{
		if (ExceptionItems.TryGetValue(slotType, out List<int> list))
		{
			if (list.Remove(itemId))
			{
				if (list.Count == 0)
				{
					ExceptionItems.TryRemove(slotType, out _);
				}
				return true;
			}
		}
		return false;
	}

	public ArmorInventory(Entity entity)
	{
		Entity = entity;

		_inventoryItems.TryAdd(ArmorSlots.Head, new ItemAir());
		_inventoryItems.TryAdd(ArmorSlots.Chest, new ItemAir());
		_inventoryItems.TryAdd(ArmorSlots.Legs, new ItemAir());
		_inventoryItems.TryAdd(ArmorSlots.Feet, new ItemAir());
	}

	public Item GetHeadItem() => _inventoryItems[ArmorSlots.Head];
	public Item GetChestItem() => _inventoryItems[ArmorSlots.Chest];
	public Item GetLegsItem() => _inventoryItems[ArmorSlots.Legs];
	public Item GetFeetItem() => _inventoryItems[ArmorSlots.Feet];

	public bool SetHeadItem(Item item, bool sendUpdate = true, bool sendEquipSound = true)
	{
		// NOTE: 0 == Item::AIR, -155 == Item::CARVED_PUMPKIN
		if (item is not ArmorHelmetBase && item.Id != 0 && item.Id != -155 && !IsMobHead(item.Id) &&
			(!ExceptionItems.TryGetValue(ArmorSlots.Head, out List<int> exceptionList) || !exceptionList.Contains(item.Id)))
		{
			Log.Error($"Item is not accepted in the head armor slot, it is not an ArmorHelmetBase class, for item Id: {item.Id} Name: {item.Name} Count: {item.Name}, NBT: {item.ExtraData}");
			return false;
		}

		_inventoryItems[ArmorSlots.Head] = item;
		if (sendUpdate)
			SendMobArmorEquipmentPacket();
		if (sendEquipSound)
			SendEquipSoundPacket(item);
		if (Entity is Player player)
			SendArmorContentPacket(player);
		return true;
	}

	public bool SetChestItem(Item item, bool sendUpdate = true, bool sendEquipSound = true)
	{
		if (item is not ArmorChestplateBase && item.Id != 0 && item.ItemType != ItemType.Elytra &&
			(!ExceptionItems.TryGetValue(ArmorSlots.Chest, out List<int> exceptionList) || !exceptionList.Contains(item.Id)))
		{
			Log.Error($"Item is not accepted in the chest armor slot, it is not an ArmorChestplateBase class, for item Id: {item.Id} Name: {item.Name} Count: {item.Name}, NBT: {item.ExtraData}");
			return false;
		}

		_inventoryItems[ArmorSlots.Chest] = item;

		if (sendUpdate)
			SendMobArmorEquipmentPacket();
		if (sendEquipSound)
			SendEquipSoundPacket(item);
		if (Entity is Player player)
			SendArmorContentPacket(player);
		return true;
	}

	public bool SetLegsItem(Item item, bool sendUpdate = true, bool sendEquipSound = true)
	{
		if (item is not ArmorLeggingsBase && item.Id != 0 &&
			(!ExceptionItems.TryGetValue(ArmorSlots.Legs, out List<int> exceptionList) || !exceptionList.Contains(item.Id)))
		{
			Log.Error($"Item is not accepted in the legs armor slot, it is not an ArmorLeggingsBase class, for item Id: {item.Id} Name: {item.Name} Count: {item.Name}, NBT: {item.ExtraData}");
			return false;
		}

		_inventoryItems[ArmorSlots.Legs] = item;
		if (sendUpdate)
			SendMobArmorEquipmentPacket();
		if (sendEquipSound)
			SendEquipSoundPacket(item);
		if (Entity is Player player)
			SendArmorContentPacket(player);
		return true;
	}

	public bool SetFeetItem(Item item, bool sendUpdate = true, bool sendEquipSound = true)
	{
		if (item is not ArmorBootsBase && item.Id != 0 &&
			(!ExceptionItems.TryGetValue(ArmorSlots.Feet, out List<int> exceptionList) || !exceptionList.Contains(item.Id)))
		{
			Log.Error($"Item is not accepted in the feet armor slot, it is not an ArmorBootsBase class, for item Id: {item.Id} Name: {item.Name} Count: {item.Name}, NBT: {item.ExtraData}");
			return false;
		}

		_inventoryItems[ArmorSlots.Feet] = item;
		if (sendUpdate)
			SendMobArmorEquipmentPacket();
		if (sendEquipSound)
			SendEquipSoundPacket(item);
		if (Entity is Player player)
			SendArmorContentPacket(player);
		return true;
	}

	public ItemStacks GetAll()
	{
		return
		[
			_inventoryItems[ArmorSlots.Head] ?? new ItemAir(),
			_inventoryItems[ArmorSlots.Chest] ?? new ItemAir(),
			_inventoryItems[ArmorSlots.Legs] ?? new ItemAir(),
			_inventoryItems[ArmorSlots.Feet] ?? new ItemAir()
		];
	}

	public void Clear(bool updateClient = true)
	{
		_inventoryItems[ArmorSlots.Head] = new ItemAir();
		_inventoryItems[ArmorSlots.Chest] = new ItemAir();
		_inventoryItems[ArmorSlots.Legs] = new ItemAir();
		_inventoryItems[ArmorSlots.Feet] = new ItemAir();
		if (updateClient)
			SendMobArmorEquipmentPacket();
	}

	public virtual void DamageAll()
	{
		if (Entity is not Player player)
			return;
		if (player.GameMode != GameMode.Survival || player.IsInvicible || player.HealthManager.CooldownTick > 0)
			return;

		bool armorBroke = false;

		_inventoryItems[ArmorSlots.Head] = DamageArmorItem(_inventoryItems[ArmorSlots.Head], ref armorBroke);
		_inventoryItems[ArmorSlots.Chest] = DamageArmorItem(_inventoryItems[ArmorSlots.Chest], ref armorBroke);
		_inventoryItems[ArmorSlots.Legs] = DamageArmorItem(_inventoryItems[ArmorSlots.Legs], ref armorBroke);
		_inventoryItems[ArmorSlots.Feet] = DamageArmorItem(_inventoryItems[ArmorSlots.Feet], ref armorBroke);

		SendArmorContentPacket(player);

		if (!armorBroke)
			return;

		// TODO: Test if the armor break broadcast works on the client and other connected clients, shouldn't neded to send the whole inventory to everyone.
		//player.SendArmorForPlayer(); // TODO: Refactor this so it's within the class, should only send the armor content
		player.Level.BroadcastSound((BlockCoordinates) player.KnownPosition, LevelSoundEventType.Break, -1);
	}

	private Item DamageArmorItem(Item item, ref bool armorBroke)
	{
		if (Entity is not Player player)
			return item;
		if (player.GameMode != GameMode.Survival)
			return item;

		var unbreakingLevel = item.GetEnchantingLevel(EnchantingType.Unbreaking);
		if (unbreakingLevel > 0)
		{
			if (Random.Shared.Next(1 + unbreakingLevel) != 0)
				return item;
		}

		item.Damage++;

		if (item.Damage >= item.Durability && item.Id != 0)
		{
			item = new ItemAir();
			armorBroke = true;
		}

		if (item.ExtraData != null)
		{
			var damageNbt = item.ExtraData.Get<NbtInt>("Damage");
			if (damageNbt != null)
			{
				damageNbt.Value = item.Damage;
			}
		}

		return item;
	}

	public void SendArmorContentPacket(Player player)
	{
		var armorContent = McpeInventoryContent.CreateObject();
		armorContent.inventoryId = 0x78;
		armorContent.slots = GetAll();
		player.SendPacket(armorContent);
	}

	public void SendMobArmorEquipmentPacket(Player[] receivers = null)
	{
		McpeMobArmorEquipment packet = McpeMobArmorEquipment.CreateObject();
		packet.helmet = GetHeadItem();
		packet.chestplate = GetChestItem();
		packet.leggings = GetLegsItem();
		packet.boots = GetFeetItem();
		packet.runtimeActorId = Entity.EntityId;
		//Entity.Level.RelayBroadcast(packet);

		if (Entity is not Player player)
			return;
		if (receivers == null)
		{
			Entity.Level.RelayBroadcast(player, packet);
		}
		else
		{
			Entity.Level.RelayBroadcast(player, receivers, packet);
		}
	}

	private void SendEquipSoundPacket(Item item)
	{
		if (Entity is not Player player)
			return;

		LevelSoundEventType levelEventType;
		switch (item.ItemMaterial)
		{
			case ItemMaterial.Leather:
				levelEventType = LevelSoundEventType.ArmorEquipLeather;
				break;
			case ItemMaterial.Chain:
				levelEventType = LevelSoundEventType.ArmorEquipChain;
				break;
			case ItemMaterial.Iron:
				levelEventType = LevelSoundEventType.ArmorEquipIron;
				break;
			case ItemMaterial.Gold:
				levelEventType = LevelSoundEventType.ArmorEquipGold;
				break;
			case ItemMaterial.Diamond:
				levelEventType = LevelSoundEventType.ArmorEquipDiamond;
				break;
			case ItemMaterial.Netherite:
				levelEventType = LevelSoundEventType.ArmorEquipNetherite;
				break;
			default:
				levelEventType = LevelSoundEventType.Armor;
				break;
		}
		var packet = McpeLevelSoundEvent.CreateObject();
		packet.position = Entity.KnownPosition;
		packet.soundId = (uint) levelEventType;
		packet.extraData = 0;
		player.SendPacket(packet);
	}

	private static bool IsMobHead(int itemId)
	{
		return Enum.IsDefined(typeof(MobHeads), itemId);
	}
}