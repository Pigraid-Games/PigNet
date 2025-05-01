
namespace PigNet.Net.Packets.Mcpe;

public class McpeSetDisplayObjective : Packet<McpeSetDisplayObjective>
{
	public string criteriaName;
	public string objectiveDisplayName;
	public string displaySlotName;
	public string objectiveName;
	public int sortOrder;

	public McpeSetDisplayObjective()
	{
		Id = 0x6b;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(displaySlotName);
		Write(objectiveName);
		Write(objectiveDisplayName);
		Write(criteriaName);
		WriteSignedVarInt(sortOrder);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		displaySlotName = ReadString();
		objectiveName = ReadString();
		objectiveDisplayName = ReadString();
		criteriaName = ReadString();
		sortOrder = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		displaySlotName = default;
		objectiveName = default;
		objectiveDisplayName = default;
		criteriaName = default;
		sortOrder = default;
	}
}