
namespace PigNet.Net.Packets.RakNet;

public class ConnectedPing : Packet<ConnectedPing>
{
	public long sendpingtime;

	public ConnectedPing()
	{
		Id = 0x00;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(sendpingtime);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		sendpingtime = ReadLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		sendpingtime = default;
	}
}