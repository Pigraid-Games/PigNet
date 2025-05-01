
using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpeChangeDimension : Packet<McpeChangeDimension>
{
	public int dimensionId;
	public Vector3 position;
	public bool respawn;
	// Leave empty if there is no loading screen expected on the client. This id needs to be unique and not conflict with any other active loading screens.
	// This is implemented with an unsigned integer incrementing forever, and that is expected to not have collisions when
	// it wraps around back to 0 if that could be a possibility.
	public uint loadingScreenId;

	public McpeChangeDimension()
	{
		Id = 0x3d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(dimensionId);
		Write(position);
		Write(respawn);
		WriteUnsignedVarInt(loadingScreenId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		dimensionId = ReadSignedVarInt();
		position = ReadVector3();
		respawn = ReadBool();
		loadingScreenId = ReadUnsignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		dimensionId = default;
		position = default;
		respawn = default;
		loadingScreenId = default;
	}
}