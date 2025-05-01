using PigNet.Blocks;
using PigNet.Net.Packets.Mcpe;
using PigNet.Worlds;

namespace PigNet.Particles;

public class DestroyBlockParticle : LegacyParticle
{
	public DestroyBlockParticle(Level level, Block block) : base(0, level)
	{
		Data =  block.RuntimeId;
		Position = block.Coordinates;
	}

	public override void Spawn()
	{
		McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
		particleEvent.eventId = LevelEventType.ParticlesDestroyBlock;
		particleEvent.position = Position;
		particleEvent.data = Data;
		Level.RelayBroadcast(particleEvent);
	}
}