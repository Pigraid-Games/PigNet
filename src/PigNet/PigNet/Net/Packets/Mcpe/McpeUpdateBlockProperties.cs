
using PigNet.Utils.Nbt;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateBlockProperties : Packet<McpeUpdateBlockProperties>
{
	public Nbt namedtag;

	public McpeUpdateBlockProperties()
	{
		Id = 0x86;
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