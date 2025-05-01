
namespace PigNet.Net.Packets.Mcpe;

public class McpeEmoteList : Packet<McpeEmoteList>
{
	public long runtimeActorId;
	public EmoteIds emoteIds;

	public McpeEmoteList()
	{
		Id = 0x8a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write(emoteIds);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		emoteIds = ReadEmoteId();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		emoteIds = default;
	}
}