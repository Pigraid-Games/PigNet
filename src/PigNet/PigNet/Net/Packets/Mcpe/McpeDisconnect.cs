
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeDisconnect : Packet<McpeDisconnect>
{
	public DisconnectFailReason failReason;
	public string filteredMessage;

	public bool hideDisconnectReason;
	public string message;

	public McpeDisconnect()
	{
		Id = 0x05;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt((uint) failReason);
		Write(hideDisconnectReason);
		Write(message);
		Write(filteredMessage);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		failReason = (DisconnectFailReason) ReadUnsignedVarInt();
		hideDisconnectReason = ReadBool();
		message = ReadString();
		filteredMessage = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		hideDisconnectReason = default;
		message = default;
		failReason = default(int);
	}
}