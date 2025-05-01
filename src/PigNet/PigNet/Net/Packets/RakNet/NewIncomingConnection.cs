
using System.Net;

namespace PigNet.Net.Packets.RakNet;

public class NewIncomingConnection : Packet<NewIncomingConnection>
{
	public IPEndPoint clientendpoint;
	public long incomingTimestamp;
	public long serverTimestamp;
	public IPEndPoint[] systemAddresses;

	public NewIncomingConnection()
	{
		Id = 0x13;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(clientendpoint);
		Write(systemAddresses);
		Write(incomingTimestamp);
		Write(serverTimestamp);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		clientendpoint = ReadIPEndPoint();
		systemAddresses = ReadIPEndPoints(20);
		incomingTimestamp = ReadLong();
		serverTimestamp = ReadLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		clientendpoint = default;
		systemAddresses = default;
		incomingTimestamp = default;
		serverTimestamp = default;
	}
}