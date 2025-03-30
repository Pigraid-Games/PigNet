using PigNet.Items;
using PigNet.Net.Packets.Mcpe;

namespace PigNet.Inventories;

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
		sendMobEquipment.runtimeActorId = Holder.EntityId;
		sendMobEquipment.containerId = WindowId;
		sendMobEquipment.slot = 1;
		sendMobEquipment.item = _item;
		Holder.Level.RelayBroadcast(sendMobEquipment);

		McpeInventorySlot sendSlotUpdate = McpeInventorySlot.CreateObject();
		sendSlotUpdate.containerId = InventoryId;
		sendSlotUpdate.slot = 1;
		sendSlotUpdate.storageItem = _item;
		Holder.SendPacket(sendSlotUpdate);
	}
}