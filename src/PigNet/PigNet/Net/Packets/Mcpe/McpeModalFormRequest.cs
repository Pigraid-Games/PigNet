
namespace PigNet.Net.Packets.Mcpe;

public class McpeModalFormRequest : Packet<McpeModalFormRequest>
{
	public uint formId;
	public string formUiJson;

	public McpeModalFormRequest()
	{
		Id = 0x64;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(formId);
		Write(formUiJson);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		formId = ReadUnsignedVarInt();
		formUiJson = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();


		formId = default;
		formUiJson = default;
	}
}