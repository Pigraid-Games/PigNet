
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSimpleEvent : Packet<McpeSimpleEvent>
{
	public SimpleEventPacketSubType eventType;

	public McpeSimpleEvent()
	{
		Id = 0x40;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((uint) eventType);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		eventType = (SimpleEventPacketSubType) ReadUshort();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		eventType = default;
	}
}