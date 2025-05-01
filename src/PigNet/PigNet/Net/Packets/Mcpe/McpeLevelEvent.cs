
using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpeLevelEvent : Packet<McpeLevelEvent>
{
	public int data;
	public LevelEventType eventId;
	public Vector3 position;

	public McpeLevelEvent()
	{
		Id = 0x19;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt((short) eventId);
		Write(position);
		WriteSignedVarInt(data);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		eventId = (LevelEventType) ReadSignedVarInt();
		position = ReadVector3();
		data = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		eventId = default;
		position = default;
		data = default;
	}
}