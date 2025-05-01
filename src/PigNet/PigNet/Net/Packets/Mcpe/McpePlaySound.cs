
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpePlaySound : Packet<McpePlaySound>
{
	public float pitch;
	public string soundName;
	public float volume;
	public float x;
	public float y;
	public float z;

	public McpePlaySound()
	{
		Id = 0x56;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(soundName);
		Write(new BlockCoordinates((int) x * 8, (int) y * 8, (int) z * 8));
		Write(volume);
		Write(pitch);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		soundName = ReadString();
		BlockCoordinates blockCoordinates = ReadBlockCoordinates();
		x = blockCoordinates.X / 8;
		y = blockCoordinates.Y / 8;
		z = blockCoordinates.Z / 8;
		volume = ReadFloat();
		pitch = ReadFloat();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		soundName = default;
		x = default;
		y = default;
		z = default;
		volume = default;
		pitch = default;
	}
}