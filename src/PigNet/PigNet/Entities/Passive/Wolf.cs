using System;
using System.Numerics;
using log4net;
using PigNet.Entities.Behaviors;
using PigNet.Items;
using PigNet.Particles;
using PigNet.Utils.Metadata;
using PigNet.Worlds;
using ItemBeef = PigNet.Items.ItemBeef;
using ItemChicken = PigNet.Items.ItemChicken;
using ItemMutton = PigNet.Items.ItemMutton;

namespace PigNet.Entities.Passive;

public class Wolf : PassiveMob
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Wolf));

	public byte CollarColor { get; set; }
	public Entity Owner { get; set; }

	public Wolf(Level level) : base(EntityType.Wolf, level)
	{
		Width = Length = 0.6;
		Height = 0.8;
		IsAngry = false;
		CollarColor = 14;
		HealthManager.MaxHealth = 80;
		HealthManager.ResetHealth();
		Speed = 0.3;

		AttackDamage = 2;

		TargetBehaviors.Add(new HurtByTargetBehavior(this));
		TargetBehaviors.Add(new FindAttackableEntityTargetBehavior<Sheep>(this, 16));
		TargetBehaviors.Add(new FindAttackableEntityTargetBehavior<Rabbit>(this, 16));
		//TargetBehaviors.Add(new FindAttackableEntityTargetBehavior<Fox>(this, 16));

		Behaviors.Add(new SittingBehavior(this));
		//Behaviors.Add(new JumpAttackBehavior(this, 1.0));
		Behaviors.Add(new MeleeAttackBehavior(this, 1.0, 16));
		Behaviors.Add(new OwnerHurtByTargetBehavior(this));
		Behaviors.Add(new OwnerHurtTargetBehavior(this));
		Behaviors.Add(new FollowOwnerBehavior(this, 20, 1.0));
		Behaviors.Add(new WanderBehavior(this, 1.0));
		Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
		Behaviors.Add(new RandomLookaroundBehavior(this));
	}

	public override void DoInteraction(int actionId, Player player)
	{
		if (IsTamed)
		{
			if (Owner == player)
			{
				IsSitting = !IsSitting;
				BroadcastSetEntityData();
			}
			else
			{
				// Hmm?
			}
		}
		else
		{
			if (player.Inventory.GetItemInHand() is ItemBone)
			{
				Log.Debug($"Wolf taming attempt by {player.Username}");

				player.Inventory.RemoveItems(ItemFactory.GetIdByType<ItemBone>(), 1);

				var random = new Random();
				if (random.Next(3) == 0)
				{
					Owner = player;
					IsTamed = true;
					IsSitting = true;
					IsAngry = false;
					AttackDamage = 4;
					BroadcastSetEntityData();

					for (int i = 0; i < 7; ++i)
					{
						LegacyParticle particle = new HeartParticle(Level, random.Next(3));
						particle.Position = KnownPosition + new Vector3(0, (float) (Height + 0.85d), 0);
						particle.Spawn();
					}


					Log.Debug($"Wolf is now tamed by {player.Username}");
				}
				else
				{
					for (int i = 0; i < 7; ++i)
					{
						LegacyParticle particle = new SmokeParticle(Level);
						particle.Position = KnownPosition + new Vector3(0, (float) (Height + 0.85d), 0);
						particle.Spawn();
					}
				}
			}
		}
	}

	public override MetadataDictionary GetMetadata()
	{
		MetadataDictionary metadata = base.GetMetadata();
		metadata[(int) MetadataFlags.StructuralIntegrity] = new MetadataInt(12);
		metadata[(int) MetadataFlags.Variant] = new MetadataInt(0);
		metadata[(int) MetadataFlags.ColorIndex] = new MetadataByte(CollarColor);
		metadata[(int) MetadataFlags.AirSupply] = new MetadataShort(300);
		metadata[(int) MetadataFlags.EffectColor] = new MetadataInt(0);
		metadata[(int) MetadataFlags.Reserved009] = new MetadataByte(0);
		metadata[(int) MetadataFlags.Scale] = new MetadataLong(0);
		metadata[(int) MetadataFlags.HasNpc] = new MetadataFloat(1.0f);
		metadata[(int) MetadataFlags.ContainerType] = new MetadataShort(300);
		metadata[(int) MetadataFlags.ContainerSize] = new MetadataInt(0);
		metadata[(int) MetadataFlags.ContainerStrengthModifier] = new MetadataByte(0);
		metadata[(int) MetadataFlags.BlockTarget] = new MetadataInt(0);
		metadata[(int) MetadataFlags.CollisionBoxWidth] = new MetadataFloat(0.6f);
		metadata[(int) MetadataFlags.CollisionBoxHeight] = new MetadataFloat(0.8f);
		metadata[(int) MetadataFlags.RiderSeatPosition] = new MetadataVector3(0, 0, 0);
		metadata[(int) MetadataFlags.SeatLockPassengerRotation] = new MetadataByte(0);
		metadata[(int) MetadataFlags.SeatLockPassengerRotationDegrees] = new MetadataFloat(0f);
		metadata[(int) MetadataFlags.SeatRotationOffset] = new MetadataFloat(0f);

		if (Owner != null)
		{
			metadata[(int) MetadataFlags.Owner] = new MetadataLong(Owner.EntityId);
		}

		return metadata;
	}
}