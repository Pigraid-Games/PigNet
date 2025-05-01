using PigNet.Inventories;

namespace PigNet.Net.Packets.Mcpe;

public class McpeCreativeContent : Packet<McpeCreativeContent>
{
	public CreativeInventoryContent content;

	public McpeCreativeContent()
	{
		Id = 0x91;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		Write(content);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		content = ReadCreativeInventoryContent();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();
		content = default;
	}
}