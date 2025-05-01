
using PigNet.Items;
using PigNet.Utils;
using PigNet.Utils.Metadata;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAddPlayer : Packet<McpeAddPlayer>
{
	public byte commandPermissions;
	public string deviceId;
	public int deviceOs;
	public long entityIdSelf;
	public uint gameType;
	public float headYaw;
	public Item item;
	public AbilityLayers layers;
	public EntityLinks links;
	public MetadataDictionary metadata;
	public float pitch;
	public string platformChatId;
	public byte playerPermissions;
	public long runtimeEntityId;
	public float speedX;
	public float speedY;
	public float speedZ;
	public PropertySyncData syncdata;
	public string username;

	public UUID uuid;
	public float x;
	public float y;
	public float yaw;
	public float z;

	public McpeAddPlayer()
	{
		Id = 0x0c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(uuid);
		Write(username);
		WriteUnsignedVarLong(runtimeEntityId);
		Write(platformChatId);
		Write(x);
		Write(y);
		Write(z);
		Write(speedX);
		Write(speedY);
		Write(speedZ);
		Write(pitch);
		Write(yaw);
		Write(headYaw);
		Write(item);
		WriteUnsignedVarInt(gameType);
		Write(metadata);
		Write(syncdata);
		Write((ulong) entityIdSelf);
		Write(playerPermissions);
		Write(commandPermissions);
		Write(layers);
		Write(links);
		Write(deviceId);
		Write(deviceOs);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		uuid = ReadUUID();
		username = ReadString();
		runtimeEntityId = ReadUnsignedVarLong();
		platformChatId = ReadString();
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		speedX = ReadFloat();
		speedY = ReadFloat();
		speedZ = ReadFloat();
		pitch = ReadFloat();
		yaw = ReadFloat();
		headYaw = ReadFloat();
		item = ReadItem();
		gameType = ReadUnsignedVarInt();
		metadata = ReadMetadataDictionary();
		syncdata = ReadPropertySyncData();
		entityIdSelf = ReadSignedVarLong();
		playerPermissions = ReadByte();
		commandPermissions = ReadByte();
		layers = ReadAbilityLayers();
		links = ReadEntityLinks();
		deviceId = ReadString();
		deviceOs = ReadInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		uuid = default;
		username = default;
		runtimeEntityId = default;
		platformChatId = default;
		x = default;
		y = default;
		z = default;
		speedX = default;
		speedY = default;
		speedZ = default;
		pitch = default;
		yaw = default;
		headYaw = default;
		item = default;
		gameType = default;
		metadata = default;
		syncdata = default;
		entityIdSelf = default;
		playerPermissions = default;
		commandPermissions = default;
		layers = default;
		links = default;
		deviceId = default;
		deviceOs = default;
	}
}