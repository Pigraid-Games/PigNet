using fNbt.Serialization;

namespace PigNet.Worlds.Anvil.Data;

[NbtObject]
public class HeightMaps
{
	[NbtProperty("MOTION_BLOCKING")]
	public long[] MotionBlocking { get; set; }

	[NbtProperty("MOTION_BLOCKING_NO_LEAVES")]
	public long[] MotionBlockingNoLeaves { get; set; }

	[NbtProperty("OCEAN_FLOOR")]
	public long[] OceanFloor { get; set; }

	[NbtProperty("WORLD_SURFACE")]
	public long[] WorldSurface { get; set; }

	public void PopulateChunk(ChunkColumn chunk)
	{
		if (WorldSurface != null)
		{
			AnvilDataUtils.ReadAnyBitLengthShortFromLongs(WorldSurface, chunk._height, 9);
		}
	}
}