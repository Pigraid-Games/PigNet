
using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpeLevelSoundEvent : Packet<McpeLevelSoundEvent>
{
	public uint soundId;
	public Vector3 position;
	public int extraData = -1;
	public string entityType = ":";
	public bool isBabyMob;
	public bool disableRelativeVolume;
	public long actorUniqueId = -1;


	public McpeLevelSoundEvent()
	{
		Id = 0x7b;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(soundId);
		Write(position);
		WriteSignedVarInt(extraData);
		Write(":");
		Write(false);
		Write(disableRelativeVolume);
		Write(actorUniqueId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		soundId = ReadUnsignedVarInt();
		position = ReadVector3();
		extraData = ReadSignedVarInt();
		entityType = ReadString();
		isBabyMob = ReadBool();
		disableRelativeVolume = ReadBool();
		actorUniqueId = ReadInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		soundId = default;
		position = default;
		extraData = default;
		entityType = default;
		isBabyMob = default;
		disableRelativeVolume = default;
		actorUniqueId = default;
	}
}