
namespace PigNet.Net.Packets.RakNet;

public class ConnectionBanned : Packet<ConnectionBanned>
{
	public readonly byte[] offlineMessageDataId = [0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78];
	public long serverGuid;

	public ConnectionBanned()
	{
		Id = 0x17;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(offlineMessageDataId);
		Write(serverGuid);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		ReadBytes(offlineMessageDataId.Length);
		serverGuid = ReadLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		serverGuid = default;
	}
}