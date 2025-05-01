
using PigNet.Utils.Nbt;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateEquipment : Packet<McpeUpdateEquipment>
{
	public long actorId;
	public Nbt namedtag;
	public byte unknown;

	public byte containerId;
	public byte containerType;

	public McpeUpdateEquipment()
	{
		Id = 0x51;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(containerId);
		Write(containerType);
		Write(unknown);
		WriteUnsignedVarLong(actorId);
		Write(namedtag);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		containerId = ReadByte();
		containerType = ReadByte();
		unknown = ReadByte();
		actorId = ReadSignedVarLong();
		namedtag = ReadNbt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		containerId = default;
		containerType = default;
		unknown = default;
		actorId = default;
		namedtag = default;
	}
}