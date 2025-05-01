
namespace PigNet.Net.Packets.Mcpe;

public class McpeSetTime : Packet<McpeSetTime>
{
	public int time;

	public McpeSetTime()
	{
		Id = 0x0a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(time);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		time = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		time = default;
	}
}