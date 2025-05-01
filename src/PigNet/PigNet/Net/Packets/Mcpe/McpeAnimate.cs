
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAnimate : Packet<McpeAnimate>
{
	public AnimatePacketAction actionId;
	public long runtimeActorId;
	public float rowingTime;
	
	public McpeAnimate()
	{
		Id = 0x2c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt((int) actionId);
		WriteUnsignedVarLong(runtimeActorId);
		if (actionId is AnimatePacketAction.RowRight or AnimatePacketAction.RowLeft) rowingTime = ReadFloat();
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		actionId = (AnimatePacketAction) ReadSignedVarInt();
		runtimeActorId = ReadUnsignedVarLong();
		if (actionId is AnimatePacketAction.RowRight or AnimatePacketAction.RowLeft)  Write(rowingTime);
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		actionId = default;
		runtimeActorId = default;
		rowingTime = default;
	}
}