
namespace PigNet.Net.Packets.Mcpe;

public class McpeRequestChunkRadius : Packet<McpeRequestChunkRadius>
{
	public int chunkRadius;
	public byte maxRadius;

	public McpeRequestChunkRadius()
	{
		Id = 0x45;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(chunkRadius);
		Write(maxRadius);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		chunkRadius = ReadSignedVarInt();
		maxRadius = ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		chunkRadius = default;
		maxRadius = default;
	}
}