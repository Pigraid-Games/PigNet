using System;
using System.Numerics;
using PigNet.Blocks;
using PigNet.Entities.Passive;
using PigNet.Particles;
using PigNet.Utils.Vectors;

namespace PigNet.Entities.Behaviors;

public class HorseEatBlockBehavior : BehaviorBase
{
	private readonly Mob _entity;
	private int _duration;
	private int _timeLeft;

	public HorseEatBlockBehavior(Mob entity, int duration)
	{
		this._entity = entity;
		_duration = Math.Max(40, duration);
		_timeLeft = _duration;
	}

	public override bool ShouldStart()
	{
		if (!(_entity is Horse)) return false;

		if (_entity.Level.Random.Next(1000) != 0) return false;

		var coordinates = _entity.KnownPosition;
		var direction = Vector3.Normalize(coordinates.GetHeadDirectionVector());

		BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

		var shouldStart = _entity.Level.GetBlock(coord.BlockDown()) is GrassBlock || _entity.Level.GetBlock(coord) is ShortGrass;
		if (!shouldStart) return false;

		_duration = 40;

		_entity.Velocity *= new Vector3(0, 1, 0);
		SetEating((Horse) _entity, true);

		return true;
	}

	public override bool CanContinue()
	{
		return _duration-- > 0;
	}

	public override void OnTick(Entity[] entities)
	{
	}

	public override void OnEnd()
	{
		var coordinates = _entity.KnownPosition;
		var direction = Vector3.Normalize(coordinates.GetHeadDirectionVector());

		BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

		Block broken = null;
		if (_entity.Level.GetBlock(coord) is ShortGrass)
		{
			broken = _entity.Level.GetBlock(coord);
			_entity.Level.SetAir(coord);
		}
		else
		{
			coord += BlockCoordinates.Down;
			broken = _entity.Level.GetBlock(coord);
			_entity.Level.SetBlock(new Dirt {Coordinates = coord});
		}
		DestroyBlockParticle particle = new DestroyBlockParticle(_entity.Level, broken);
		particle.Spawn();
		SetEating((Horse) _entity, false);
	}

	private void SetEating(Horse horse, bool isEating)
	{
		horse.IsEating = isEating;
		horse.BroadcastSetEntityData();
	}
}