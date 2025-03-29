#region LICENSE
// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

namespace PigNet.Net.EnumerationsTable;

public enum ActorDamageCause
{
	None = -1,
	Override = 0,
	Contact = 1,
	EntityAttack = 2,
	Projectile = 3,
	Suffocation = 4,
	Fall = 5,
	Fire = 6,
	FireTick = 7,
	Lava = 8,
	Drowning = 9,
	BlockExplosion = 10,
	EntityExplosion = 11,
	Void = 12,
	SelfDestruct = 13,
	Magic = 14,
	Wither = 15,
	Starve = 16,
	Anvil = 17,
	Thorns = 18,
	FallingBlock = 19,
	Piston = 20,
	FlyIntoWall = 21,
	Magma = 22,
	Fireworks = 23,
	Lightning = 24,
	Charging = 25,
	Temperature = 26,
	Freezing = 27,
	Stalactite = 28,
	Stalagmite = 29,
	RamAttack = 30,
	SonicBoom = 31,
	Campfire = 32,
	SoulCampfire = 33,
	All = 34
}