
using PigNet.Utils.Nbt;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAvailableEntityIdentifiers : Packet<McpeAvailableEntityIdentifiers>
{
	public Nbt namedtag;

	public McpeAvailableEntityIdentifiers()
	{
		Id = 0x77;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(namedtag);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		namedtag = ReadNbt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		namedtag = default;
	}
}