
namespace PigNet.Net.Packets.Mcpe;

public class McpeRequestNetworkSettings : Packet<McpeRequestNetworkSettings>
{
	public int protocolVersion;

	public McpeRequestNetworkSettings()
	{
		Id = 0xc1;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteBe(protocolVersion);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		protocolVersion = ReadIntBe();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		protocolVersion = default;
	}
}