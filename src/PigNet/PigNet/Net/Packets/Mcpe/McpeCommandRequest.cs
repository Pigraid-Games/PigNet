
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeCommandRequest : Packet<McpeCommandRequest>
{
	public string command;
	public uint commandType;
	public bool isinternal;
	public string requestId;
	public UUID unknownUuid;
	public int version;

	public McpeCommandRequest()
	{
		Id = 0x4d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(command);
		WriteUnsignedVarInt(commandType);
		Write(unknownUuid);
		Write(requestId);
		Write(isinternal);
		WriteSignedVarInt(version);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		command = ReadString();
		commandType = ReadUnsignedVarInt();
		unknownUuid = ReadUUID();
		requestId = ReadString();
		isinternal = ReadBool();
		version = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		command = default;
		commandType = default;
		unknownUuid = default;
		requestId = default;
		isinternal = default;
		version = default;
	}
}