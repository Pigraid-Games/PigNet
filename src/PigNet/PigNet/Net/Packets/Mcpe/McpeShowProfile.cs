
namespace PigNet.Net.Packets.Mcpe;

public class McpeShowProfile : Packet<McpeShowProfile>
{
	public string xuid;

	public McpeShowProfile()
	{
		Id = 0x68;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(xuid);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		xuid = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		xuid = default;
	}
}