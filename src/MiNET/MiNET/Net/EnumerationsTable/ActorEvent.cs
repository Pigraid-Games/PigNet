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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

namespace MiNET.Net.EnumerationsTable;

public enum ActorEvent : byte
{
	None = 0,
	Jump = 1,
	Hurt = 2,
	Death = 3,
	StartAttacking = 4,
	StopAttacking = 5,
	TamingFailed = 6,
	TamingSucceeded = 7,
	ShakeWetness = 8,
	EatGrass = 10,
	FishHookBubble = 11,
	FishHookFishPos = 12,
	FishHookHookTime = 13,
	FishHookTease = 14,
	SquidFleeing = 15,
	ZombieConverting = 16,
	PlayAmbient = 17,
	SpawnAlive = 18,
	StartOfferFlower = 19,
	StopOfferFlower = 20,
	LoveHearts = 21,
	VillagerAngry = 22,
	VillagerHappy = 23,
	WitchHatMagic = 24,
	FireworksExplode = 25,
	InLoveHearts = 26,
	SilverfishMergeAnim = 27,
	GuardianAttackSound = 28,
	DrinkPotion = 29,
	ThrowPotion = 30,
	PrimeTntCart = 31,
	PrimeCreeper = 32,
	AirSupply = 33,
	AddPlayerLevels = 34,
	GuardianMiningFatigue = 35,
	AgentSwingArm = 36,
	DragonStartDeathAnim = 37,
	GroundDust = 38,
	Shake = 39,
	Feed = 57,
	BabyAge = 60,
	InstantDeath = 61,
	NotifyTrade = 62,
	LeashDestroyed = 63,
	CaravanUpdated = 64,
	TalismanActivate = 65,
	DeprecatedUpdateStructureFeature = 66,
	PlayerSpawnedMob = 67,
	Puke = 68,
	UpdateStackSize = 69,
	StartSwimming = 70,
	BalloonPop = 71,
	TreasureHunt = 72,
	SummonAgent = 73,
	FinishedChargingItem = 74,
	ActorGrowUp = 76,
	VibrationDetected = 77,
	DrinkMilk = 78
}