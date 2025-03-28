using System;
using MiNET.Blocks;
using MiNET.Net.Packets.Mcpe;
using MiNET.Sounds;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class ExperienceOrb : Entity
	{
		private static int[] orbSize = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 17, 37, 73, 149, 307, 617, 1237, 2477 };
		public short xpValue { get; set; } = 0;
		public ExperienceOrb(Level level) : base(EntityType.ExperienceOrb, level)
		{
			IsAffectedByGravity = true;
			HasCollision = true;

			Gravity = 0.04;  //idk
			Drag = 0.02;  //idk
		}

		public override MetadataDictionary GetMetadata()
		{
			if (xpValue == 0)
			{
				xpValue = (short) orbSize[new Random().Next(0, orbSize.Length)];
			}
			return new MetadataDictionary
			{
				[(int) MetadataFlags.EntityFlags] = new MetadataLong(GetDataValue()),
				[(int) MetadataFlags.ExperienceValue] = new MetadataShort(xpValue),
			};
		}

		public override void OnTick(Entity[] entities)
		{
			if (Level.GetBlock(KnownPosition) is Air)
			{
				KnownPosition.Y = KnownPosition.Y - (float) 0.2;
			}

			var players = Level.GetSpawnedPlayers();
			foreach (Player player in players)
			{
				var directionToPlayer = player.KnownPosition.ToVector3() - KnownPosition;
				var distanceToPlayer = (float) Math.Sqrt(directionToPlayer.X * directionToPlayer.X + directionToPlayer.Y * directionToPlayer.Y + directionToPlayer.Z * directionToPlayer.Z);
				if (distanceToPlayer <= 7.25)
				{
					KnownPosition += directionToPlayer * (Math.Min(0.1f, 100f / (distanceToPlayer * 1000)) / distanceToPlayer); //not 100% right but ok
				}
				if (IsColliding(player))
				{
					player.ExperienceManager.AddExperience(xpValue);
					var sound = new Sound((short) LevelEventType.SoundExperienceOrbPickup, player.KnownPosition);
					sound.Spawn(player.Level);
					DespawnEntity();
				}
			}

			var entityData = McpeMoveActor.CreateObject();
			entityData.runtimeEntityId = EntityId;
			entityData.position = KnownPosition;
			Level.RelayBroadcast(entityData);

			base.OnTick(entities);
			if (!IsSpawned) { return; }
			if (Age > 6000)
			{
				DespawnEntity();
			}
		}
	}
}