
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpePlayerFog : Packet<McpePlayerFog>
{
	public FogStack fogstack;

	public McpePlayerFog()
	{
		Id = 0xa0;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(fogstack);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		fogstack = Read();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		fogstack = default;
	}
}