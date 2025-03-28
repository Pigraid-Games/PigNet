#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.Net;
using MiNET.Net.Packets.Mcpe;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public enum ParticleType
	{
		Bubble = 1,
		Critical = 3,
		BlockForceField =4,
		Smoke=5,
		Explode=6,
		WhiteSmoke=7,
		Flame=8,
		Lava=10,
		LargeSmoke=11,
		Redstone=12,
		RisingRedDust=13,
		ItemBreak = 14,
		SnowballPoof = 15,
		LargeExplode = 16,
		HugeExplode = 17,
		MobFlame = 19,
		Heart = 20,
		Terrain = 21,
		TownAura = 22,
		Portal = 23,
		WaterSplash = 25,
		WaterWake = 27,
		DripWater = 28,
		DripLava = 29,
		DripHoney= 30,
		Dust = 33,
		MobSpell = 34,
		MobSpellAmbient = 35,
		MobSpellInstantaneous = 36,
		Ink = 37,
		Slime = 38,
		RainSplash = 39,
		VillagerAngry = 40,
		VillagerHappy = 41,
		EnchantmentTable = 42,
		TrackingEmitter = 43,
		Note = 44,
		WitchSpell = 45,
		Carrot = 46,
		Unknown39 = 47,
		EndRod = 48,
		DragonsBreath = 49,
		Spit = 50,
		Totem = 51,
		Food = 52,
		FireworksStarter = 53,
		FireworksSpark = 54,
		FireworksOverlay = 55,
		BalloonGas = 56,
		ColoredFlame = 57,
		Sparkler = 58,
		Conduit = 59,
		BubbleColumnUp = 60,
		BubbleColumnDown = 61,
		Sneeze = 62
	}

	public class LegacyParticle : Particle
	{
		public int Id { get; private set; }
		protected int Data { get; set; }

		public LegacyParticle(ParticleType particle, Level level): this((int)particle, level)
		{
		}

		public LegacyParticle(int id, Level level) : base(level)
		{
			Id = id;
			Level = level;
		}

		public override void Spawn(Player[] players)
		{
			var particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (LevelEventType) (0x4000 | Id);
			particleEvent.position = Position;
			particleEvent.data = Data;
			Level.RelayBroadcast(players, particleEvent);
		}
	}
}