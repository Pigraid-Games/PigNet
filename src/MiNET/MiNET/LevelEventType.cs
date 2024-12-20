﻿#region LICENSE

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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

namespace MiNET;

public enum LevelEventType : short
{
	Undefined = 0,
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
	SoundCameraTakePicture = 1050,
	SoundExperienceOrbPickup = 1051,
	SoundTotemUsed = 1052,
	SoundArmorStandBreak = 1060,
	SoundArmorStandHit = 1061,
	SoundArmorStandLand = 1062,
	SoundArmorStandPlace = 1063,
	SoundPointedDripstoneLand = 1064,
	SoundDyeUsed = 1065,
	SoundInkSacUsed = 1066,
	QueueCustomMusic = 1900,
	PlayCustomMusic = 1901,
	StopCustomMusic = 1902,
	SetMusicVolume = 1903,
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
	ParticlesCritical = 2012,
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
	ParticleSculkShriek = 2035,
	SculkCatalystBloom = 2036,
	SculkCharge = 2037,
	SculkChargePop = 2038,
	SonicExplosion = 2039,
	StartRaining = 3001,
	StartThunderstorm = 3002,
	StopRaining = 3003,
	StopThunderstorm = 3004,
	GlobalPause = 3005,
	SimTimeStep = 3006,
	SimTimeScale = 3007,
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
	StartBlockCracking = 3600,
	StopBlockCracking = 3601,
	UpdateBlockCracking = 3602,
	BreezeWindExplosion = 3610,
	WindExplosion = 3614,
	AllPlayersSleeping = 9800,
	SleepingPlayers = 9801,
	JumpPrevented = 9810,
	ParticleLegacyEvent = 0x4000
}

