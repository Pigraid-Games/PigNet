
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeNpcRequest : Packet<McpeNpcRequest>
{
	public long runtimeActorId; // npc runtime id
	public NpcRequestType requestType;
	public string actions;
	public byte actionIndex;
	public string sceneName;

	public McpeNpcRequest()
	{
		Id = 0x62;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		Write((byte) requestType);
		Write(actions);
		Write(actionIndex);
		Write(sceneName);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		requestType = (NpcRequestType) ReadByte();
		actions = ReadString();
		actionIndex = ReadByte();
		sceneName = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		requestType = default;
		actions = default;
		actionIndex = default;
		sceneName = default;
	}
}