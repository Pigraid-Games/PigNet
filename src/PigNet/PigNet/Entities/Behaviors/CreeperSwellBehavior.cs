using PigNet.Entities.Hostile;

namespace PigNet.Entities.Behaviors;

public class CreeperSwellBehavior : BehaviorBase
{
	private readonly Creeper _entity;

	public CreeperSwellBehavior(Creeper entity)
	{
		this._entity = entity;
	}

	public override bool ShouldStart()
	{
		if (_entity.Target == null) return false;

		return (_entity.IsIgnited || _entity.DistanceTo(_entity.Target) < 3);
	}

	public override bool CanContinue()
	{
		return ShouldStart();
	}


	public override void OnTick(Entity[] entities)
	{
		if (_entity.Target == null)
		{
			_entity.Prime(false);
		}
		else if (_entity.DistanceTo(_entity.Target) > 7)
		{
			_entity.Prime(false);
		}
		else if (!_entity.CanSee(_entity.Target))
		{
			_entity.Prime(false);
		}
		else
		{
			_entity.Prime(true);
		}
	}

	public override void OnEnd()
	{
		_entity.SetTarget(null);
	}
}