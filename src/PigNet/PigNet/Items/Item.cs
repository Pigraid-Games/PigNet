using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using fNbt;
using fNbt.Serialization;
using Newtonsoft.Json;
using PigNet.BlockEntities;
using PigNet.Blocks;
using PigNet.Crafting;
using PigNet.Entities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

//// <summary>
/// Items are objects which only exist within the player's inventory and hands - which means, they cannot be placed in
/// the game world. Some items simply place blocks or entities into the game world when used. They are thus an item
/// when in the inventory and a block when placed. Some examples of objects which exhibit these properties are item
/// frames, which turn into an entity when placed, and beds, which turn into a group of blocks when placed. When
/// equipped, items (and blocks) briefly display their names above the HUD.
/// </summary>
public abstract class Item : ICloneable
{
	private static int _uniqueIdIncrement = 100000;

	private readonly Lazy<int> _runtimeId;

	protected Item()
	{
		_runtimeId = new Lazy<int>(() => ItemFactory.GetRuntimeIdById(Id));
	}

	[Obsolete] public short LegacyId { get; protected set; }

	[NbtProperty("Name")] public virtual string Id { get; protected set; } = string.Empty;
	public virtual int RuntimeId => _runtimeId.Value;
	public int UniqueId { get; set; } = GetUniqueId();
	public virtual int BlockRuntimeId { get; protected set; }
	[NbtProperty("Damage")] public short Metadata { get; set; }
	[NbtProperty] public byte Count { get; set; } = 1;
	[NbtProperty("tag")] public virtual NbtCompound ExtraData { get; set; }

	public virtual bool Edu { get; protected set; } = false;

	[JsonIgnore] public virtual ItemMaterial ItemMaterial { get; set; } = ItemMaterial.None;

	[JsonIgnore] public virtual ItemType ItemType { get; set; } = ItemType.Item;

	[JsonIgnore] public virtual int MaxStackSize { get; set; } = 64;

	[JsonIgnore] public bool IsStackable => MaxStackSize > 1;

	[JsonIgnore] public int Durability { get; set; }

	[JsonIgnore] public int FuelEfficiency { get; set; }

	[JsonIgnore] public bool Unbreakable { get; set; } = false;

	public virtual object Clone()
	{
		var item = MemberwiseClone() as Item;
		item.UniqueId = GetUniqueId();

		return item;
	}

	public virtual void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
	}

	public virtual bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return false;
	}

	public virtual bool BreakBlock(Level world, Player player, Block block, BlockEntity blockEntity)
	{
		return true;
	}

	public virtual bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		return false;
	}

	protected virtual int GetMaxUses()
	{
		switch (ItemMaterial)
		{
			case ItemMaterial.Wood:
				return 60;
			case ItemMaterial.Gold:
				return 33;
			case ItemMaterial.Stone:
				return 132;
			case ItemMaterial.Iron:
				return 251;
			case ItemMaterial.Diamond:
				return 1562;
			default:
				return 0;
		}
	}

	public virtual bool Animate(Level world, Player player)
	{
		return false;
	}

	public BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
	{
		switch (face)
		{
			case BlockFace.Down:
				return target + Level.Down;
			case BlockFace.Up:
				return target + Level.Up;
			case BlockFace.North:
				return target + Level.North;
			case BlockFace.South:
				return target + Level.South;
			case BlockFace.West:
				return target + Level.West;
			case BlockFace.East:
				return target + Level.East;
			default:
				return target;
		}
	}

	public int GetDamage()
	{
		switch (ItemType)
		{
			case ItemType.Sword:
				return GetSwordDamage(ItemMaterial);
			case ItemType.Item:
				return 1;
			case ItemType.Axe:
				return GetAxeDamage(ItemMaterial);
			case ItemType.PickAxe:
				return GetPickAxeDamage(ItemMaterial);
			case ItemType.Shovel:
				return GetShovelDamage(ItemMaterial);
			default:
				return 1;
		}
	}

	protected int GetSwordDamage(ItemMaterial itemMaterial)
	{
		switch (itemMaterial)
		{
			case ItemMaterial.Wood:
				return 5;
			case ItemMaterial.Gold:
				return 5;
			case ItemMaterial.Stone:
				return 6;
			case ItemMaterial.Iron:
				return 7;
			case ItemMaterial.Diamond:
				return 8;
			default:
				return 1;
		}
	}

	private int GetAxeDamage(ItemMaterial itemMaterial)
	{
		return GetSwordDamage(itemMaterial) - 1;
	}

	private int GetPickAxeDamage(ItemMaterial itemMaterial)
	{
		return GetSwordDamage(itemMaterial) - 2;
	}

	private int GetShovelDamage(ItemMaterial itemMaterial)
	{
		return GetSwordDamage(itemMaterial) - 3;
	}

	public virtual Item GetSmelt(string block)
	{
		RecipeManager.TryGetSmeltingResult(this, block, out Item result);

		return result;
	}

	public virtual void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
	}

	protected virtual bool Equals(Item other)
	{
		if (Id != other.Id || Metadata != other.Metadata) return false;
		if ((ExtraData == null) ^ (other.ExtraData == null)) return false;

		//TODO: This doesn't work in  most cases. We need to fix comparison when name == null
		byte[] saveToBuffer = null;
		if (other.ExtraData?.Name != null) saveToBuffer = new NbtFile(other.ExtraData).SaveToBuffer(NbtCompression.None);
		byte[] saveToBuffer2 = null;
		if (ExtraData?.Name != null) saveToBuffer2 = new NbtFile(ExtraData).SaveToBuffer(NbtCompression.None);
		bool nbtCheck = !((saveToBuffer == null) ^ (saveToBuffer2 == null));
		if (nbtCheck)
		{
			if (saveToBuffer == null)
				nbtCheck = true;
			else
				nbtCheck = saveToBuffer.SequenceEqual(saveToBuffer2);
		}

		return nbtCheck;
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (!(obj is Item)) return false;
		return Equals((Item) obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Id, Metadata);
	}

	public override string ToString()
	{
		return $"{GetType().Name}(Id={Id}, Meta={Metadata}, UniqueId={UniqueId}) Count={Count}, NBT={ExtraData}";
	}

	public bool Interact(Level level, Player player, Entity target)
	{
		return false; // Not handled
	}

	public static int GetUniqueId()
	{
		Interlocked.CompareExchange(ref _uniqueIdIncrement, 100000, int.MaxValue / 2);
		return Interlocked.Increment(ref _uniqueIdIncrement);
	}
}

public enum ItemMaterial
{
	//Armor Only
	Leather = -2,
	Chain = -1,

	None = 0,
	Wood = 1,
	Stone = 2,
	Gold = 3,
	Iron = 4,
	Diamond = 5,
	Netherite = 6,
	Turtle = 7
}

public enum ItemType
{
	//Tools
	Sword,
	Bow,
	Shovel,
	PickAxe,
	Axe,
	Item,
	Hoe,
	Sheers,
	FlintAndSteel,
	Elytra,
	Trident,
	CarrotOnAStick,
	FishingRod,
	Book,

	//Armor
	Helmet,
	Chestplate,
	Leggings,
	Boots
}

public enum ItemDamageReason
{
	BlockBreak,
	BlockInteract,
	EntityAttack,
	EntityInteract,
	ItemUse
}