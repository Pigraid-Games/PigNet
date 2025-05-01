
namespace PigNet.Net.Packets.Mcpe;

public class McpeRemoveObjective : Packet<McpeRemoveObjective>
{
	public string objectiveName;

	public McpeRemoveObjective()
	{
		Id = 0x6a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(objectiveName);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		objectiveName = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		objectiveName = default;
	}
}