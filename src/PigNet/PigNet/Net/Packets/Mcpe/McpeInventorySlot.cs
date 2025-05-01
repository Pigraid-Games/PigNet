using PigNet.Items;

namespace PigNet.Net.Packets.Mcpe;

public class McpeInventorySlot : Packet<McpeInventorySlot>
{
	public uint containerId;
	public uint slot;
	public FullContainerName fullContainerName = new();
	public Item storageItem;
	public Item item;

	public McpeInventorySlot()
	{
		Id = 0x32;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(containerId);
		WriteUnsignedVarInt(slot);
		Write(fullContainerName);
		Write(storageItem);
		Write(item);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		containerId = ReadUnsignedVarInt();
		slot = ReadUnsignedVarInt();
		fullContainerName = ReadFullContainerName();
		storageItem = ReadItem();
		item = ReadItem();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		containerId = default;
		slot = default;
		fullContainerName = default;
		storageItem = default;
		item = default;
	}
}