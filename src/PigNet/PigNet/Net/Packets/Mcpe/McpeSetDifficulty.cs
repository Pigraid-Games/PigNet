
using PigNet.Net.EnumerationsTable;
using PigNet.Worlds;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetDifficulty : Packet<McpeSetDifficulty>
{
	public uint difficulty;

	public McpeSetDifficulty()
	{
		Id = 0x3c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(difficulty);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		difficulty = ReadUnsignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		difficulty = default;
	}
}