using System.Linq;
using System.Numerics;
using PigNet.Entities.Passive;
using PigNet.Worlds;

namespace PigNet.Entities.Behaviors;

public class FindAttackableTargetBehavior : BehaviorBase, ITargetingBehavior
{
	protected readonly Mob _entity;
	private readonly double _targetDistance;
	private int _targetUnseenTicks = 0;

	public FindAttackableTargetBehavior(Mob entity, double targetDistance = 16)
	{
		_entity = entity;
		_targetDistance = targetDistance;
	}

	public override bool ShouldStart()
	{
		if (_entity.Level.Random.Next(10) != 0) return false;

		Player player = _entity.Level.Players
			.OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.Value.KnownPosition))
			.FirstOrDefault(p =>
				p.Value.IsSpawned
				&& !p.Value.HealthManager.IsDead
				&& p.Value.GameMode != GameMode.Creative
				&& p.Value.GameMode != GameMode.Spectator
				&& _entity.DistanceTo(p.Value) < GetTargetDistance(p.Value)).Value;

		if (player == null)
		{
			_entity.SetTarget(null);
			return false;
		}

		_entity.SetTarget(player);

		return true;
	}

	private double GetTargetDistance(Player player)
	{
		double distance = _targetDistance;
		if (player.IsSneaking) distance *= 0.8;
		return distance;
	}

	public override void OnStart()
	{
		_targetUnseenTicks = 0;
	}

	public override bool CanContinue()
	{
		Entity target = _entity.Target;
		if (target == null) return false;
		if (target.HealthManager.IsDead) return false;


		if (target is not Player player)
		{
			if (_entity.DistanceTo(target) > _targetDistance) return false;
		}
		else
		{
			if (_entity.DistanceTo(player) > GetTargetDistance(player)) return false;
			if (_entity.CanSee(player)) _targetUnseenTicks = 0;
			else if (_targetUnseenTicks++ > 60) return false;

			_entity.SetTarget(player); // This makes sense when we to attacked by targeting
		}

		return true;
	}

	public override void OnTick(Entity[] entities)
	{
	}

	public override void OnEnd()
	{
		_entity.SetTarget(null);
	}
}

public class FindAttackableEntityTargetBehavior<TEntity> : BehaviorBase, ITargetingBehavior where TEntity : Entity
{
	private readonly Mob _entity;
	private readonly double _targetDistance;
	private readonly int _attackChance;
	private int _targetUnseenTicks = 0;
	private Entity _misssedEntity;

	public FindAttackableEntityTargetBehavior(Mob entity, double targetDistance = 16, int attackChance = 10)
	{
		_entity = entity;
		_targetDistance = targetDistance;
		_attackChance = attackChance;
	}

	public override bool ShouldStart()
	{
		if (_entity.Level.Random.Next(150) != 0)
		{
			return false;
		}

		var target = _entity.Level.Entities
			.OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.Value.KnownPosition))
			.FirstOrDefault(p =>
				p.Value != _entity
				&& p.Value is TEntity
				&& !p.Value.HealthManager.IsDead
				&& _entity.DistanceTo(p.Value) < _targetDistance).Value as TEntity;

		if (target == _misssedEntity)
		{
			return false;
		}

		if (target == null)
		{
			_entity.SetTarget(null);
			return false;
		}

		_entity.SetTarget(target);

		return true;
	}

	public override void OnStart()
	{
		_targetUnseenTicks = 0;
	}

	public override bool CanContinue()
	{
		// Give the poor entity a chance to survive
		// Also makes it let go of unreachable targets
		var target = _entity.Target;

		if (_entity.Level.Random.Next(_attackChance * 10) == 0 && _entity.DistanceTo(target) > 4)
		{
			_misssedEntity = _entity.Target;
			return false;
		}

		if (target == null)
			return false;

		if (target.HealthManager.IsDead)
			return false;


		if (_entity.DistanceTo(target) > _targetDistance)
		{
			return false;
		}

		if (_entity.CanSee(target))
		{
			_targetUnseenTicks = 0;
		}
		else if (_targetUnseenTicks++ > 60)
		{
			return false;
		}

		if (_entity is Wolf )
		{
			Wolf wolf = _entity as Wolf;
			if (wolf.DistanceTo(wolf.Owner) > 10)
			{
				return false;
			}
		}

		_entity.SetTarget(target); // This makes sense when we to attacked by targeting

		return true;
	}

	public override void OnTick(Entity[] entities)
	{
	}

	public override void OnEnd()
	{
		_entity.SetTarget(null);
	}
}