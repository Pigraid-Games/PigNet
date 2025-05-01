
namespace PigNet.Net.Packets.RakNet;

public class OpenConnectionReply1 : Packet<OpenConnectionReply1>
{
	public readonly byte[] offlineMessageDataId = [0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78];
	public short mtuSize;
	public long serverGuid;
	public byte serverHasSecurity;

	public OpenConnectionReply1()
	{
		Id = 0x06;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(offlineMessageDataId);
		Write(serverGuid);
		Write(serverHasSecurity);
		WriteBe(mtuSize);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		ReadBytes(offlineMessageDataId.Length);
		serverGuid = ReadLong();
		serverHasSecurity = ReadByte();
		mtuSize = ReadShortBe();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		serverGuid = default;
		serverHasSecurity = default;
		mtuSize = default;
	}
}