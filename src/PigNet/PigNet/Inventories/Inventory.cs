using System;
using System.Collections.Concurrent;
using System.Linq;
using log4net;
using PigNet.Items;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Inventories;

public interface IInventory
{
	public WindowType Type { get; }
	public ItemStacks Slots { get; }
	public WindowId WindowId { get; }

	public long RuntimeEntityId { get; }
	public BlockCoordinates Coordinates { get; }

	public bool IsOpen { get; }

	public bool Open(Player player);
	public bool Close(Player player, bool closedByPlayer = false);
	public void Clear();
}

public class ContainerInventory : CommonInventory
{
	public ContainerInventory(ItemStacks items, long runtimeEntityId)
		: base(items, default, runtimeEntityId)
	{
	}

	public ContainerInventory(ItemStacks items, BlockCoordinates coordinates)
		: base(items, coordinates, EntityManager.EntityIdUndefined)
	{
	}

	public override bool IsOpen => Observers.Any();

	// Below is a workaround making it possible to send
	// updates to only peopele that is looking at this inventory.
	// Is should be converted to some sort of event based version.

	public ConcurrentBag<Player> Observers { get; } = [];
	public event EventHandler<InventoryChangeEventArgs> InventoryChanged;

	public virtual void SetSlot(Player player, byte slot, Item itemStack)
	{
		Slots[slot] = itemStack;

		OnInventoryChange(player, slot, itemStack);
		BroadcastSetSlot(player, slot);
	}

	public virtual Item GetSlot(byte slot)
	{
		return Slots[slot];
	}

	public bool DecreaseSlot(byte slot)
	{
		Item slotData = Slots[slot];
		if (slotData is ItemAir) return false;
		byte count = slotData.Count;

		slotData.Count--;

		if (slotData.Count <= 0) slotData = new ItemAir();

		SetSlot(null, slot, slotData);

		if (count <= 0) return false;

		OnInventoryChange(null, slot, slotData);
		BroadcastSetSlot(slot);
		return true;
	}

	public void IncreaseSlot(byte slot, string id, short metadata)
	{
		Item slotData = Slots[slot];
		if (slotData is ItemAir) slotData = ItemFactory.GetItem(id, metadata);
		else slotData.Count++;

		SetSlot(null, slot, slotData);

		OnInventoryChange(null, slot, slotData);
	}

	public virtual void Close()
	{
		foreach (Player observer in Observers.ToArray()) Close(observer);
	}

	public override void Clear()
	{
		base.Clear();

		foreach (Player observer in Observers) SendContent(observer);
	}

	protected virtual void BroadcastSetSlot(int slot)
	{
		BroadcastSetSlot(null, slot);
	}

	protected virtual void BroadcastSetSlot(Player sender, int slot)
	{
		Item item = Slots[slot];
		foreach (Player observer in Observers)
		{
			if (observer == sender) continue;
			SendSetSlot(observer, slot, item, WindowId);
		}
	}

	protected override bool OnInventoryOpen(Player player, bool open)
	{
		bool opened = base.OnInventoryOpen(player, open);
		if (opened) AddObserver(player);
		return opened;
	}

	protected override void OnInventoryClose(Player player)
	{
		base.OnInventoryClose(player);

		RemoveObserver(player);
	}

	protected virtual void AddObserver(Player player)
	{
		Observers.Add(player);
	}

	protected virtual void RemoveObserver(Player player)
	{
		// Need to arrange for this to work when players get disconnected
		// from crash. It will leak players for sure.
		Observers.TryTake(out player);
	}

	protected virtual void OnInventoryChange(Player player, byte slot, Item itemStack)
	{
		InventoryChanged?.Invoke(this, new InventoryChangeEventArgs(player, this, slot, itemStack));
	}
}

public class Inventory : CommonInventory
{
	private bool _isOpen;

	public Inventory(BlockCoordinates coordinates, WindowType type) : this(new ItemStacks(0), coordinates, type)
	{
	}

	public Inventory(ItemStacks items, BlockCoordinates coordinates, WindowType type) : base(items, coordinates, EntityManager.EntityIdSelf)
	{
		Type = type;
	}

	public Inventory(int size, long runtimeEntityId, WindowType type) : base(ItemStacks.CreateAir(size), default, runtimeEntityId)
	{
		Type = type;
	}

	public override bool IsOpen => _isOpen;

	protected override bool OnInventoryOpen(Player player, bool open)
	{
		if (_isOpen) return false;
		bool opened = base.OnInventoryOpen(player, open);
		if (opened) _isOpen = true;
		return opened;
	}

