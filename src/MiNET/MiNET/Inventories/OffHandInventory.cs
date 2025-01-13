using log4net;
using MiNET.Items;
using MiNET.Net;

namespace MiNET.Inventories;

public class OffHandInventory
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(OffHandInventory));

	public static readonly byte WindowId = 119;
	public static readonly byte InventoryId = 34;

	private Item Item = new ItemAir();
	public Player Holder { get; set; }

	public OffHandInventory(Player player)
	{
		Holder = player;
	}

	public Item GetItem() => Item;

	public void SetItem(Item item, bool sendUpdate = true)
	{
		Item = item;
		if (sendUpdate)
			SendUpdate();
	}

	public void SendUpdate()
	{
		var sendMobEquipment = McpeMobEquipment.CreateObject();
		sendMobEquipment.runtimeEntityId = Holder.EntityId;
		sendMobEquipment.selectedSlot = 0;
		sendMobEquipment.slot = 0;
		sendMobEquipment.windowsId = WindowId;
		sendMobEquipment.item = Item;
		Holder.Level.RelayBroadcast(sendMobEquipment);

		var sendSlotUpdate = McpeInventorySlot.CreateObject();
		sendSlotUpdate.inventoryId = InventoryId;
		sendSlotUpdate.slot = 0;
		sendSlotUpdate.storageItem = Item;
		Holder.SendPacket(sendSlotUpdate);
	}
}