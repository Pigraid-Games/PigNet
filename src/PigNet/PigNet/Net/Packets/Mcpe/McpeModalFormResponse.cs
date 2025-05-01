
#nullable enable
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeModalFormResponse : Packet<McpeModalFormResponse>
{

	public uint formId;
	public string? jsonResponse;
	public ModalFormCancelReason? formCancelReason;
	
	public McpeModalFormResponse()
	{
		Id = 0x65;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(formId);
		Write(jsonResponse != null);
		if (jsonResponse != null) Write(jsonResponse);
		Write(formCancelReason.HasValue);
		if (formCancelReason.HasValue) Write((byte) formCancelReason.Value);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		formId = ReadUnsignedVarInt();
		if (ReadBool()) jsonResponse = ReadString();
		if (ReadBool()) formCancelReason = (ModalFormCancelReason) ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		formId = default;
		jsonResponse = default;
		formCancelReason = default;
	}
}