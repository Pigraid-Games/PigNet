
namespace PigNet.Net.Packets.Mcpe;

public class McpePlayStatus : Packet<McpePlayStatus>
{
	public int status;

	public McpePlayStatus()
	{
		Id = 0x02;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteBe(status);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		status = ReadIntBe();
	}
	
	protected override void ResetPacket()
	{
		base.ResetPacket();

		status = default;
	}
}