
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeNetworkChunkPublisherUpdate : Packet<McpeNetworkChunkPublisherUpdate>
{
	public BlockCoordinates coordinates;
	public uint radius;
	public int savedChunks;
	public uint x;
	public uint z;

	public McpeNetworkChunkPublisherUpdate()
	{
		Id = 0x79;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(coordinates);
		WriteUnsignedVarInt(radius);
		Write(savedChunks);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		coordinates = ReadBlockCoordinates();
		radius = ReadUnsignedVarInt();
		savedChunks = ReadInt();
		for (int i = 0; i < savedChunks; i++)
		{
			x = ReadUnsignedVarInt();
			z = ReadUnsignedVarInt();
			//todo saved chunk list
		}
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		coordinates = default;
		radius = default;
		savedChunks = default;
		x = default(int);
		z = default(int);
	}
}