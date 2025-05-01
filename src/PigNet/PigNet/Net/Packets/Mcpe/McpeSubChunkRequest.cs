
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSubChunkRequestPacket : Packet<McpeSubChunkRequestPacket>
{
	public BlockCoordinates basePosition;

	public int dimension;
	public SubChunkPositionOffset[] offsets;

	public McpeSubChunkRequestPacket()
	{
		Id = 0xaf;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteVarInt(dimension);
		Write(basePosition);
		Write(offsets);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		dimension = ReadVarInt();
		basePosition = ReadBlockCoordinates();
		offsets = ReadSubChunkPositionOffsets();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		dimension = default;
		basePosition = default;
		offsets = default;
	}
}