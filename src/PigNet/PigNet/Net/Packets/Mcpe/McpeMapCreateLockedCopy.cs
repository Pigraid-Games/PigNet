
namespace PigNet.Net.Packets.Mcpe;

public class McpeMapCreateLockedCopy : Packet<McpeMapCreateLockedCopy>
{
	public long originalMapId;
	public long newMapId;
	
	public McpeMapCreateLockedCopy()
	{
		Id = 0x83;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(originalMapId);
		WriteSignedVarLong(newMapId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		originalMapId = ReadSignedVarLong();
		newMapId = ReadSignedVarLong();
	}

	protected override void ResetPacket()
	{
		originalMapId = default;
		newMapId = default;
	}
}