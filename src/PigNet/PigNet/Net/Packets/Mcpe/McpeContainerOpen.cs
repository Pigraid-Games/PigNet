using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeContainerOpen : Packet<McpeContainerOpen>
{
	public BlockCoordinates position;
	public byte containerId;
	public sbyte containerType;
	public long runtimeActorId;

	public McpeContainerOpen()
	{
		Id = 0x2e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(containerId);
		Write((byte) containerType);
		Write(position);
		WriteUnsignedVarLong(runtimeActorId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		containerId = ReadByte();
		containerType = ReadSByte();
		position = ReadBlockCoordinates();
		runtimeActorId = ReadSignedVarLong();
	}
	
	protected override void ResetPacket()
	{
		base.ResetPacket();

		containerId = default;
		containerType = default;
		position = default;
		runtimeActorId = default;
	}
}