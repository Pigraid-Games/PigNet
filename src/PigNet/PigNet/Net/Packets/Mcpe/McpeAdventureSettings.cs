namespace PigNet.Net.Packets.Mcpe;

public class McpeAdventureSettings : Packet<McpeAdventureSettings>
{
	public uint flags;
	public uint commandPermission;
	public uint actionPermissions;
	public uint permissionLevel;
	public uint customStoredPermissions;
	public long actorUniqueId;
	
	public McpeAdventureSettings()
	{
		Id = 0x37;
		IsMcpe = true;
	}
	
	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(flags);
		WriteUnsignedVarInt(commandPermission);
		WriteUnsignedVarInt(actionPermissions);
		WriteUnsignedVarInt(permissionLevel);
		WriteUnsignedVarInt(customStoredPermissions);
		Write(actorUniqueId);
	}
	
	protected override void DecodePacket()
	{
		base.DecodePacket();

		flags = ReadUnsignedVarInt();
		commandPermission = ReadUnsignedVarInt();
		actionPermissions = ReadUnsignedVarInt();
		permissionLevel = ReadUnsignedVarInt();
		customStoredPermissions = ReadUnsignedVarInt();
		actorUniqueId = ReadLong();
	}
	
	protected override void ResetPacket()
	{
		base.ResetPacket();

		flags = default;
		commandPermission = default;
		actionPermissions = default;
		permissionLevel = default;
		customStoredPermissions = default;
		actorUniqueId = default;
	}
}