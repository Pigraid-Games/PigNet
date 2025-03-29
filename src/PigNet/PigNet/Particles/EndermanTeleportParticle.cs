using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PigNet.Worlds;

namespace PigNet.Particles
{
	public class EndermanTeleportParticle : Particle
	{
		public EndermanTeleportParticle(Level level) : base("minecraft:eye_of_ender_bubble_particle", level)
		{

		}
	}
}
