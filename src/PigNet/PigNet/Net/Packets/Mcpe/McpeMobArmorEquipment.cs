
using PigNet.Items;

namespace PigNet.Net.Packets.Mcpe;

public class McpeMobArmorEquipment : Packet<McpeMobArmorEquipment>
{
	public Item body;
	public Item boots;
	public Item chestplate;
	public Item helmet;
	public Item leggings;
	public long runtimeActorId;

	public McpeMobArmorEquipment()
	{
		Id = 0x20;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write(helmet);
		Write(chestplate);
		Write(leggings);
		Write(boots);
		Write(body);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		helmet = ReadItem();
		chestplate = ReadItem();
		leggings = ReadItem();
		boots = ReadItem();
		body = ReadItem();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		helmet = default;
		chestplate = default;
		leggings = default;
		boots = default;
		body = default;
	}
}