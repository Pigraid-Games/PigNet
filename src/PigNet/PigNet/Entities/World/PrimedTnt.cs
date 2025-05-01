using System;
using System.Numerics;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils.Metadata;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Entities.World;

public class PrimedTnt : Entity
{
	public byte Fuse { get; set; }
	public bool Fire { get; set; }

	public PrimedTnt(Level level) : base(EntityType.PrimedTnt, level)
	{
		IsIgnited = true;
		NoAi = false;
		HasCollision = true;

		Gravity = 0.04;
		Drag = 0.02;
	}

	public override MetadataDictionary GetMetadata()
	{
		return new MetadataDictionary
		{
			[(int) MetadataFlags.EntityFlags] = new MetadataLong(GetDataValue()),
			[(int) MetadataFlags.FuseTime] = new MetadataInt(Fuse)
		};
	}

	public override void SpawnEntity()
	{
		Fire = false; // Why false?

		base.SpawnEntity();
	}

	public override void OnTick(Entity[] entities)
	{
		Fuse--;

		if (Fuse == 0)
		{
			DespawnEntity();
			Explode();
		}
		else
		{
			PositionCheck();

			if (KnownPosition.Y > -1 && _checkPosition)
			{
				Velocity -= new Vector3(0, (float) Gravity, 0);
				Velocity *= (float) (1.0f - Drag);
			}

			var entityData = McpeSetActorData.CreateObject();
			entityData.runtimeActorId = EntityId;
			entityData.metadata = GetMetadata();
			Level.RelayBroadcast(entityData);
		}
	}

	private bool _checkPosition = true;

	private void PositionCheck()
	{
		if (Velocity.Y < -0.1)
		{
			int distance = (int) Math.Ceiling(Velocity.Length());
			BlockCoordinates check = new BlockCoordinates(KnownPosition);
			for (int i = 0; i < distance; i++)
			{
				if (Level.GetBlock(check).IsSolid)
				{
					_checkPosition = false;
					KnownPosition = check.BlockUp();
					return;
				}
				check = check.BlockDown();
			}
		}
		KnownPosition.X += (float) Velocity.X;
		KnownPosition.Y += (float) Velocity.Y;
		KnownPosition.Z += (float) Velocity.Z;
	}

	private void Explode()
	{
		// Litteral "fire and forget"
		new Explosion(Level,
				new BlockCoordinates((int) Math.Floor(KnownPosition.X), (int) Math.Floor(KnownPosition.Y), (int) Math.Floor(KnownPosition.Z)), 4, Fire)
			.Explode();
		Level.BroadcastSound(KnownPosition.ToVector3(), LevelSoundEventType.Explode);
	}
}