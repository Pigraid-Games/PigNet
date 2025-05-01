
namespace PigNet.Net.Packets.Mcpe;

public class McpeServerToClientHandshake : Packet<McpeServerToClientHandshake>
{
	public string token;

	public McpeServerToClientHandshake()
	{
		Id = 0x03;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(token);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		token = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		token = default;
	}
}