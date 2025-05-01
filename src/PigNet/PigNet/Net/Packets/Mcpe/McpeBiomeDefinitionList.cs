
using PigNet.Utils.Nbt;

namespace PigNet.Net.Packets.Mcpe;

public class McpeBiomeDefinitionList : Packet<McpeBiomeDefinitionList>
{
	public Nbt namedtag;

	public McpeBiomeDefinitionList()
	{
		Id = 0x7a;
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