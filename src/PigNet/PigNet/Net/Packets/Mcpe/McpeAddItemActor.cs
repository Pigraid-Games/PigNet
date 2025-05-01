
using PigNet.Items;
using PigNet.Utils.Metadata;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAddItemActor : Packet<McpeAddItemActor>
{
	public long entityIdSelf;
	public bool isFromFishing;
	public Item item;
	public MetadataDictionary metadata;
	public long runtimeActorId;
	public float speedX;
	public float speedY;
	public float speedZ;
	public float x;
	public float y;
	public float z;

	public McpeAddItemActor()
	{
		Id = 0x0f;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(entityIdSelf);
		WriteUnsignedVarLong(runtimeActorId);
		Write(item);
		Write(x);
		Write(y);
		Write(z);
		Write(speedX);
		Write(speedY);
		Write(speedZ);
		Write(metadata);
		Write(isFromFishing);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entityIdSelf = ReadSignedVarLong();
		runtimeActorId = ReadUnsignedVarLong();
		item = ReadItem();
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		speedX = ReadFloat();
		speedY = ReadFloat();
		speedZ = ReadFloat();
		metadata = ReadMetadataDictionary();
		isFromFishing = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entityIdSelf = default;
		runtimeActorId = default;
		item = default;
		x = default;
		y = default;
		z = default;
		speedX = default;
		speedY = default;
		speedZ = default;
		metadata = default;
		isFromFishing = default;
	}
}