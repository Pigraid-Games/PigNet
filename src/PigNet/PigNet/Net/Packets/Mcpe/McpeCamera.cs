
namespace PigNet.Net.Packets.Mcpe;

public class McpeCamera : Packet<McpeCamera>
{
	public long cameraId;			// actorUniqueId
	public long targetPlayerId;		// actorUniqueId

	public McpeCamera()
	{
		Id = 0x49;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(cameraId);
		WriteSignedVarLong(targetPlayerId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		cameraId = ReadSignedVarLong();
		targetPlayerId = ReadSignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		cameraId = default;
		targetPlayerId = default;
	}
}