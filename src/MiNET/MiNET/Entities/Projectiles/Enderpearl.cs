using MiNET.Particles;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class Enderpearl : Projectile
	{
		public Enderpearl(Player shooter, Level level): base(shooter, EntityType.ThrownEnderPerl, level, 0)
		{
			Width = 0.25;
			Length = 0.25;
			Height = 0.25;

			Gravity = 0.03;
			Drag = 0.01;

			HealthManager.IsInvulnerable = true;
			DespawnOnImpact = true;
			BroadcastMovement = true;
			IsDispawningOnShooterImpact = false;
		}
		 
		public override void DespawnEntity()
		{
			if(Shooter == null) base.DespawnEntity();

			var endermanTeleportParticle = new EndermanTeleportParticle(Level)
			{
				Position = KnownPosition
			};
			endermanTeleportParticle.Spawn();
			KnownPosition.HeadYaw = Shooter.KnownPosition.HeadYaw;
			KnownPosition.Yaw = Shooter.KnownPosition.Yaw;
			Shooter.Teleport(KnownPosition);
			Shooter.HealthManager.TakeHit(this, 5, DamageCause.Fall);
			base.DespawnEntity();
		}
	}
}
