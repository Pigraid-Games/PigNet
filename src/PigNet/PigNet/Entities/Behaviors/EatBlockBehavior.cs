using System.Numerics;
using PigNet.Blocks;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Particles;
using PigNet.Utils.Vectors;

namespace PigNet.Entities.Behaviors;

public class EatBlockBehavior(Mob entity) : BehaviorBase
{
	private int _duration;

	public override bool ShouldStart()
	{
		if (entity.Level.Random.Next(1000) != 0) return false;

		var coordinates = entity.KnownPosition;
		var direction = Vector3.Normalize(coordinates.GetHeadDirectionVector());

		BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

		var shouldStart = entity.Level.GetBlock(coord.BlockDown()) is GrassBlock || entity.Level.GetBlock(coord) is ShortGrass;
		if (!shouldStart) return false;

		_duration = 40;

		entity.Velocity *= new Vector3(0, 1, 0);

		McpeActorEvent actorEvent = McpeActorEvent.CreateObject();
		actorEvent.runtimeEntityId = entity.EntityId;
		actorEvent.eventId = ActorEvent.EatGrass;
		entity.Level.RelayBroadcast(actorEvent);

		return true;
	}

	public override bool CanContinue()
	{
		return _duration-- > 0;
	}

	public override void OnEnd()
	{
		PlayerLocation coordinates = entity.KnownPosition;
		var direction = Vector3.Normalize(coordinates.GetHeadDirectionVector());

		BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

		Block broken;
		if (entity.Level.GetBlock(coord) is ShortGrass)
		{
			broken = entity.Level.GetBlock(coord);
			entity.Level.SetAir(coord);
		}
		else
		{
			coord += BlockCoordinates.Down;
			broken = entity.Level.GetBlock(coord);
			entity.Level.SetBlock(new Dirt {Coordinates = coord});
		}
		var particle = new DestroyBlockParticle(entity.Level, broken);
		particle.Spawn();
	}
}