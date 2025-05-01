
namespace PigNet.Net.Packets.Mcpe;

public class McpeNetworkSettings : Packet<McpeNetworkSettings>
{

	public bool clientThrottleEnabled;
	public float clientThrottleScalar;
	public byte clientThrottleThreshold;
	public short compressionAlgorithm;

	public short compressionThreshold;

	public McpeNetworkSettings()
	{
		Id = 0x8f;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(compressionThreshold);
		Write(compressionAlgorithm);
		Write(clientThrottleEnabled);
		Write(clientThrottleThreshold);
		Write(clientThrottleScalar);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		compressionThreshold = ReadShort();
		compressionAlgorithm = ReadShort();
		clientThrottleEnabled = ReadBool();
		clientThrottleThreshold = ReadByte();
		clientThrottleScalar = ReadFloat();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		compressionThreshold = default;
		compressionAlgorithm = default;
		clientThrottleEnabled = default;
		clientThrottleThreshold = default;
		clientThrottleScalar = default;
	}
}