	protected override void OnInventoryClose(Player player)
	{
		base.OnInventoryClose(player);
		_isOpen = false;
	}
}

public abstract class CommonInventory : IInventory, PigNet.IInventory
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Inventory));

	private static byte LastWindowId;

	protected CommonInventory(ItemStacks items, BlockCoordinates coordinates, long runtimeEntityId)
	{
		Slots = items;
		Coordinates = coordinates;
		RuntimeEntityId = runtimeEntityId;
	}

	public WindowType Type { get; set; }
	public virtual ItemStacks Slots { get; set; }
	public WindowId WindowId { get; set; } = GetNewWindowId();

	public long RuntimeEntityId { get; set; }
	public BlockCoordinates Coordinates { get; set; }

	public abstract bool IsOpen { get; }

	public virtual bool Open(Player player)
	{
		PigNet.IInventory openedInventory = player.GetOpenInventory();

		if (openedInventory != null) player.CloseOpenedInventory();

		bool open = !IsOpen;
		if (!OnInventoryOpen(player, open)) return false;

		player.SetOpenInventory(this);

		SendOpen(player);
		SendContent(player);

		OnInventoryOpened(player, open);

		return true;
	}

	public virtual bool Close(Player player, bool closedByPlayer = false)
	{
		PigNet.IInventory openedInventory = player.GetOpenInventory();

		OnInventoryClose(player);

		player.SetOpenInventory(null);

		SendClose(player, closedByPlayer);

		player.Inventory.CloseUiInventory();

		OnInventoryClosed(player, !IsOpen);

		return true;
	}

	public virtual void Clear()
	{
		Slots.Reset();
	}

	public event EventHandler<InventoryOpenEventArgs> InventoryOpen;
	public event EventHandler<InventoryOpenedEventArgs> InventoryOpened;
	public event EventHandler<InventoryEventArgs> InventoryClose;
	public event EventHandler<InventoryClosedEventArgs> InventoryClosed;

	public void SendContent(Player player)
	{
		McpeInventoryContent containerSetContent = McpeInventoryContent.CreateObject();
		containerSetContent.inventoryId = (byte) WindowId;
		containerSetContent.slots = Slots;
		containerSetContent.fullContainerName = FullContainerName.Unknown;
		player.SendPacket(containerSetContent);
	}

	protected virtual void SendOpen(Player player)
	{
		McpeContainerOpen containerOpen = McpeContainerOpen.CreateObject();
		containerOpen.containerId = (byte) WindowId;
		containerOpen.containerType = (sbyte) Type;
		containerOpen.position = Coordinates;
		containerOpen.runtimeActorId = RuntimeEntityId;
		player.SendPacket(containerOpen);
	}

	protected virtual void SendClose(Player player, bool closedByPlayer)
	{
		McpeContainerClose closePacket = McpeContainerClose.CreateObject();
		closePacket.containerId = (byte) WindowId;
		closePacket.containerType = (sbyte) Type;
		closePacket.server = !closedByPlayer;
		player.SendPacket(closePacket);
	}

	protected virtual void SendSetSlot(Player player, int slot)
	{
		SendSetSlot(player, slot, Slots[slot], WindowId);
	}

	protected virtual void SendSetSlot(Player player, int slot, Item item, WindowId windowId)
	{
		McpeInventorySlot sendSlot = McpeInventorySlot.CreateObject();
		sendSlot.containerId = (uint) windowId;
		sendSlot.slot = (uint) slot;
		sendSlot.item = item;
		sendSlot.fullContainerName = FullContainerName.Unknown;
		player.SendPacket(sendSlot);
	}

	protected virtual bool OnInventoryOpen(Player player, bool open)
	{
		var args = new InventoryOpenEventArgs(player, this, open);
		InventoryOpen?.Invoke(this, args);

		return !args.Cancel;
	}

	protected virtual void OnInventoryOpened(Player player, bool opened)
	{
		InventoryOpened?.Invoke(this, new InventoryOpenedEventArgs(player, this, opened));
	}

	protected virtual void OnInventoryClose(Player player)
	{
		InventoryClose?.Invoke(this, new InventoryEventArgs(player, this));
	}

	protected virtual void OnInventoryClosed(Player player, bool closed)
	{
		InventoryClosed?.Invoke(this, new InventoryClosedEventArgs(player, this, closed));
	}

	private static WindowId GetNewWindowId()
	{
		return (WindowId) (LastWindowId = (byte) Math.Max((byte) WindowId.First, ++LastWindowId % (byte) WindowId.Last));
	}
}