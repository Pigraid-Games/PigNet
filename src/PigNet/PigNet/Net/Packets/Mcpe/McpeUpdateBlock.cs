
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateBlock : Packet<McpeUpdateBlock>
{

	public uint blockPriority;
	public uint blockRuntimeId;
	public BlockCoordinates coordinates;
	public uint storage;

	public McpeUpdateBlock()
	{
		Id = 0x15;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(coordinates);
		WriteUnsignedVarInt(blockRuntimeId);
		WriteUnsignedVarInt(blockPriority);
		WriteUnsignedVarInt(storage);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		coordinates = ReadBlockCoordinates();
		blockRuntimeId = ReadUnsignedVarInt();
		blockPriority = ReadUnsignedVarInt();
		storage = ReadUnsignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		coordinates = default;
		blockRuntimeId = default;
		blockPriority = default;
		storage = default;
	}
}