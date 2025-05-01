
namespace PigNet.Net.Packets.Mcpe;

public class McpeRequestPermission : Packet<McpeRequestPermission>
{
	public long runtimeActorId;
	public uint permissionLevel;
	public ushort customPermissionFlags;


	public McpeRequestPermission()
	{
		Id = 0xb9;
		IsMcpe = true;
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadLong();
		permissionLevel = ReadUnsignedVarInt();
		customPermissionFlags = ReadUshort();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		permissionLevel = default;
		customPermissionFlags = default;
	}
}