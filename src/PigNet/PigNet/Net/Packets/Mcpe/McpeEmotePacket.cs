
namespace PigNet.Net.Packets.Mcpe;

public class McpeEmotePacket : Packet<McpeEmotePacket>
{
	public long runtimeActorId;
	public string emoteId;
	public uint emoteLengthTicks;
	public string xuid;
	public string platformId;
	public byte flags;

	public const int FlagServer = 1 << 0;
	public const int MuteAnnouncement = 1 << 1;
	
	public McpeEmotePacket()
	{
		Id = 0x8a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write(emoteId);
		WriteUnsignedVarInt(emoteLengthTicks);
		Write(xuid);
		Write(platformId);
		Write(flags);

	}
	
	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		emoteId = ReadString();
		emoteLengthTicks = ReadUnsignedVarInt();
		xuid = ReadString();
		platformId = ReadString();
		flags = ReadByte();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		xuid = default;
		platformId = default;
		emoteId = default;
		emoteLengthTicks = default;
		flags = default;
	}
}