
using PigNet.Utils.Nbt;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateTrade : Packet<McpeUpdateTrade>
{
	public string displayName;
	public bool isWilling;
	public Nbt namedtag;
	public long playerEntityId;
	public long traderEntityId;
	public int unknown0;
	public int unknown1;
	public int unknown2;

	public byte containerId;
	public byte containerType;

	public McpeUpdateTrade()
	{
		Id = 0x50;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(containerId);
		Write(containerType);
		WriteVarInt(unknown0);
		WriteVarInt(unknown1);
		WriteVarInt(unknown2);
		Write(isWilling);
		WriteUnsignedVarLong(traderEntityId);
		WriteSignedVarLong(playerEntityId);
		Write(displayName);
		Write(namedtag);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		containerId = ReadByte();
		containerType = ReadByte();
		unknown0 = ReadVarInt();
		unknown1 = ReadVarInt();
		unknown2 = ReadVarInt();
		isWilling = ReadBool();
		traderEntityId = ReadSignedVarLong();
		playerEntityId = ReadSignedVarLong();
		displayName = ReadString();
		namedtag = ReadNbt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		containerId = default;
		containerType = default;
		unknown0 = default;
		unknown1 = default;
		unknown2 = default;
		isWilling = default;
		traderEntityId = default;
		playerEntityId = default;
		displayName = default;
		namedtag = default;
	}
}