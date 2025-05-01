
namespace PigNet.Net.Packets.Mcpe;

public class McpeGameRulesChanged : Packet<McpeGameRulesChanged>
{
	public GameRules rules;

	public McpeGameRulesChanged()
	{
		Id = 0x48;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(rules);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		rules = ReadGameRules();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		rules = default;
	}
}