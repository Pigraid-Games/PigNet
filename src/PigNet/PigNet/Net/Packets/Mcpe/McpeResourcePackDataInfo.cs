
namespace PigNet.Net.Packets.Mcpe;

public class McpeResourcePackDataInfo : Packet<McpeResourcePackDataInfo>
{
	public uint chunkCount;
	public ulong compressedPackageSize;
	public byte[] hash;
	public bool isPremium;
	public uint maxChunkSize;

	public string packageId;
	public byte packType;

	public McpeResourcePackDataInfo()
	{
		Id = 0x52;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(packageId);
		Write(maxChunkSize);
		Write(chunkCount);
		Write(compressedPackageSize);
		WriteByteArray(hash);
		Write(isPremium);
		Write(packType);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		packageId = ReadString();
		maxChunkSize = ReadUint();
		chunkCount = ReadUint();
		compressedPackageSize = ReadUlong();
		hash = ReadByteArray();
		isPremium = ReadBool();
		packType = ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		packageId = default;
		maxChunkSize = default;
		chunkCount = default;
		compressedPackageSize = default;
		hash = default;
		isPremium = default;
		packType = default;
	}
}