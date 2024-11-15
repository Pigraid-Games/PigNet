using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MiNET.Effects;
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
			ApplyPotionEffects(playersInArea, effects);
			base.DespawnEntity();
		}

		private void ApplyPotionEffects(List<Player> players, List<Effect> effects)
		{
			if (effects == null) return;
			foreach (var player in players)
			{
				foreach (var effect in effects)
				{
					if (!GetBoundingBox().Intersects(player.GetBoundingBox() + 1))
					{
						float distance = Vector3.Distance(player.KnownPosition + new Vector3(0, 1.62f, 0), KnownPosition);
						float multiplier = (1 - (distance / 4));
						effect.Duration = (int) (effect.Duration * multiplier);
					}
					effect.Particles = true;
					player.SetEffect(effect);
				}
			}
		}

		public override MetadataDictionary GetMetadata()
		{
			return new MetadataDictionary
			{
				[(int) MetadataFlags.AuxValueData] = new MetadataShort(Metadata),
			};
		}
	}
}
