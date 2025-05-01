
namespace PigNet.Net.Packets.Mcpe;

public class McpeMobEffect : Packet<McpeMobEffect>
{
	public int amplifier;
	public int duration;
	public int effectId;
	public byte eventId;
	public bool particles;

	public long runtimeActorId;
	public long tick;

	public McpeMobEffect()
	{
		Id = 0x1c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write(eventId);
		WriteSignedVarInt(effectId);
		WriteSignedVarInt(amplifier);
		Write(particles);
		WriteSignedVarInt(duration);
		WriteUnsignedVarLong(tick);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		eventId = ReadByte();
		effectId = ReadSignedVarInt();
		amplifier = ReadSignedVarInt();
		particles = ReadBool();
		duration = ReadSignedVarInt();
		tick = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		eventId = default;
		effectId = default;
		amplifier = default;
		particles = default;
		duration = default;
		tick = default;
	}
}