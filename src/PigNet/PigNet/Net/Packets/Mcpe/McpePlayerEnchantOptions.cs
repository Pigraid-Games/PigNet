
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpePlayerEnchantOptions : Packet<McpePlayerEnchantOptions>
{
	public EnchantOptions enchantOptions;

	public McpePlayerEnchantOptions()
	{
		Id = 0x92;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(enchantOptions);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		enchantOptions = ReadEnchantOptions();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		enchantOptions = default;
	}
}