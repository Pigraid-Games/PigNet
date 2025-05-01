
namespace PigNet.Net.Packets.Mcpe;

public class McpeSetLocalPlayerAsInitialized : Packet<McpeSetLocalPlayerAsInitialized>
{
	public long runtimeActorId;

	public McpeSetLocalPlayerAsInitialized()
	{
		Id = 0x71;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
	}
}