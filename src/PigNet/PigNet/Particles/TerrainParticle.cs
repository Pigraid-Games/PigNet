using PigNet.Blocks;
using PigNet.Worlds;

namespace PigNet.Particles;

public class TerrainParticle : LegacyParticle
{
	public TerrainParticle(Level level, Block block) : base(ParticleType.Terrain, level)
	{
		Data = block.RuntimeId;
	}
}