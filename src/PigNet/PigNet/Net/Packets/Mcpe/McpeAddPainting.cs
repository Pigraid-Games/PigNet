
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAddPainting : Packet<McpeAddPainting>
{
	public BlockCoordinates coordinates;
	public int direction;
	public long entityIdSelf;
	public long runtimeActorId;
	public string title;

	public McpeAddPainting()
	{
		Id = 0x16;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(entityIdSelf);
		WriteUnsignedVarLong(runtimeActorId);
		Write(coordinates);
		WriteSignedVarInt(direction);
		Write(title);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entityIdSelf = ReadSignedVarLong();
		runtimeActorId = ReadUnsignedVarLong();
		coordinates = ReadBlockCoordinates();
		direction = ReadSignedVarInt();
		title = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entityIdSelf = default;
		runtimeActorId = default;
		coordinates = default;
		direction = default;
		title = default;
	}
}