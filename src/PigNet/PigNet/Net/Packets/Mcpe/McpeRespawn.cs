
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeRespawn : Packet<McpeRespawn>
{
	public long runtimeActorId;
	public PlayerRespawnState state;

	public float x;
	public float y;
	public float z;

	public McpeRespawn()
	{
		Id = 0x2d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(x);
		Write(y);
		Write(z);
		Write((byte) state);
		WriteUnsignedVarLong(runtimeActorId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		state = (PlayerRespawnState) ReadByte();
		runtimeActorId = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		x = default;
		y = default;
		z = default;
		state = default;
		runtimeActorId = default;
	}
}