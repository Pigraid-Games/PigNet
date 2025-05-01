
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeMovePlayer : Packet<McpeMovePlayer>
{
	public float headYaw;
	public PositionMode mode;
	public bool onGround;
	public long ridingRuntimeId;
	public float pitch;

	public long playerRuntimeId;
	public float x;
	public float y;
	public float yaw;
	public float z;
	
	public long tick;

	public McpeMovePlayer()
	{
		Id = 0x13;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(playerRuntimeId);
		Write(x);
		Write(y);
		Write(z);
		Write(pitch);
		Write(yaw);
		Write(headYaw);
		Write((byte) mode);
		Write(onGround);
		WriteUnsignedVarLong(ridingRuntimeId);
		if (mode == PositionMode.Teleport)
		{
			Write(0);
			Write(0);
		}

		WriteUnsignedVarLong(tick);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		playerRuntimeId = ReadUnsignedVarLong();
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		pitch = ReadFloat();
		yaw = ReadFloat();
		headYaw = ReadFloat();
		mode = (PositionMode) ReadByte();
		onGround = ReadBool();
		ridingRuntimeId = ReadUnsignedVarLong();
		if (mode == PositionMode.Teleport)
		{
			ReadInt();
			ReadInt();
		}

		tick = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		playerRuntimeId = default;
		x = default;
		y = default;
		z = default;
		pitch = default;
		yaw = default;
		headYaw = default;
		mode = default;
		onGround = default;
		ridingRuntimeId = default;
	}
}