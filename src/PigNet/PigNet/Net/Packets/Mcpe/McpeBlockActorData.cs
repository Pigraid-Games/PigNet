
using PigNet.Utils.Nbt;
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeBlockActorData : Packet<McpeBlockActorData>
{
	public BlockCoordinates blockPosition;
	public Nbt actorDataTags;

	public McpeBlockActorData()
	{
		Id = 0x38;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(blockPosition);
		Write(actorDataTags);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		blockPosition = ReadBlockCoordinates();
		actorDataTags = ReadNbt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		blockPosition = default;
		actorDataTags = default;
	}
}