
namespace PigNet.Net.Packets.Mcpe;

public class McpeLogin : Packet<McpeLogin>
{
	public byte[] payload;
	public int protocolVersion;

	public McpeLogin()
	{
		Id = 0x01;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteBe(protocolVersion);
		WriteByteArray(payload);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		protocolVersion = ReadIntBe();
		payload = ReadByteArray();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		protocolVersion = default;
		payload = default;
	}
}