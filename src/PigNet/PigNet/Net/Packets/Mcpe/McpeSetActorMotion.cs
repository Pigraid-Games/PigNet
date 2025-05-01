
using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetActorMotion : Packet<McpeSetActorMotion>
{
	public long runtimeActorId;
	public long tick;
	public Vector3 velocity;

	public McpeSetActorMotion()
	{
		Id = 0x28;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write(velocity);
		WriteUnsignedVarLong(tick);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		velocity = ReadVector3();
		tick = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		velocity = default;
		tick = default;
	}
}