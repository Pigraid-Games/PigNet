
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeShowStoreOffer : Packet<McpeShowStoreOffer>
{
	public string productId;
	public ShowStoreOfferRedirectType redirectType;

	public McpeShowStoreOffer()
	{
		Id = 0x5b;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(productId);
		Write((byte) redirectType);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		productId = ReadString();
		redirectType = (ShowStoreOfferRedirectType) ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		productId = default;
		redirectType = default;
	}
}