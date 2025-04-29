namespace PigNet.Net.Packets.Mcpe;

public class McpeContainerClose : Packet<McpeContainerClose>
{
	public bool server;
	public byte containerId;
	public sbyte containerType;

	public McpeContainerClose()
	{
		Id = 0x2f;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(containerId);
		Write((byte) containerType);
		Write(server);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		containerId = ReadByte();
		containerType = ReadSByte();
		server = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		containerId = default;
		containerType = default;
		server = default;
	}
}