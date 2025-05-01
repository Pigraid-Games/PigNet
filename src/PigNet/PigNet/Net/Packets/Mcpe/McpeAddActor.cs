
using PigNet.Utils.Metadata;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAddActor : Packet<McpeAddActor>
{
	public EntityAttributes attributes;
	public float bodyYaw;

	public long entityIdSelf;
	public string entityType;
	public float headYaw;
	public EntityLinks links;
	public MetadataDictionary metadata;
	public float pitch;
	public long runtimeEntityId;
	public float speedX;
	public float speedY;
	public float speedZ;
	public PropertySyncData syncdata;
	public float x;
	public float y;
	public float yaw;
	public float z;

	public McpeAddActor()
	{
		Id = 0x0d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(entityIdSelf);
		WriteUnsignedVarLong(runtimeEntityId);
		Write(entityType);
		Write(x);
		Write(y);
		Write(z);
		Write(speedX);
		Write(speedY);
		Write(speedZ);
		Write(pitch);
		Write(yaw);
		Write(headYaw);
		Write(bodyYaw);
		Write(attributes);
		Write(metadata);
		Write(syncdata);
		Write(links);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entityIdSelf = ReadSignedVarLong();
		runtimeEntityId = ReadUnsignedVarLong();
		entityType = ReadString();
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		speedX = ReadFloat();
		speedY = ReadFloat();
		speedZ = ReadFloat();
		pitch = ReadFloat();
		yaw = ReadFloat();
		headYaw = ReadFloat();
		bodyYaw = ReadFloat();
		attributes = ReadEntityAttributes();
		metadata = ReadMetadataDictionary();
		syncdata = ReadPropertySyncData();
		links = ReadEntityLinks();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entityIdSelf = default;
		runtimeEntityId = default;
		entityType = default;
		x = default;
		y = default;
		z = default;
		speedX = default;
		speedY = default;
		speedZ = default;
		pitch = default;
		yaw = default;
		headYaw = default;
		bodyYaw = default;
		attributes = default;
		metadata = default;
		syncdata = default;
		links = default;
	}
}