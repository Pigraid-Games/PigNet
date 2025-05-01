
namespace PigNet.Net.Packets.Mcpe;

public class McpeStopSound : Packet<McpeStopSound>
{
	public string name;
	public bool stopAll;

	public McpeStopSound()
	{
		Id = 0x57;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(name);
		Write(stopAll);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		name = ReadString();
		stopAll = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		name = default;
		stopAll = default;
	}
}