
using PigNet.Items;
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeInventoryContent : Packet<McpeInventoryContent>
{
	public uint inventoryId;
	public ItemStacks slots;
	public FullContainerName fullContainerName = new();
	public Item storageItem;

	public McpeInventoryContent()
	{
		Id = 0x31;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(inventoryId);
		Write(slots);
		Write(fullContainerName);
		Write(storageItem);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		inventoryId = ReadUnsignedVarInt();
		slots = ReadItemStacks();
		fullContainerName = ReadFullContainerName();
		storageItem = ReadItem();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		inventoryId = default;
		slots = default;
		storageItem = default;
		fullContainerName = default;
	}
}