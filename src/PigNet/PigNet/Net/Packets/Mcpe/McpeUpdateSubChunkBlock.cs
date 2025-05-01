
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateSubChunkBlocks : Packet<McpeUpdateSubChunkBlocks>
{
	public UpdateSubChunkBlocksPacketEntry[] layerOneUpdates;
	public UpdateSubChunkBlocksPacketEntry[] layerZeroUpdates;

	public BlockCoordinates subchunkCoordinates;

	public McpeUpdateSubChunkBlocks()
	{
		Id = 0xac;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(subchunkCoordinates);
		Write(layerZeroUpdates);
		Write(layerOneUpdates);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		subchunkCoordinates = ReadBlockCoordinates();
		layerZeroUpdates = ReadUpdateSubChunkBlocksPacketEntrys();
		layerOneUpdates = ReadUpdateSubChunkBlocksPacketEntrys();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		subchunkCoordinates = default;
		layerZeroUpdates = default;
		layerOneUpdates = default;
	}
}