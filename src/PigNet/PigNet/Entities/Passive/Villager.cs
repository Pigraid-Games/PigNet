using PigNet.Entities.Behaviors;
using PigNet.Utils.Metadata;
using PigNet.Worlds;

namespace PigNet.Entities.Passive;

public class Villager : PassiveMob
{
	public Villager(Level level) : base(EntityType.Villager, level)
	{
		Width = Length = 0.6;
		Height = 1.9;
		HealthManager.MaxHealth = 200;
		HealthManager.ResetHealth();
		Speed = 0.3;
		IsInLove = false;

		Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.4));

		Behaviors.Add(new WanderBehavior(this, 1.0, 40));
		Behaviors.Add(new LookAtEntityBehavior(this, 4, 20));
		Behaviors.Add(new LookAtPlayerBehavior(this, 4, 10));
		Behaviors.Add(new RandomLookaroundBehavior(this, 40));
	}

	public override MetadataDictionary GetMetadata()
	{
		Scale = IsBaby ? 0.50f : 1.0;
		MetadataDictionary metadata = base.GetMetadata();
		metadata[(int) MetadataFlags.Variant] = new MetadataInt(Variant);
		metadata[(int) MetadataFlags.ContainerType] = new MetadataShort(247);
		metadata[(int) MetadataFlags.ContainerSize] = new MetadataByte(8);
		metadata[(int) MetadataFlags.MaxTradeTier] = new MetadataInt(4);

		return metadata;
	}
}