
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeActorEvent : Packet<McpeActorEvent>
{
	public int data;
	public ActorEvent eventId;

	public long runtimeEntityId;

	public McpeActorEvent()
	{
		Id = 0x1b;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeEntityId);
		Write((byte) eventId);
		WriteSignedVarInt(data);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeEntityId = ReadUnsignedVarLong();
		eventId = (ActorEvent) ReadByte();
		data = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeEntityId = default;
		eventId = default;
		data = default;
	}
}