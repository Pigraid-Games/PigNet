
namespace PigNet.Net.Packets.Mcpe;

public class McpeSetLastHurtBy : Packet<McpeSetLastHurtBy>
{
	public int lastHurtBy;

	public McpeSetLastHurtBy()
	{
		Id = 0x60;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteVarInt(lastHurtBy);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		lastHurtBy = ReadVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		lastHurtBy = default;
	}
}