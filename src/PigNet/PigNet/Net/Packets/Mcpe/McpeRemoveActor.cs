
namespace PigNet.Net.Packets.Mcpe;

public class McpeRemoveActor : Packet<McpeRemoveActor>
{
	public long entityIdSelf;

	public McpeRemoveActor()
	{
		Id = 0x0e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(entityIdSelf);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entityIdSelf = ReadSignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entityIdSelf = default;
	}
}