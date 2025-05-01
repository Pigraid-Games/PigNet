
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetActorLink : Packet<McpeSetActorLink>
{
	public ActorLinkType linkType;

	public long riddenId;
	public long riderId;
	public byte unknown;
	public float vehicleAngularVelocity;

	public McpeSetActorLink()
	{
		Id = 0x29;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(riddenId);
		WriteSignedVarLong(riderId);
		Write((byte) linkType);
		Write(unknown);
		Write(false);
		Write(vehicleAngularVelocity);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		riddenId = ReadSignedVarLong();
		riderId = ReadSignedVarLong();
		linkType = (ActorLinkType) ReadByte();
		unknown = ReadByte();
		vehicleAngularVelocity = ReadFloat();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		riddenId = default;
		riderId = default;
		linkType = default;
		unknown = default;
		vehicleAngularVelocity = default;
	}
}