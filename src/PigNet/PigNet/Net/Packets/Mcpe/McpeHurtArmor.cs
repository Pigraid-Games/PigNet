namespace PigNet.Net.Packets.Mcpe;

public class McpeHurtArmor : Packet<McpeHurtArmor>
{
	public long armorSlotFlags;

	public int cause;
	public int health;

	public McpeHurtArmor()
	{
		Id = 0x26;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteVarInt(cause);
		WriteSignedVarInt(health);
		WriteUnsignedVarLong(armorSlotFlags);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		cause = ReadVarInt();
		health = ReadSignedVarInt();
		armorSlotFlags = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		cause = default;
		health = default;
		armorSlotFlags = default;
	}
}