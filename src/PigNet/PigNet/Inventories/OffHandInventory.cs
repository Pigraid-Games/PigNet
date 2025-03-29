using PigNet.Items;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;

namespace PigNet.Inventories;

public class OffHandInventory(Player player)
{
	private const byte ContainerId = (byte)Net.EnumerationsTable.ContainerId.Offhand;
	private const byte ContainerType = (byte)ContainerEnumName.OffhandContainer;

	private Item _item = new ItemAir();
	public Player Holder { get; } = player;

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
		sendMobEquipment.selectedSlot = 0;
		sendMobEquipment.slot = 0;
		sendMobEquipment.containerId = ContainerId;
		sendMobEquipment.item = _item;
		Holder.Level.RelayBroadcast(Holder, sendMobEquipment);

		McpeInventorySlot sendSlotUpdate = McpeInventorySlot.CreateObject();
		sendSlotUpdate.containerId = ContainerType;
		sendSlotUpdate.slot = 0;
		sendSlotUpdate.storageItem = _item;
		Holder.SendPacket(sendSlotUpdate);
	}
}