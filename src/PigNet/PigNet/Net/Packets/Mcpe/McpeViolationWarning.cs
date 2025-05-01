
namespace PigNet.Net.Packets.Mcpe;

public class McpeViolationWarning : Packet<McpeViolationWarning>
{
	public int packetId;
	public string reason;
	public int severity;

	public int violationType;

	public McpeViolationWarning()
	{
		Id = 0x9c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(violationType);
		WriteSignedVarInt(severity);
		WriteSignedVarInt(packetId);
		Write(reason);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		violationType = ReadSignedVarInt();
		severity = ReadSignedVarInt();
		packetId = ReadSignedVarInt();
		reason = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		violationType = default;
		severity = default;
		packetId = default;
		reason = default;
	}
}