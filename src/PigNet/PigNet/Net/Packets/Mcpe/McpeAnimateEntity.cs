
namespace PigNet.Net.Packets.Mcpe;

public class McpeAnimateEntity : Packet<McpeAnimateEntity>
{
	public string animationName;
	public float blendOutTime;
	public string controllerName;
	public long[] entities;
	public int molangVersion;
	public string nextState;
	public string stopExpression;

	public McpeAnimateEntity()
	{
		Id = 0x9e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(animationName);
		Write(nextState);
		Write(stopExpression);
		Write(molangVersion);
		Write(controllerName);
		Write(blendOutTime);
		WriteUnsignedVarInt((uint) entities.Length);
		foreach (long t in entities) WriteUnsignedVarLong(t);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		animationName = ReadString();
		nextState = ReadString();
		stopExpression = ReadString();
		molangVersion = ReadInt();
		controllerName = ReadString();
		blendOutTime = ReadFloat();
		for (int i = 0; i < ReadUnsignedVarInt(); i++) entities[i] = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		animationName = default;
		nextState = default;
		stopExpression = default;
		molangVersion = default;
		controllerName = default;
		blendOutTime = default;
		entities = default;
	}
}