
using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetSpawnPosition : Packet<McpeSetSpawnPosition>
{
	public BlockCoordinates coordinates;
	public int dimension;
	public int spawnType;
	public BlockCoordinates unknownCoordinates;

	public McpeSetSpawnPosition()
	{
		Id = 0x2b;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(spawnType);
		Write(coordinates);
		WriteSignedVarInt(dimension);
		Write(unknownCoordinates);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		spawnType = ReadSignedVarInt();
		coordinates = ReadBlockCoordinates();
		dimension = ReadSignedVarInt();
		unknownCoordinates = ReadBlockCoordinates();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		spawnType = default;
		coordinates = default;
		dimension = default;
		unknownCoordinates = default;
	}
}