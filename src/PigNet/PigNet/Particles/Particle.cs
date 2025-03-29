using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using PigNet.Net;
using PigNet.Net.Packets.Mcpe;
using PigNet.Worlds;

namespace PigNet.Particles
{
	public class Particle
	{
		protected string Name { get; set; }
		protected Level Level { get; set; }
		public Vector3 Position { get; set; }

		public Particle(string name, Level level) : this(level)
		{
			Name = name;
		}
		
		public Particle(Level level)
		{
			Level = level;
		}

		public virtual void Spawn(Player[] players)
		{
			var pk = McpeSpawnParticleEffect.CreateObject();
			pk.particleName = Name;
			pk.position = Position;
			pk.dimensionId = 0;
			pk.entityId = -1;
			Level.RelayBroadcast(players, pk);
		}

		public virtual void Spawn()
		{
			Spawn(Level.GetAllPlayers());
		}
	}
}
