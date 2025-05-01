
namespace PigNet.Net.Packets.Mcpe;

public class McpePlayerHotbar : Packet<McpePlayerHotbar>
{
	public uint selectedSlot;
	public byte containerId;
	public bool shouldSelectSlot;

	public McpePlayerHotbar()
	{
		Id = 0x30;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(selectedSlot);
		Write(containerId);
		Write(shouldSelectSlot);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		selectedSlot = ReadUnsignedVarInt();
		containerId = ReadByte();
		shouldSelectSlot = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		selectedSlot = default;
		containerId = default;
		shouldSelectSlot = default;
	}
}