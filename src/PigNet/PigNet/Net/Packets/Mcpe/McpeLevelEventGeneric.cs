
using PigNet.Utils.Nbt;

namespace PigNet.Net.Packets.Mcpe;

public class McpeLevelEventGeneric : Packet<McpeLevelEventGeneric>
{
	public Nbt eventData;

	public int eventId;

	public McpeLevelEventGeneric()
	{
		Id = 0x7c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(eventId);
		Write(eventData);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		eventId = ReadSignedVarInt();
		//eventData = ReadNbt(); todo wrong
		for (byte i = 0; i < 60; i++) //shhhh
			ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		eventId = default;
		eventData = default;
	}
}