
namespace PigNet.Net.Packets.Mcpe;

public class McpeResourcePackChunkData : Packet<McpeResourcePackChunkData>
{
	public uint chunkIndex;

	public string packageId;
	public byte[] payload;
	public ulong progress;

	public McpeResourcePackChunkData()
	{
		Id = 0x53;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(packageId);
		Write(chunkIndex);
		Write(progress);
		WriteByteArray(payload);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		packageId = ReadString();
		chunkIndex = ReadUint();
		progress = ReadUlong();
		payload = ReadByteArray();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		packageId = default;
		chunkIndex = default;
		progress = default;
		payload = default;
	}
}