public enum LevelSoundEventType
{
	ItemUseOn = 0,
	Hit = 1,
	Step = 2,
	Fly = 3,
	Jump = 4,
	Break = 5,
	Place = 6,
	HeavyStep = 7,
	Gallop = 8,
	Fall = 9,
	Ambient = 10,
	AmbientBaby = 11,
	AmbientInWater = 12,
	Breathe = 13,
	Death = 14,
	DeathInWater = 15,
	DeathToZombie = 16,
	Hurt = 17,
	HurtInWater = 18,
	Mad = 19,
	Boost = 20,
	Bow = 21,
	SquishBig = 22,
	SquishSmall = 23,
	FallBig = 24,
	FallSmall = 25,
	Splash = 26,
	Fizz = 27,
	Flap = 28,
	Swim = 29,
	Drink = 30,
	Eat = 31,
	Takeoff = 32,
	Shake = 33,
	Plop = 34,
	Land = 35,
	Saddle = 36,
	Armor = 37,
	MobArmorStandPlace = 38,
	AddChest = 39,
	Throw = 40,
	Attack = 41,
	AttackNodamage = 42,
	AttackStrong = 43,
	Warn = 44,
	Shear = 45,
	Milk = 46,
	Thunder = 47,
	Explode = 48,
	Fire = 49,
	Ignite = 50,
	Fuse = 51,
	Stare = 52,
	Spawn = 53,
	Shoot = 54,
	BreakBlock = 55,
	Launch = 56,
	Blast = 57,
	LargeBlast = 58,
	Twinkle = 59,
	Remedy = 60,
	Unfect = 61,
	Levelup = 62,
	BowHit = 63,
	BulletHit = 64,
	ExtinguishFire = 65,
	ItemFizz = 66,
	ChestOpen = 67,
	ChestClosed = 68,
	ShulkerboxOpen = 69,
	ShulkerboxClosed = 70,
	EnderchestOpen = 71,
	EnderchestClosed = 72,
	PowerOn = 73,
	PowerOff = 74,
	Attach = 75,
	Detach = 76,
	Deny = 77,
	Tripod = 78,
	Pop = 79,
	DropSlot = 80,
	Note = 81,
	Thorns = 82,
	PistonIn = 83,
	PistonOut = 84,
	Portal = 85,
	Water = 86,
	LavaPop = 87,
	Lava = 88,
	Burp = 89,
	BucketFillWater = 90,
	BucketFillLava = 91,
	BucketEmptyWater = 92,
	BucketEmptyLava = 93,
	ArmorEquipChain = 94,
	ArmorEquipDiamond = 95,
	ArmorEquipGeneric = 96,
	ArmorEquipGold = 97,
	ArmorEquipIron = 98,
	ArmorEquipLeather = 99,
	ArmorEquipElytra = 100,
	Record13 = 101,
	RecordCat = 102,
	RecordBlocks = 103,
	RecordChirp = 104,
	RecordFar = 105,
	RecordMall = 106,
	RecordMellohi = 107,
	RecordStal = 108,
	RecordStrad = 109,
	RecordWard = 110,
	Record11 = 111,
	RecordWait = 112,
	RecordNull = 113,
	Flop = 114,
	ElderguardianCurse = 115,
	MobWarning = 116,
	MobWarningBaby = 117,
	Teleport = 118,
	ShulkerOpen = 119,
	ShulkerClose = 120,
	Haggle = 121,
	HaggleYes = 122,
	HaggleNo = 123,
	HaggleIdle = 124,
	Chorusgrow = 125,
	Chorusdeath = 126,
	Glass = 127,
	PotionBrewed = 128,
	CastSpell = 129,
	PrepareAttack = 130,
	PrepareSummon = 131,
	PrepareWololo = 132,
	Fang = 133,
	Charge = 134,
	CameraTakePicture = 135,
	LeashknotPlace = 136,
	LeashknotBreak = 137,
	Growl = 138,
	Whine = 139,
	Pant = 140,
	Purr = 141,
	Purreow = 142,
	DeathMinVolume = 143,
	DeathMidVolume = 144,
	ImitateBlaze = 145,
	ImitateCaveSpider = 146,
	ImitateCreeper = 147,
	ImitateElderGuardian = 148,
	ImitateEnderDragon = 149,
	ImitateEnderman = 150,
	ImitateEndermite = 151,
	ImitateEvocationIllager = 152,
	ImitateGhast = 153,
	ImitateHusk = 154,
	ImitateIllusionIllager = 155,
	ImitateMagmaCube = 156,
	ImitatePolarBear = 157,
	ImitateShulker = 158,
	ImitateSilverfish = 159,
	ImitateSkeleton = 160,
	ImitateSlime = 161,
	ImitateSpider = 162,
	ImitateStray = 163,
	ImitateVex = 164,
	ImitateVindicationIllager = 165,
	ImitateWitch = 166,
	ImitateWither = 167,
	ImitateWitherSkeleton = 168,
	ImitateWolf = 169,
	ImitateZombie = 170,
	ImitateZombiePigman = 171,
	ImitateZombieVillager = 172,
	BlockEndPortalFrameFill = 173,
	BlockEndPortalSpawn = 174,
	RandomAnvilUse = 175,
	BottleDragonbreath = 176,
	PortalTravel = 177,
	ItemTridentHit = 178,
	ItemTridentReturn = 179,
	ItemTridentRiptide1 = 180,
	ItemTridentRiptide2 = 181,
	ItemTridentRiptide3 = 182,
	ItemTridentThrow = 183,
	ItemTridentThunder = 184,
	ItemTridentHitGround = 185,
	Default = 186,
	BlockFletchingTableUse = 187,
	ElemconstructOpen = 188,
	IcebombHit = 189,
	Balloonpop = 190,
	LtReactionIcebomb = 191,
	LtReactionBleach = 192,
	LtReactionEpaste = 193,
	LtReactionEpaste2 = 194,
	LtReactionGlowStick = 195,
	LtReactionGlowStick2 = 196,
	LtReactionLuminol = 197,
	LtReactionSalt = 198,
	LtReactionFertilizer = 199,
	LtReactionFireball = 200,
	LtReactionMgsalt = 201,
	LtReactionMiscfire = 202,
	LtReactionFire = 203,
	LtReactionMiscexplosion = 204,
	LtReactionMiscmystical = 205,
	LtReactionMiscmystical2 = 206,
	LtReactionProduct = 207,
	SparklerUse = 208,
	GlowstickUse = 209,
	SparklerActive = 210,
	ConvertToDrowned = 211,
	BucketFillFish = 212,
	BucketEmptyFish = 213,
	BubbleUp = 214,
	BubbleDown = 215,
	BubblePop = 216,
	BubbleUpinside = 217,
	BubbleDowninside = 218,
	HurtBaby = 219,
	DeathBaby = 220,
	StepBaby = 221,
	SpawnBaby = 222,
	Born = 223,
	BlockTurtleEggBreak = 224,
	BlockTurtleEggCrack = 225,
	BlockTurtleEggHatch = 226,
	LayEgg = 227,
	BlockTurtleEggAttack = 228,
	BeaconActivate = 229,
	BeaconAmbient = 230,
	BeaconDeactivate = 231,
	BeaconPower = 232,
	ConduitActivate = 233,
	ConduitAmbient = 234,
	ConduitAttack = 235,
	ConduitDeactivate = 236,
	ConduitShort = 237,
	Swoop = 238,
	BlockBambooSaplingPlace = 239,
	Presneeze = 240,
	Sneeze = 241,
	AmbientTame = 242,
	Scared = 243,
	BlockScaffoldingClimb = 244,
	CrossbowLoadingStart = 245,
	CrossbowLoadingMiddle = 246,
	CrossbowLoadingEnd = 247,
	CrossbowShoot = 248,
	CrossbowQuickChargeStart = 249,
	CrossbowQuickChargeMiddle = 250,
	CrossbowQuickChargeEnd = 251,
	AmbientAggressive = 252,
	AmbientWorried = 253,
	CantBreed = 254,
	ItemShieldBlock = 255,
	ItemBookPut = 256,
	BlockGrindstoneUse = 257,
	BlockBellHit = 258,
	BlockCampfireCrackle = 259,
	Roar = 260,
	Stun = 261,
	BlockSweetBerryBushHurt = 262,
	BlockSweetBerryBushPick = 263,
	BlockComposterEmpty = 266,
	BlockComposterFill = 267,
	BlockComposterFillSuccess = 268,
	BlockComposterReady = 269,
	BlockBarrelOpen = 270,
	BlockBarrelClose = 271,
	RaidHorn = 272,
	BlockLoomUse = 273,
	AmbientInRaid = 274,
	UiCartographyTableTakeResult = 275,
	UiStonecutterTakeResult = 276,
	UiLoomTakeResult = 277,
	BlockSmokerSmoke = 278,
	BlockBlastFurnaceFireCrackle = 279,
	BlockSmithingTableUse = 280,
	Screech = 281,
	Sleep = 282,
	BlockFurnaceLit = 283,
	ConvertMooshroom = 284,
	MilkSuspiciously = 285,
	Celebrate = 286,
	JumpPrevent = 287,
	AmbientPollinate = 288,
	BlockBeehiveDrip = 289,
	BlockBeehiveEnter = 290,
	BlockBeehiveExit = 291,
	BlockBeehiveWork = 292,
	BlockBeehiveShear = 293,
	DrinkHoney = 294,
	AmbientCave = 295,
	Retreat = 296,
	ConvertedToZombified = 297,
	Admire = 298,
	StepLava = 299,
	Tempt = 300,
	Panic = 301,
	Angry = 302,
	AmbientWarpedForestMood = 303,
	AmbientSoulSandValleyMood = 304,
	AmbientNetherWastesMood = 305,
	AmbientBasaltDeltasMood = 306,
	AmbientCrimsonForestMood = 307,
	RespawnAnchorCharge = 308,
	RespawnAnchorDeplete = 309,
	RespawnAnchorSetSpawn = 310,
	RespawnAnchorAmbient = 311,
	ParticleSoulEscapeQuiet = 312,
	ParticleSoulEscapeLoud = 313,
	RecordPigstep = 314,
	LodestoneCompassLinkCompassToLodestone = 315,
	SmithingTableUse = 316,
	ArmorEquipNetherite = 317,
	AmbientWarpedForestLoop = 318,
	AmbientSoulSandValleyLoop = 319,
	AmbientNetherWastesLoop = 320,
	AmbientBasaltDeltasLoop = 321,
	AmbientCrimsonForestLoop = 322,
	AmbientWarpedForestAdditions = 323,
	AmbientSoulSandValleyAdditions = 324,
	AmbientNetherWastesAdditions = 325,
	AmbientBasaltDeltasAdditions = 326,
	AmbientCrimsonForestAdditions = 327,
	PowerOnSculkSensor = 328,
	PowerOffSculkSensor = 329,
	BucketFillPowderSnow = 330,
	BucketEmptyPowderSnow = 331,
	CauldronDripWaterPointedDripstone = 332,
	CauldronDripLavaPointedDripstone = 333,
	DripWaterPointedDripstone = 334,
	DripLavaPointedDripstone = 335,
	PickBerriesCaveVines = 336,
	TiltDownBigDripleaf = 337,
	TiltUpBigDripleaf = 338,
	CopperWaxOn = 339,
	CopperWaxOff = 340,
	Scrape = 341,
	MobPlayerHurtDrown = 342,
	MobPlayerHurtOnFire = 343,
	MobPlayerHurtFreeze = 344,
	ItemSpyglassUse = 345,
	ItemSpyglassStopUsing = 346,
	ChimeAmethystBlock = 347,
	AmbientScreamer = 348,
	HurtScreamer = 349,
	DeathScreamer = 350,
	MilkScreamer = 351,
	JumpToBlock = 352,
	PreRam = 353,
	PreRamScreamer = 354,
	RamImpact = 355,
	RamImpactScreamer = 356,
	SquidInkSquirt = 357,
	GlowSquidInkSquirt = 358,
	ConvertToStray = 359,
	CakeAddCandle = 360,
	ExtinguishCandle = 361,
	AmbientCandle = 362,
	BlockClick = 363,
	BlockClickFail = 364,
	BlockSculkCatalystBloom = 365,
	BlockSculkShriekerShriek = 366,
	NearbyClose = 367,
	NearbyCloser = 368,
	NearbyClosest = 369,
	Agitated = 370,
	RecordOtherside = 371,
	Tongue = 372,
	IronGolemCrack = 373,
	IronGolemRepair = 374,
	Listening = 375,
	Heartbeat = 376,
	HornBreak = 377,
	BlockSculkSpread = 379,
	ChargeSculk = 380,
	BlockSculkSensorPlace = 381,
	BlockSculkShriekerPlace = 382,
	HornCall0 = 383,
	HornCall1 = 384,
	HornCall2 = 385,
	HornCall3 = 386,
	HornCall4 = 387,
	HornCall5 = 388,
	HornCall6 = 389,
	HornCall7 = 390,
	ImitateWarden = 426,
	ListeningAngry = 427,
	ItemGiven = 428,
	ItemTaken = 429,
	Disappeared = 430,
	Reappeared = 431,
	DrinkMilk = 432,
	BlockFrogSpawnHatch = 433,
	LaySpawn = 434,
	BlockFrogSpawnBreak = 435,
	SonicBoom = 436,
	SonicCharge = 437,
	ItemThrown = 438,
	Record5 = 439,
	ConvertToFrog = 440,
	BlockEnchantingTableUse = 442,
	StepSand = 443,
	DashReady = 444,
	BundleDropContents = 445,
	BundleInsert = 446,
	BundleRemoveOne = 447,
	PressurePlateOff = 448,
	PressurePlateOn = 449,
	ButtonOff = 450,
	ButtonOn = 451,
	DoorOpen = 452,
	DoorClose = 453,
	TrapdoorOpen = 454,
	TrapdoorClose = 455,
	FenceGateOpen = 456,
	FenceGateClose = 457,
	Insert = 458,
	Pickup = 459,
	InsertEnchanted = 460,
	PickupEnchanted = 461,
	Brush = 462,
	BrushCompleted = 463,
	ShatterPot = 464,
	BreakPot = 465,
	BlockSnifferEggCrack = 466,
	BlockSnifferEggHatch = 467,
	BlockSignWaxedInteractFail = 468,
	RecordRelic = 469,
	NoteBass = 470,
	PumpkinCarve = 471,
	MobHuskConvertToZombie = 472,
	MobPigDeath = 473,
	MobHoglinConvertedToZombified = 474,
	AmbientUnderwaterEnter = 475,
	AmbientUnderwaterExit = 476,
	BottleFill = 477,
	BottleEmpty = 478,
	CrafterCraft = 479,
	CrafterFail = 480,
	BlockDecoratedPotInsert = 481,
	BlockDecoratedPotInsertFail = 482,
	CrafterDisableSlot = 483,
	TrialSpawnerOpenShutter = 484,
	TrialSpawnerEjectItem = 485,
	TrialSpawnerDetectPlayer = 486,
	TrialSpawnerSpawnMob = 487,
	TrialSpawnerCloseShutter = 488,
	TrialSpawnerAmbient = 489,
	BlockCopperBulbTurnOn = 490,
	BlockCopperBulbTurnOff = 491,
	AmbientInAir = 492,
	BreezeWindChargeBurst = 493,
	ImitateBreeze = 494,
	MobArmadilloBrush = 495,
	MobArmadilloScuteDrop = 496,
	ArmorEquipWolf = 497,
	ArmorUnequipWolf = 498,
	Reflect = 499,
	VaultOpenShutter = 500,
	VaultCloseShutter = 501,
	VaultEjectItem = 502,
	VaultInsertItem = 503,
	VaultInsertItemFail = 504,
	VaultAmbient = 505,
	VaultActivate = 506,
	VaultDeactivate = 507,
	HurtReduced = 508,
	WindChargeBurst = 509,
	ImitateBogged = 510,
	ArmorCrackWolf = 511,
	ArmorBreakWolf = 512,
	ArmorRepairWolf = 513,
	MaceSmashAir = 514,
	MaceSmashGround = 515,
	TrialSpawnerChargeActivate = 516,
	TrialSpawnerAmbientOminous = 517,
	OminousItemSpawnerSpawnItem = 518,
	OminousBottleEndUse = 519,
	MaceHeavySmashGround = 520,
	OminousItemSpawnerSpawnItemBegin = 521,
	ApplyEffectBadOmen = 523,
	ApplyEffectRaidOmen = 524,
	ApplyEffectTrialOmen = 525,
	OminousItemSpawnerAboutToSpawnItem = 526,
	RecordCreator = 527,
	RecordCreatorMusicBox = 528,
	RecordPrecipice = 529,
	VaultRejectRewardedPlayer = 530,
	ImitateDrowned = 531,
	ImitateCreaking = 532,
	BundleInsertFail = 533,
	SpongeAbsorb = 534,
	BlockCreakingHeartTrail = 536,
	CreakingHeartSpawn = 537,
	Activate = 538,
	Deactivate = 539,
	Freeze = 540,
	Unfreeze = 541,
	Open = 542,
	OpenLong = 543,
	Close = 544,
	CloseLong = 545
}