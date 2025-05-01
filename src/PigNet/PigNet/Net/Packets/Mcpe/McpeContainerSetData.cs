
namespace PigNet.Net.Packets.Mcpe;

public class McpeContainerSetData : Packet<McpeContainerSetData>
{
	public byte containerId;
	public int id;
	public int value;

	public McpeContainerSetData()
	{
		Id = 0x33;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(containerId);
		WriteSignedVarInt(id);
		WriteSignedVarInt(value);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		containerId = ReadByte();
		id = ReadSignedVarInt();
		value = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		containerId = default;
		id = default;
		value = default;
	}
}