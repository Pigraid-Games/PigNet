
namespace PigNet.Net.Packets.RakNet;

public class ConnectionRequest : Packet<ConnectionRequest>
{
	public long clientGuid;
	public byte doSecurity;
	public long timestamp;

	public ConnectionRequest()
	{
		Id = 0x09;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(clientGuid);
		Write(timestamp);
		Write(doSecurity);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		clientGuid = ReadLong();
		timestamp = ReadLong();
		doSecurity = ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		clientGuid = default;
		timestamp = default;
		doSecurity = default;
	}
}