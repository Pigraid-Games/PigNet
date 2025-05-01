
namespace PigNet.Net.Packets.Mcpe;

public class McpeTransfer : Packet<McpeTransfer>
{
	public ushort port;
	public bool reload;
	public string serverAddress;

	public McpeTransfer()
	{
		Id = 0x55;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(serverAddress);
		Write(port);
		Write(reload);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		serverAddress = ReadString();
		port = ReadUshort();
		reload = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		serverAddress = default;
		port = default;
		reload = default;
	}
}