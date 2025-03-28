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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

namespace MiNET.Net.EnumerationsTable;

public enum ActorDataIDs
{
    Reserved0 = 0,
    StructuralIntegrity = 1,
    Variant = 2,
    ColorIndex = 3,
    Name = 4,
    Owner = 5,
    Target = 6,
    AirSupply = 7,
    EffectColor = 8,
    Reserved009 = 9,
    Reserved010 = 10,
    Hurt = 11,
    HurtDir = 12,
    RowTimeLeft = 13,
    RowTimeRight = 14,
    Value = 15,
    DisplayTileRuntimeId = 16,
    DisplayOffset = 17,
    CustomDisplay = 18,
    Swell = 19,
    OldSwell = 20,
    SwellDir = 21,
    ChargeAmount = 22,
    CarryBlockRuntimeId = 23,
    ClientEvent = 24,
    UsingItem = 25,
    PlayerFlags = 26,
    PlayerIndex = 27,
    BedPosition = 28,
    XPower = 29,
    YPower = 30,
    ZPower = 31,
    AuxPower = 32,
    FishX = 33,
    FishZ = 34,
    FishAngle = 35,
    AuxValueData = 36,
    LeashHolder = 37,
    Reserved038 = 38,
    HasNpc = 39,
    NpcData = 40,
    Actions = 41,
    AirSupplyMax = 42,
    MarkVariant = 43,
    ContainerType = 44,
    ContainerSize = 45,
    ContainerStrengthModifier = 46,
    BlockTarget = 47,
    Inv = 48,
    TargetA = 49,
    TargetB = 50,
    TargetC = 51,
    AerialAttack = 52,
    Reserved053 = 53,
    Reserved054 = 54,
    FuseTime = 55,
    Reserved056 = 56,
    SeatLockPassengerRotation = 57,
    SeatLockPassengerRotationDegrees = 58,
    SeatRotationOffset = 59,
    SeatRotationOffsetDegrees = 60,
    DataRadius = 61,
    DataWaiting = 62,
    DataParticle = 63,
    PeekId = 64,
    AttachFace = 65,
    Attached = 66,
    AttachPos = 67,
    TradeTarget = 68,
    Career = 69,
    HasCommandBlock = 70,
    CommandName = 71,
    LastCommandOutput = 72,
    TrackCommandOutput = 73,
    Reserved074 = 74,
    Strength = 75,
    StrengthMax = 76,
    DataSpellCastingColor = 77,
    DataLifetimeTicks = 78,
    PoseIndex = 79,
    DataTickOffset = 80,
    NametagAlwaysShow = 81,
    Color2Index = 82,
    NameAuthor = 83,
    Score = 84,
    BalloonAnchor = 85,
    PuffedState = 86,
    BubbleTime = 87,
    Agent = 88,
    SittingAmount = 89,
    SittingAmountPrevious = 90,
    EatingCounter = 91,
    Reserved092 = 92,
    LayingAmount = 93,
    LayingAmountPrevious = 94,
    DataDuration = 95,
    DataSpawnTimeDeprecated = 96,
    DataChangeRate = 97,
    DataChangeOnPickup = 98,
    DataPickupCount = 99,
    InteractText = 100,
    TradeTier = 101,
    MaxTradeTier = 102,
    TradeExperience = 103,
    SkinId = 104,
    SpawningFrames = 105,
    CommandBlockTickDelay = 106,
    CommandBlockExecuteOnFirstTick = 107,
    AmbientSoundInterval = 108,
    AmbientSoundIntervalRange = 109,
    AmbientSoundEventName = 110,
    FallDamageMultiplier = 111,
    NameRawText = 112,
    CanRideTarget = 113,
    LowTierCuredTradeDiscount = 114,
    HighTierCuredTradeDiscount = 115,
    NearbyCuredTradeDiscount = 116,
    NearbyCuredDiscountTimeStamp = 117,
    Hitbox = 118,
    IsBuoyant = 119,
    FreezingEffectStrength = 120,
    BuoyancyData = 121,
    GoatHornCount = 122,
    BaseRuntimeId = 123,
    MovementSoundDistanceOffset = 124,
    HeartbeatIntervalTicks = 125,
    HeartbeatSoundEvent = 126,
    PlayerLastDeathPos = 127,
    PlayerLastDeathDimension = 128,
    PlayerHasDied = 129,
    CollisionBox = 130,
    VisibleMobEffects = 131,
    FilteredName = 132,
    EnterBedPosition = 133,
    Count = 134
}