
namespace PigNet.Net.Packets.Mcpe;

public class McpeShowCredits : Packet<McpeShowCredits>
{
	public long runtimeActorId;
	public int creditState;

	public McpeShowCredits()
	{
		Id = 0x4b;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		WriteSignedVarInt(creditState);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		creditState = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		creditState = default;
	}
}