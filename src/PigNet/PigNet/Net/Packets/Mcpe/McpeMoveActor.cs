
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeMoveActor : Packet<McpeMoveActor>
{
	public byte flags;
	public PlayerLocation position;

	public long runtimeEntityId;

	public McpeMoveActor()
	{
		Id = 0x12;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeEntityId);
		Write(flags);
		Write(position);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeEntityId = ReadUnsignedVarLong();
		flags = ReadByte();
		position = ReadPlayerLocation();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeEntityId = default;
		flags = default;
		position = default;
	}
}