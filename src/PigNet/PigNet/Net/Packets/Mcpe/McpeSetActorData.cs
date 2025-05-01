
using PigNet.Utils.Metadata;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetActorData : Packet<McpeSetActorData>
{
	public MetadataDictionary metadata;
	public long runtimeActorId;
	public PropertySyncData syncdata;
	public long tick;

	public McpeSetActorData()
	{
		Id = 0x27;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write(metadata);
		Write(syncdata);
		WriteUnsignedVarLong(tick);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		metadata = ReadMetadataDictionary();
		syncdata = ReadPropertySyncData();
		tick = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		metadata = default;
		syncdata = default;
		tick = default;
	}
}