
using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSpawnParticleEffect : Packet<McpeSpawnParticleEffect>
{
	public byte dimensionId;
	public long entityId;
	public string molangVariablesJson;
	public string particleName;
	public Vector3 position;

	public McpeSpawnParticleEffect()
	{
		Id = 0x76;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(dimensionId);
		WriteSignedVarLong(entityId);
		Write(position);
		Write(particleName);
		Write(molangVariablesJson);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		dimensionId = ReadByte();
		entityId = ReadSignedVarLong();
		position = ReadVector3();
		particleName = ReadString();
		molangVariablesJson = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		dimensionId = default;
		entityId = default;
		position = default;
		particleName = default;
		molangVariablesJson = default;
	}
}