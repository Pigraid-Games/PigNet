
using PigNet.Net.RakNet;

namespace PigNet.Net.Packets.RakNet;

public class OpenConnectionRequest1 : Packet<OpenConnectionRequest1>
{
	public readonly byte[] offlineMessageDataId = [0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78];
	public byte raknetProtocolVersion;
	public short mtuSize;
	
	public OpenConnectionRequest1()
	{
		Id = 0x05;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		mtuSize = (short) (_reader.Length + RakOfflineHandler.UdpHeaderSize);
		ReadBytes((int) (_reader.Length - _reader.Position));
		Write(offlineMessageDataId);
		Write(raknetProtocolVersion);
		Write(new byte[mtuSize - _buffer.Position - RakOfflineHandler.UdpHeaderSize]);
	}
	
	protected override void DecodePacket()
	{
		base.DecodePacket();

		ReadBytes(offlineMessageDataId.Length);
		raknetProtocolVersion = ReadByte();
		mtuSize = (short) (_reader.Length + RakOfflineHandler.UdpHeaderSize);
		ReadBytes((int) (_reader.Length - _reader.Position));
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		raknetProtocolVersion = default;
	}
}