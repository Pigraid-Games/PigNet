
namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateAbilities : Packet<McpeUpdateAbilities>
{
	public byte commandPermissions;

	public long entityUniqueId;
	public AbilityLayers layers;
	public byte playerPermissions;

	public McpeUpdateAbilities()
	{
		Id = 0xbb;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(entityUniqueId);
		Write(playerPermissions);
		Write(commandPermissions);
		Write(layers);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entityUniqueId = ReadLong();
		playerPermissions = ReadByte();
		commandPermissions = ReadByte();
		layers = ReadAbilityLayers();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entityUniqueId = default;
		playerPermissions = default;
		commandPermissions = default;
		layers = default;
	}
}