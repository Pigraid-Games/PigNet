
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeBlockEvent : Packet<McpeBlockEvent>
{
	public int case1;
	public int case2;

	public BlockCoordinates coordinates;

	public McpeBlockEvent()
	{
		Id = 0x1a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(coordinates);
		WriteSignedVarInt(case1);
		WriteSignedVarInt(case2);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		coordinates = ReadBlockCoordinates();
		case1 = ReadSignedVarInt();
		case2 = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		coordinates = default;
		case1 = default;
		case2 = default;
	}
}