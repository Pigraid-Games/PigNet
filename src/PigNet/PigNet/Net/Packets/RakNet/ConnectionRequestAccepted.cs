
using System.Net;

namespace PigNet.Net.Packets.RakNet;

public class ConnectionRequestAccepted : Packet<ConnectionRequestAccepted>
{
	public long incomingTimestamp;
	public long serverTimestamp;
	public IPEndPoint systemAddress;
	public IPEndPoint[] systemAddresses;
	public short systemIndex;

	public ConnectionRequestAccepted()
	{
		Id = 0x10;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(systemAddress);
		WriteBe(systemIndex);
		Write(systemAddresses);
		Write(incomingTimestamp);
		Write(serverTimestamp);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		systemAddress = ReadIPEndPoint();
		systemIndex = ReadShortBe();
		systemAddresses = ReadIPEndPoints(20);
		incomingTimestamp = ReadLong();
		serverTimestamp = ReadLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		systemAddress = default;
		systemIndex = default;
		systemAddresses = default;
		incomingTimestamp = default;
		serverTimestamp = default;
	}
}