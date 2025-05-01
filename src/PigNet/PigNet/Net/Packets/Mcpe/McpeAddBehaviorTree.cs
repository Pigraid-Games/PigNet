
namespace PigNet.Net.Packets.Mcpe;

public class McpeAddBehaviorTree : Packet<McpeAddBehaviorTree>
{
	public string behaviortree;

	public McpeAddBehaviorTree()
	{
		Id = 0x59;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(behaviortree);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		behaviortree = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		behaviortree = default;
	}
}