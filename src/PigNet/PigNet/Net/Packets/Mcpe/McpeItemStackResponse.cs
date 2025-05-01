
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeItemStackResponse : Packet<McpeItemStackResponse>
{
	public ItemStackResponses responses;

	public McpeItemStackResponse()
	{
		Id = 0x94;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(responses);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		responses = ReadItemStackResponses();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		responses = default;
	}
}