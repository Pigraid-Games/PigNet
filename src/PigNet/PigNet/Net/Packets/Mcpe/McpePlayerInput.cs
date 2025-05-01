
using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpePlayerInput : Packet<McpePlayerInput>
{
	public Vector2 move;
	public bool jumping;
	public bool sneaking;

	public McpePlayerInput()
	{
		Id = 0x39;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(move);
		Write(jumping);
		Write(sneaking);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		move = ReadVector2();
		jumping = ReadBool();
		sneaking = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		move = default;
		jumping = default;
		sneaking = default;
	}
}