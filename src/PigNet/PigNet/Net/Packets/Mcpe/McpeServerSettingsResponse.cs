
namespace PigNet.Net.Packets.Mcpe;

public class McpeServerSettingsResponse : Packet<McpeServerSettingsResponse>
{
	public long formId;
	public string formUiJson;

	public McpeServerSettingsResponse()
	{
		Id = 0x67;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(formId);
		Write(formUiJson);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		formId = ReadUnsignedVarLong();
		formUiJson = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		formId = default;
		formUiJson = default;
	}
}