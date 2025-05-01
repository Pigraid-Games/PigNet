using PigNet.Items;
using PigNet.Worlds;

namespace PigNet.Particles;

public class ItemBreakParticle : LegacyParticle
{
	public ItemBreakParticle(Level level, Item item) : base(ParticleType.ItemBreak, level)
	{
		Data = (item.RuntimeId << 16) | (ushort) item.Metadata;
	}
}