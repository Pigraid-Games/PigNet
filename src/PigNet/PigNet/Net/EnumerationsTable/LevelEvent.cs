﻿#region LICENSE
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

public enum LevelEvent
{
    Undefined = 0,
    
    // Sound Events (1000-1999)
    SoundClick = 1000,
    SoundClickFail = 1001,
    SoundLaunch = 1002,
    SoundOpenDoor = 1003,
    SoundFizz = 1004,
    SoundFuse = 1005,
    SoundPlayRecording = 1006,
    SoundGhastWarning = 1007,
    SoundGhastFireball = 1008,
    SoundBlazeFireball = 1009,
    SoundZombieWoodenDoor = 1010,
    SoundZombieDoorCrash = 1012,
    SoundZombieInfected = 1016,
    SoundZombieConverted = 1017,
    SoundEndermanTeleport = 1018,
    SoundAnvilBroken = 1020,
    SoundAnvilUsed = 1021,
    SoundAnvilLand = 1022,
    SoundInfinityArrowPickup = 1030,
    SoundTeleportEnderPearl = 1032,
    SoundAddItem = 1040,
    SoundItemFrameBreak = 1041,
    SoundItemFramePlace = 1042,
    SoundItemFrameRemoveItem = 1043,
    SoundItemFrameRotateItem = 1044,
    SoundExperienceOrbPickup = 1051,
    SoundTotemUsed = 1052,
    SoundArmorStandBreak = 1060,
    SoundArmorStandHit = 1061,
    SoundArmorStandLand = 1062,
    SoundArmorStandPlace = 1063,
    SoundPointedDripstoneLand = 1064,
    SoundDyeUsed = 1065,
    SoundInkSacUsed = 1066,
    SoundAmethystResonate = 1067,

    // Music Events (1900-1999)
    QueueCustomMusic = 1900,
    PlayCustomMusic = 1901,
    StopCustomMusic = 1902,
    SetMusicVolume = 1903,

    // Particle Events (2000-2999)
    ParticlesShoot = 2000,
    ParticlesDestroyBlock = 2001,
    ParticlesPotionSplash = 2002,
    ParticlesEyeOfEnderDeath = 2003,
    ParticlesMobBlockSpawn = 2004,
    ParticleCropGrowth = 2005,
    ParticleSoundGuardianGhost = 2006,
    ParticleDeathSmoke = 2007,
    ParticleDenyBlock = 2008,
    ParticleGenericSpawn = 2009,
    ParticlesDragonEgg = 2010,
    ParticlesCropEaten = 2011,
    ParticlesCrit = 2012,
    ParticlesTeleport = 2013,
    ParticlesCrackBlock = 2014,
    ParticlesBubble = 2015,
    ParticlesEvaporate = 2016,
    ParticlesDestroyArmorStand = 2017,
    ParticlesBreakingEgg = 2018,
    ParticleDestroyEgg = 2019,
    ParticlesEvaporateWater = 2020,
    ParticlesDestroyBlockNoSound = 2021,
    ParticlesKnockbackRoar = 2022,
    ParticlesTeleportTrail = 2023,
    ParticlesPointCloud = 2024,
    ParticlesExplosion = 2025,
    ParticlesBlockExplosion = 2026,
    ParticlesVibrationSignal = 2027,
    ParticlesDripstoneDrip = 2028,
    ParticlesFizzEffect = 2029,
    WaxOn = 2030,
    WaxOff = 2031,
    Scrape = 2032,
    ParticlesElectricSpark = 2033,
    ParticleTurtleEgg = 2034,
    ParticlesSculkShriek = 2035,
    SculkCatalystBloom = 2036,
    SculkCharge = 2037,
    SculkChargePop = 2038,
    SonicExplosion = 2039,
    DustPlume = 2040,

    // Weather Events (3000-3099)
    StartRaining = 3001,
    StartThunderstorm = 3002,
    StopRaining = 3003,
    StopThunderstorm = 3004,
    GlobalPause = 3005,
    SimTimeStep = 3006,
    SimTimeScale = 3007,

    // Block Events (3500-3599)
    ActivateBlock = 3500,
    CauldronExplode = 3501,
    CauldronDyeArmor = 3502,
    CauldronCleanArmor = 3503,
    CauldronFillPotion = 3504,
    CauldronTakePotion = 3505,
    CauldronFillWater = 3506,
    CauldronTakeWater = 3507,
    CauldronAddDye = 3508,
    CauldronCleanBanner = 3509,
    CauldronFlush = 3510,
    AgentSpawnEffect = 3511,
    CauldronFillLava = 3512,
    CauldronTakeLava = 3513,
    CauldronFillPowderSnow = 3514,
    CauldronTakePowderSnow = 3515,

    // Block Cracking Events (3600-3699)
    StartBlockCracking = 3600,
    StopBlockCracking = 3601,
    UpdateBlockCracking = 3602,
    ParticlesCrackBlockDown = 3603,
    ParticlesCrackBlockUp = 3604,
    ParticlesCrackBlockNorth = 3605,
    ParticlesCrackBlockSouth = 3606,
    ParticlesCrackBlockWest = 3607,
    ParticlesCrackBlockEast = 3608,
    ParticlesShootWhiteSmoke = 3609,
    ParticlesBreezeWindExplosion = 3610,
    ParticlesTrialSpawnerDetection = 3611,
    ParticlesTrialSpawnerSpawning = 3612,
    ParticlesTrialSpawnerEjecting = 3613,
    ParticlesWindExplosion = 3614,
    ParticlesTrialSpawnerDetectionCharged = 3615,
    ParticlesTrialSpawnerBecomeCharged = 3616,
    AllPlayersSleeping = 3617,
    Deprecated = 3618,

    // Special Events (9800+)
    SleepingPlayers = 9801,
    JumpPrevented = 9810,
    AnimationVaultActivate = 9811,
    AnimationVaultDeactivate = 9812,
    AnimationVaultEjectItem = 9813,
    AnimationSpawnCobweb = 9814,
    ParticleSmashAttackGroundDust = 9815,
    
    // Legacy Event
    ParticleLegacyEvent = 0x4000
}