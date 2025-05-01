
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetDefaultGameType : Packet<McpeSetDefaultGameType>
{
	public GameType gamemode;

	public McpeSetDefaultGameType()
	{
		Id = 0x69;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteVarInt((int) gamemode);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		gamemode = (GameType) ReadVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		gamemode = default;
	}
}