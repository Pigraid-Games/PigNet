
namespace PigNet.Net.Packets.Mcpe;

public class McpeActorPickRequest : Packet<McpeActorPickRequest>
{
	public bool addUserData;
	public ulong runtimeActorId;
	public byte selectedSlot;

	public McpeActorPickRequest()
	{
		Id = 0x23;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(runtimeActorId);
		Write(selectedSlot);
		Write(addUserData);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUlong();
		selectedSlot = ReadByte();
		addUserData = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		selectedSlot = default;
		addUserData = default;
	}
}