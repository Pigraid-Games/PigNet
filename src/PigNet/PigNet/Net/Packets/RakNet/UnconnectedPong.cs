
namespace PigNet.Net.Packets.RakNet;


public class UnconnectedPong : Packet<UnconnectedPong>
{
	public readonly byte[] offlineMessageDataId = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };

	public long pingId;
	public long serverId;
	public string serverName;

	public UnconnectedPong()
	{
		Id = 0x1c;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(pingId);
		Write(serverId);
		Write(offlineMessageDataId);
		WriteFixedString(serverName);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		pingId = ReadLong();
		serverId = ReadLong();
		ReadBytes(offlineMessageDataId.Length);
		serverName = ReadFixedString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		pingId = default;
		serverId = default;
		serverName = default;
	}
}
