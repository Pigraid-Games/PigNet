
using System.Numerics;
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeInteract : Packet<McpeInteract>
{
	public enum Actions
	{
		RightClick = 1,
		LeftClick = 2,
		LeaveVehicle = 3,
		MouseOver = 4,
		OpenNpc = 5,
		OpenInventory = 6
	}

	public InteractPacketAction actionId;
	public long targetRuntimeActorId;
	public Vector3 position;

	public McpeInteract()
	{
		Id = 0x21;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte) actionId);
		WriteUnsignedVarLong(targetRuntimeActorId);
		if (actionId is InteractPacketAction.InteractUpdate or InteractPacketAction.StopRiding)
			// TODO: Something useful with this value
			Write(position);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		actionId = (InteractPacketAction) ReadByte();
		targetRuntimeActorId = ReadUnsignedVarLong();
		if (actionId is InteractPacketAction.InteractUpdate or InteractPacketAction.StopRiding)
			// TODO: Something useful with this value
			position = ReadVector3();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		actionId = default;
		targetRuntimeActorId = default;
		position = default;
	}
}