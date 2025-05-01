
namespace PigNet.Net.Packets.Mcpe;

public class McpeInitiateWebSocketConnection : Packet<McpeInitiateWebSocketConnection>
{
	public string server;

	public McpeInitiateWebSocketConnection()
	{
		Id = 0x5f;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(server);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		server = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		server = default;
	}
}