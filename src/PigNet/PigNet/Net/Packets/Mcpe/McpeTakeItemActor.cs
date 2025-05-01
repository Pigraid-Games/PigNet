
namespace PigNet.Net.Packets.Mcpe;

public class McpeTakeItemActor : Packet<McpeTakeItemActor>
{
	public long runtimeActorId;
	public long target;

	public McpeTakeItemActor()
	{
		Id = 0x11;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		WriteUnsignedVarLong(target);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		target = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		target = default;
	}
}