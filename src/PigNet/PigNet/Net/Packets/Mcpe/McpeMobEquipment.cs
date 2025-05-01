
using PigNet.Items;

namespace PigNet.Net.Packets.Mcpe;

public class McpeMobEquipment : Packet<McpeMobEquipment>
{
	public long runtimeActorId;
	public Item item;
	public byte slot;
	public byte selectedSlot;
	public byte containerId;

	public McpeMobEquipment()
	{
		Id = 0x1f;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write(item);
		Write(slot);
		Write(selectedSlot);
		Write(containerId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		item = ReadItem();
		slot = ReadByte();
		selectedSlot = ReadByte();
		containerId = ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		item = default;
		slot = default;
		selectedSlot = default;
		containerId = default;
	}
}