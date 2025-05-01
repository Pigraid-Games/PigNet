
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateSoftEnum : Packet<McpeUpdateSoftEnum>
{
	public string enumName;
	public string[] values;
	public SoftEnumUpdateType updateType;
	
	public McpeUpdateSoftEnum()
	{
		Id = 0x72;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		
		Write(enumName);
		WriteUnsignedVarInt((uint) values.Length);
		foreach (string value in values) Write(value);
		Write((byte) updateType);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		enumName = ReadString();
		var size = ReadUnsignedVarInt();
		values = new string[size];
		for (int i = 0; i < size; i++) values[i] = ReadString();
		updateType = (SoftEnumUpdateType) ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();
		
		enumName = default;
		values = default;
		updateType = default;
	}
}