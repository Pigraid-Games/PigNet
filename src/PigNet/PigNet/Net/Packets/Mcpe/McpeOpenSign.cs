
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeOpenSign : Packet<McpeOpenSign>
{
	public BlockCoordinates coordinates;
	public bool front;

	public McpeOpenSign()
	{
		Id = 0x12f;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(coordinates);
		Write(front);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		coordinates = ReadBlockCoordinates();
		front = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		coordinates = default;
		front = default;
	}
}