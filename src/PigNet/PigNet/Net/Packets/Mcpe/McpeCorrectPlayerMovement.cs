
using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpeCorrectPlayerMovement : Packet<McpeCorrectPlayerMovement>
{
	public byte Type;
	public bool onGround;
	public Vector3 position;
	public long tick;
	public Vector3 velocity;

	public McpeCorrectPlayerMovement()
	{
		Id = 0xA1;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(Type);
		Write(position);
		Write(velocity);
		Write(onGround);
		WriteUnsignedVarLong(tick);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		Type = ReadByte();
		position = ReadVector3();
		velocity = ReadVector3();
		onGround = ReadBool();
		tick = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		Type = default;
		position = default;
		velocity = default;
		onGround = default;
		tick = default;
	}
}