
namespace PigNet.Net.Packets.Mcpe;

public class McpeBlockPickRequest : Packet<McpeBlockPickRequest>
{
	public bool addUserData;
	public byte selectedSlot;
	public int x;
	public int y;
	public int z;

	public McpeBlockPickRequest()
	{
		Id = 0x22;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(x);
		WriteSignedVarInt(y);
		WriteSignedVarInt(z);
		Write(addUserData);
		Write(selectedSlot);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		x = ReadSignedVarInt();
		y = ReadSignedVarInt();
		z = ReadSignedVarInt();
		addUserData = ReadBool();
		selectedSlot = ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		x = default;
		y = default;
		z = default;
		addUserData = default;
		selectedSlot = default;
	}
}