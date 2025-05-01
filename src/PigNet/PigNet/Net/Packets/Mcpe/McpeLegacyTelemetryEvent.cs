
namespace PigNet.Net.Packets.Mcpe;

public class McpeLegacyTelemetryEvent : Packet<McpeLegacyTelemetryEvent>
{
	public long targetActorId;
	public int eventType;
	public byte userPlayerId;
	public byte[] auxData; // TODO: Implement this correctly, using LegacyTelemetryEventPacket::AgentResult & LegacyTelemetryPacket::Type

	public McpeLegacyTelemetryEvent()
	{
		Id = 0x41;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(targetActorId);
		WriteSignedVarInt(eventType);
		Write(userPlayerId);
		Write(auxData);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		targetActorId = ReadUnsignedVarLong();
		eventType = ReadSignedVarInt();
		userPlayerId = ReadByte();
		auxData = ReadBytes(0, true);
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		targetActorId = default;
		eventType = default;
		userPlayerId = default;
		auxData = default;
	}
}