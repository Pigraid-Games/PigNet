using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities.World;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class SplashPotion : Projectile
	{
		public short Metadata { get; set; }
		public SplashPotion(Player shooter, Level level, short metadata) : base(shooter, EntityType.ThrownSpashPotion, level, 0)
		{
			Width = 0.25;
			Length = 0.25;
			Height = 0.25;

			Gravity = 0.15;
			Drag = 0.25;

			Damage = -1;
			HealthManager.IsInvulnerable = true;
			DespawnOnImpact = true;
			BroadcastMovement = true;
			Metadata = metadata;
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
		
		public override void DespawnEntity()
		{
			var particle = new SplashPotionParticle(Level, KnownPosition, 255, 85, 85);
			particle.Spawn();
			Level.BroadcastSound(KnownPosition, LevelSoundEventType.Glass);

			var playersInArea = new List<Player>();

			foreach (var player in Level.Players.Values)
			{
				float distanceSquared = Vector3.DistanceSquared(player.KnownPosition, KnownPosition);
				if (distanceSquared <= 16)
				{
					playersInArea.Add(player);
				}
			}

			if (playersInArea.Count <= 0)
			{
				base.DespawnEntity();
				return;
			}

			ApplyPotionEffect(playersInArea);
			base.DespawnEntity();
		}


		private void ApplyPotionEffect(List<Player> players)
		{
			Effect effect = null;
			switch (Metadata)
			{
				case 5: // Splash Potion of Night Vision (2:15)
					effect = new NightVision { Duration = 3000 };
					break;
				case 6: // Splash Potion of Night Vision (6:00)
					effect = new NightVision { Duration = 7200 };
					break;
				case 7: // Splash Potion of Invisibility (2:15)
					effect = new Invisibility { Duration = 3000 };
					break;
				case 8: // Splash Potion of Invisibility (6:00)
					effect = new Invisibility { Duration = 7200 };
					break;
				case 9: // Splash Potion of Leaping (Jump Boost 1, 2:15)
					effect = new JumpBoost { Duration = 3000, Level = 1 };
					break;
				case 10: // Splash Potion of Leaping (Jump Boost 1, 6:00)
					effect = new JumpBoost { Duration = 7200, Level = 1 };
					break;
				case 11: // Splash Potion of Leaping (Jump Boost 2, 1:07)
					effect = new JumpBoost { Duration = 1480, Level = 2 };
					break;
				case 12: // Splash Potion of Fire Resistance (2:15)
					effect = new FireResistance { Duration = 3000 };
					break;
				case 13: // Splash Potion of Fire Resistance (6:00)
					effect = new FireResistance { Duration = 7200 };
					break;
				case 14: // Splash Potion of Swiftness (Speed 1, 2:15)
					effect = new Speed { Duration = 3000, Level = 1 };
					break;
				case 15: // Splash Potion of Swiftness (Speed 1, 6:00)
					effect = new Speed { Duration = 7200, Level = 1 };
					break;
				case 16: // Splash Potion of Swiftness (Speed 2, 1:07)
					effect = new Speed { Duration = 1480, Level = 2 };
					break;
				case 17: // Splash Potion of Slowness (1:07)
					effect = new Slowness { Duration = 1480 };
					break;
				case 18: // Splash Potion of Slowness (3:00)
					effect = new Slowness { Duration = 3600 };
					break;
				case 19: // Splash Potion of Water Breathing (2:15)
					effect = new WaterBreathing { Duration = 3000 };
					break;
				case 20: // Splash Potion of Water Breathing (6:00)
					effect = new WaterBreathing { Duration = 7200 };
					break;
				case 21: // Splash Potion of Healing
					effect = new InstantHealth { Level = 1 };
					break;
				case 22: // Splash Potion of healing 2
					effect = new InstantHealth { Level = 2 };
					break;
				case 23: // Splash Potion of Harming
					effect = new InstantDamage { Level = 1 };
					break;
				case 24: // Splash Potion of Harming 2
					effect = new InstantDamage { Level = 2 };
					break;
				case 25: // Splash Potion of Poison (Poison 1, 0:33)
					effect = new Poison { Duration = 1320, Level = 1 };
					break;
				case 26: // Splash Potion of Poison (Poison 1, 1:30)
					effect = new Poison { Duration = 2400, Level = 1 };
					break;
				case 27: // Splash Potion of Poison (Poison 2, 0:16)
					effect = new Poison { Duration = 640, Level = 2 };
					break;
				case 28: // Splash Potion of Regeneration (Regen 1, 0:33)
					effect = new Regeneration { Duration = 1320, Level = 1 };
					break;
				case 29: // Splash Potion of Regeneration (Regen 1, 1:30)
					effect = new Regeneration { Duration = 2400, Level = 1 };
					break;
				case 30: // Splash Potion of Regeneration (Regen 2, 0:16)
					effect = new Regeneration { Duration = 640, Level = 2 };
					break;
				case 31: // Splash Potion of Strength (Strength 1, 2:15)
					effect = new Strength { Duration = 3000, Level = 1 };
					break;
				case 32: // Splash Potion of Strength (Strength 1, 6:00)
					effect = new Strength { Duration = 7200, Level = 1 };
					break;
				case 33: // Splash Potion of Strength (Strength 2, 1:07)
					effect = new Strength { Duration = 1480, Level = 2 };
					break;
				case 34: // Splash Potion of Weakness (1:07)
					effect = new Weakness { Duration = 1480 };
					break;
				case 35: // Splash Potion of Weakness (3:00)
					effect = new Weakness { Duration = 3600 };
					break;
				case 36: // Splash Potion of Decay (Wither 2, 0:30)
					effect = new Wither { Duration = 1200, Level = 2 };
					break;
				case 37: // Splash Potion of the Turtle Master (Slowness 4 & Resistance 3, 0:15)
				case 38: // Splash Potion of the Turtle Master (Slowness 4 & Resistance 3, 0:30)
				case 39: // Splash Potion of the Turtle Master (Slowness 6 & Resistance 4, 0:15)
					//ApplyTurtleMasterEffect(players, Metadata);
					// TODO: Add the EffectType.TurtleMaster
					return;
				case 40: // Splash Potion of Slow Falling (1:07)
					// TODO: Add the SlowFalling Effect
					//effect = new SlowFalling { Duration = 1480 };
					break;
				case 41: // Splash Potion of Slow Falling (3:00)
					// TODO: Add the SlowFalling Effect
					//effect = new SlowFalling { Duration = 3600 };
					break;
			}

			if (effect == null) return;
			effect.Particles = true;
			foreach (var player in players)
			{
				player.SetEffect(effect);
			}
		}

		private Effect[] ApplyTurtleMasterEffect(List<Player> players)
		{
			Effect slowness, resistance;

			switch (Metadata)
			{
				case 37:
					slowness = new Slowness { Duration = 600, Level = 4 };
					resistance = new Resistance { Duration = 600, Level = 3 };
					break;
				case 38:
					slowness = new Slowness { Duration = 1200, Level = 4 };
					resistance = new Resistance { Duration = 1200, Level = 3 };
					break;
				case 39:
					slowness = new Slowness { Duration = 600, Level = 6 };
					resistance = new Resistance { Duration = 600, Level = 4 };
					break;
				default:
					return [];
			}
			return [slowness, resistance];
		}
	}
}
