using MiNET.Items;
using MiNET.Net;

namespace MiNET.Inventories;

public class OffHandInventory(Player player)
{
	public static readonly byte WindowId = 119;
	public static readonly byte InventoryId = 34;

	private Item _item = new ItemAir();
	public Player Holder { get; set; } = player;

	public Item GetItem() => _item;

	public void SetItem(Item item, bool sendUpdate = true)
	{
		_item = item;
		if (sendUpdate)
			SendUpdate();
	}

	public void SendUpdate()
	{
		McpeMobEquipment sendMobEquipment = McpeMobEquipment.CreateObject();
		sendMobEquipment.runtimeEntityId = Holder.EntityId;
		sendMobEquipment.selectedSlot = 0;
		sendMobEquipment.slot = 0;
		sendMobEquipment.windowsId = WindowId;
		sendMobEquipment.item = _item;
		Holder.Level.RelayBroadcast(sendMobEquipment);

		McpeInventorySlot sendSlotUpdate = McpeInventorySlot.CreateObject();
		sendSlotUpdate.inventoryId = InventoryId;
		sendSlotUpdate.slot = 0;
		sendSlotUpdate.storageItem = _item;
		Holder.SendPacket(sendSlotUpdate);
	}
}