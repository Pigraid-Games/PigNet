using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeItemComponent : Packet<McpeItemComponent>
{
	public ItemStates entries;

	public McpeItemComponent()
	{
		Id = 0xa2;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(entries);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entries = ReadItemStates();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entries = default;
	}
}