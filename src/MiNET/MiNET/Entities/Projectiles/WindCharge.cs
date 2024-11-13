using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MiNET.Blocks;
using MiNET.Entities.World;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class WindCharge : Projectile
	{
		public WindCharge(Player shooter, Level level) : base(shooter, EntityType.WindCharge, level, 0)
		{
			Width = 0.25;
			Length = 0.25;
			Height = 0.25;

			Gravity = 0;
			Drag = 0;
			Ttl = 5000;

			HealthManager.IsInvulnerable = true;
			DespawnOnImpact = true;
			BroadcastMovement = true;
		}

		public override void DespawnEntity()
		{
			base.DespawnEntity();

			var particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (short) LevelEventType.WindExplosion;
			particleEvent.position = KnownPosition;
			particleEvent.data = Data;
			Level.RelayBroadcast(Level.GetAllPlayers(), particleEvent);
			Level.BroadcastSound(KnownPosition, LevelSoundEventType.WindChargeBurst);
			ApplyEffect();
		}

		protected override void OnHitEntity(Entity entityCollided)
		{
			base.OnHitEntity(entityCollided);
			entityCollided.HealthManager.TakeHit(Shooter, 1, DamageCause.EntityExplosion);

			Vector3 explosionPoint = KnownPosition;
			Vector3 direction = entityCollided.KnownPosition - explosionPoint;
			direction = direction.Normalize() * (float)1.5;

			entityCollided.Knockback(direction);
		}

		private void ApplyEffect()
		{
			var playersInArea = new List<Player>();
			foreach (var player in Level.Players.Values)
			{
				float distanceSquared = Vector3.DistanceSquared(player.KnownPosition, KnownPosition);
				if (distanceSquared <= 16)
				{
					playersInArea.Add(player);
				}
			}

			foreach (var player in playersInArea)
			{
				Vector3 explosionPoint = KnownPosition;
				Vector3 direction = player.KnownPosition - explosionPoint;
				direction = direction.Normalize() * (float) 1.5;
				player.Knockback(direction);
			}
		}

		public override void OnTick(Entity[] entities)
		{
			//base.OnTick(entities);

			if (KnownPosition.Y <= -16
				|| (Velocity.Length() <= 0 && DespawnOnImpact)
				|| (Velocity.Length() <= 0 && !DespawnOnImpact && Ttl <= 0))
			{
				if (DespawnOnImpact || (!DespawnOnImpact && Ttl <= 0))
				{
					DespawnEntity();
					return;
				}

				return;
			}

			Ttl--;

			if (KnownPosition.Y <= 0 || Velocity.Length() <= 0)
				return;

			Entity entityCollided = CheckEntityCollide(KnownPosition, Velocity);
			if (entityCollided is Player playerCollided)
			{
				if (playerCollided == Shooter)
					return;
			}

			bool collided = false;
			Block collidedWithBlock = null;
			if (entityCollided != null && Damage >= 0)
			{
				double speed = Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y + Velocity.Z * Velocity.Z);
				double damage = Math.Ceiling(speed * Damage);
				if (IsCritical)
				{
					damage += Level.Random.Next((int) (damage / 2 + 2));

					McpeAnimate animate = McpeAnimate.CreateObject();
					animate.runtimeEntityId = entityCollided.EntityId;
					animate.actionId = 4;
					Level.RelayBroadcast(animate);
				}

				if (PowerLevel > 0)
				{
					damage = damage + ((PowerLevel + 1) * 0.25);
				}

				Player player = entityCollided as Player;

				if (player != null)
				{
					damage = player.DamageCalculator.CalculatePlayerDamage(this, player, null, damage, DamageCause.Projectile);
					player.LastAttackTarget = entityCollided;
				}

				entityCollided.HealthManager.TakeHit(this, (int) damage, DamageCause.Projectile);
				entityCollided.HealthManager.LastDamageSource = Shooter;
				OnHitEntity(entityCollided);
				if (entityCollided is not ExperienceOrb)
				{ DespawnEntity(); } //todo add collision values
				return;
			}
			else if (entityCollided != null && Damage == -1)
			{
				entityCollided.HealthManager.LastDamageSource = Shooter;
				OnHitEntity(entityCollided);
				if (entityCollided is not ExperienceOrb)
				{ DespawnEntity(); } //todo add collision values
			}
			else
			{
				var velocity2 = Velocity;
				velocity2 *= (float) (1.0d - Drag);
				velocity2 -= new Vector3(0, (float) Gravity, 0);
				double distance = velocity2.Length();
				velocity2 = Vector3.Normalize(velocity2) / 2;

				for (int i = 0; i < Math.Ceiling(distance) * 2; i++)
				{
					Vector3 nextPos = KnownPosition.ToVector3();
					nextPos.X += (float) velocity2.X * i;
					nextPos.Y += (float) velocity2.Y * i;
					nextPos.Z += (float) velocity2.Z * i;

					Block block = Level.GetBlock(nextPos);
					collided = block.IsSolid && block.GetBoundingBox().Contains(nextPos);
					if (collided)
					{
						SetIntersectLocation(block.GetBoundingBox(), KnownPosition.ToVector3());
						collidedWithBlock = block;
						break;
					}
				}
			}

			bool sendPosition = Velocity != Vector3.Zero;

			if (collided)
			{
				Velocity = Vector3.Zero;
			}
			else
			{
				KnownPosition.X += (float) Velocity.X;
				KnownPosition.Y += (float) Velocity.Y;
				KnownPosition.Z += (float) Velocity.Z;

				Velocity *= (float) (1.0 - Drag);
				Velocity -= new Vector3(0, (float) Gravity, 0);
				Velocity += Force;

				KnownPosition.Yaw = (float) Velocity.GetYaw();
				KnownPosition.Pitch = (float) Velocity.GetPitch();
			}

			// For debugging of flight-path
			if (sendPosition && BroadcastMovement)
			{
				//LastUpdatedTime = DateTime.UtcNow;

				BroadcastMoveAndMotion();
			}

			if (collided)
			{
				OnHitBlock(collidedWithBlock);
			}
		}

		private Entity CheckEntityCollide(Vector3 position, Vector3 direction)
		{
			float Distance = 2.0f;

			Vector3 offsetPosition = position + Vector3.Normalize(direction) * Distance;

			Ray2 ray = new Ray2 { x = offsetPosition, d = Vector3.Normalize(direction) };

			var entities = Level.Entities.Values.Concat(Level.GetSpawnedPlayers()).OrderBy(entity => Vector3.Distance(position, entity.KnownPosition.ToVector3()));
			foreach (Entity entity in entities)
			{
				if (entity == this)
					continue;
				if (entity is Projectile)
					continue; // This should actually be handled for some projectiles
				if (entity is Player player && player.GameMode == GameMode.Spectator)
					continue;

				if (Intersect(entity.GetBoundingBox() + HitBoxPrecision, ray))
				{
					if (ray.tNear > direction.Length())
						break;

					Vector3 p = ray.x + new Vector3((float) ray.tNear) * ray.d;
					KnownPosition = new PlayerLocation(p.X, p.Y, p.Z);
					return entity;
				}
			}

			return null;
		}

		private bool SetIntersectLocation(BoundingBox bbox, Vector3 location)
		{
			Ray ray = new Ray(location - Velocity, Vector3.Normalize(Velocity));
			double? distance = ray.Intersects(bbox);
			if (distance != null)
			{
				float dist = (float) distance - 0.1f;
				Vector3 pos = ray.Position + (ray.Direction * new Vector3(dist));
				KnownPosition.X = pos.X;
				KnownPosition.Y = pos.Y;
				KnownPosition.Z = pos.Z;
				return true;
			}

			return false;
		}

		private void BroadcastMoveAndMotion()
		{
			if (new Random().Next(5) == 0)
			{
				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.runtimeEntityId = EntityId;
				motions.velocity = Velocity;
				Level.RelayBroadcast(motions);
			}

			if (LastSentPosition != null)
			{
				McpeMoveEntityDelta move = McpeMoveEntityDelta.CreateObject();
				move.runtimeEntityId = EntityId;
				move.prevSentPosition = LastSentPosition;
				move.currentPosition = (PlayerLocation) KnownPosition.Clone();
				move.isOnGround = IsWalker && IsOnGround;
				if (move.SetFlags())
				{
					Level.RelayBroadcast(move);
				}
			}

			LastSentPosition = (PlayerLocation) KnownPosition.Clone(); // Used for delta

			if (Shooter != null && IsCritical)
			{
				var particle = new CriticalParticle(Level);
				particle.Position = KnownPosition.ToVector3();
				particle.Spawn(new[] { Shooter });
			}
		}
	}
}
