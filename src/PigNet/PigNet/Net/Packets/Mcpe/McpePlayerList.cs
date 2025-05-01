
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpePlayerList : Packet<McpePlayerList>
{
	public PlayerRecords records;

	public McpePlayerList()
	{
		Id = 0x3f;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(records);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		records = ReadPlayerRecords();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		records = default;
	}
}