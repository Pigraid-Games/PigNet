
namespace PigNet.Net.Packets.Mcpe;

public class McpeSetHealth : Packet<McpeSetHealth>
{
	public int health;

	public McpeSetHealth()
	{
		Id = 0x2a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(health);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		health = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		health = default;
	}
}