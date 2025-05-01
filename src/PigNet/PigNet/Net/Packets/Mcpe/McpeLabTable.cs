
using PigNet.Net.EnumerationsTable;
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeLabTable : Packet<McpeLabTable>
{
	public LabTablePacketType type;
	public BlockCoordinates position;
	public byte reaction;

	public McpeLabTable()
	{
		Id = 0x6d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte) type);
		Write(position);
		Write(reaction);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		type = (LabTablePacketType) ReadByte();
		position = ReadBlockCoordinates();
		reaction = ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		type = default;
		position = default;
		reaction = default;
	}
}