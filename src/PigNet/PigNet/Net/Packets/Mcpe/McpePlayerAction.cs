
using PigNet.Net.EnumerationsTable;
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpePlayerAction : Packet<McpePlayerAction>
{
	public PlayerActionType actionId;
	public BlockCoordinates coordinates;
	public int face;
	public BlockCoordinates resultCoordinates;

	public long runtimeActorId;

	public McpePlayerAction()
	{
		Id = 0x24;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		WriteSignedVarInt((int) actionId);
		Write(coordinates);
		Write(resultCoordinates);
		WriteSignedVarInt(face);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		actionId = (PlayerActionType)ReadSignedVarInt();
		coordinates = ReadBlockCoordinates();
		resultCoordinates = ReadBlockCoordinates();
		face = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		actionId = default;
		coordinates = default;
		resultCoordinates = default;
		face = default;
	}
}