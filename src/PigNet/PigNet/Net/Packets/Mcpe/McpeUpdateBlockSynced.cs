
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateBlockSynced : Packet<McpeUpdateBlockSynced>
{
	public BlockCoordinates blockPosition;
	public uint blockRuntimeId;
	public uint flags;
	public uint dataLayerId;
	public long runtimeEntityId;
	public long runtimeEntitySyncMessageId;

	public McpeUpdateBlockSynced()
	{
		Id = 0x6e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(blockPosition);
		WriteUnsignedVarInt(blockRuntimeId);
		WriteUnsignedVarInt(flags);
		WriteUnsignedVarInt(dataLayerId);
		WriteUnsignedVarLong(runtimeEntityId);
		WriteUnsignedVarLong(runtimeEntitySyncMessageId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		blockPosition = ReadBlockCoordinates();
		blockRuntimeId = ReadUnsignedVarInt();
		flags = ReadUnsignedVarInt();
		dataLayerId = ReadUnsignedVarInt();
		runtimeEntityId = ReadUnsignedVarLong();
		runtimeEntitySyncMessageId = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		blockPosition = default;
		blockRuntimeId = default;
		flags = default;
		dataLayerId = default;
		runtimeEntityId = default;
		runtimeEntitySyncMessageId = default;
	}
}