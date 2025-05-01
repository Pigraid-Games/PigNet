
using System.Net;

namespace PigNet.Net.Packets.RakNet;

public class OpenConnectionRequest2 : Packet<OpenConnectionRequest2>
{
	public readonly byte[] offlineMessageDataId = [0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78];
	public long clientGuid;
	public short mtuSize;
	public IPEndPoint remoteBindingAddress;

	public OpenConnectionRequest2()
	{
		Id = 0x07;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(offlineMessageDataId);
		Write(remoteBindingAddress);
		WriteBe(mtuSize);
		Write(clientGuid);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		ReadBytes(offlineMessageDataId.Length);
		remoteBindingAddress = ReadIPEndPoint();
		mtuSize = ReadShortBe();
		clientGuid = ReadLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		remoteBindingAddress = default;
		mtuSize = default;
		clientGuid = default;
	}
}