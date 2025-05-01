
namespace PigNet.Net.Packets.Mcpe;

public class McpeSetPlayerGameType : Packet<McpeSetPlayerGameType>
{
	public int gamemode;

	public McpeSetPlayerGameType()
	{
		Id = 0x3e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(gamemode);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		gamemode = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		gamemode = default;
	}
}