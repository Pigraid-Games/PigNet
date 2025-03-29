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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2024 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Numerics;

namespace PigNet.Sounds;

public class ItemUseOnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemUseOn, position, pitch);

public class HitSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Hit, position, pitch);

public class StepSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Step, position, pitch);

public class FlySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Fly, position, pitch);

public class JumpSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Jump, position, pitch);

public class BreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Break, position, pitch);

public class PlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Place, position, pitch);

public class HeavyStepSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HeavyStep, position, pitch);

public class GallopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Gallop, position, pitch);

public class FallSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Fall, position, pitch);

public class AmbientSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Ambient, position, pitch);

public class AmbientBabySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientBaby, position, pitch);

public class AmbientInWaterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientInWater, position, pitch);

public class BreatheSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Breathe, position, pitch);

public class DeathSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Death, position, pitch);

public class DeathInWaterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DeathInWater, position, pitch);

public class DeathToZombieSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DeathToZombie, position, pitch);

public class HurtSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Hurt, position, pitch);

public class HurtInWaterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HurtInWater, position, pitch);

public class MadSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Mad, position, pitch);

public class BoostSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Boost, position, pitch);

public class BowSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Bow, position, pitch);

public class SquishBigSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SquishBig, position, pitch);

public class SquishSmallSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SquishSmall, position, pitch);

public class FallBigSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.FallBig, position, pitch);

public class FallSmallSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.FallSmall, position, pitch);

public class SplashSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Splash, position, pitch);

public class FlapSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Flap, position, pitch);

public class SwimSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Swim, position, pitch);

public class DrinkSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Drink, position, pitch);

public class EatSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Eat, position, pitch);

public class TakeoffSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Takeoff, position, pitch);

public class ShakeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Shake, position, pitch);

public class PlopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Plop, position, pitch);

public class LandSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Land, position, pitch);

public class SaddleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Saddle, position, pitch);

public class ArmorSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Armor, position, pitch);

public class MobArmorStandPlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobArmorStandPlace, position, pitch);

public class AddChestSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AddChest, position, pitch);

public class ThrowSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Throw, position, pitch);

public class AttackSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Attack, position, pitch);

public class AttackNoDamageSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AttackNodamage, position, pitch);

public class AttackStrongSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AttackStrong, position, pitch);

public class WarnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Warn, position, pitch);

public class ShearSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Shear, position, pitch);

public class MilkSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Milk, position, pitch);

public class ThunderSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Thunder, position, pitch);

public class ExplodeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Explode, position, pitch);

public class FireSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Fire, position, pitch);

public class IgniteSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Ignite, position, pitch);

public class StareSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Stare, position, pitch);

public class SpawnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Spawn, position, pitch);

public class ShootSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Shoot, position, pitch);

public class BreakBlockSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BreakBlock, position, pitch);

public class BlastSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Blast, position, pitch);

public class LargeBlastSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LargeBlast, position, pitch);

public class TwinkleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Twinkle, position, pitch);

public class RemedySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Remedy, position, pitch);

public class UnfectSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Unfect, position, pitch);

public class LevelUpSound : Sound
{
	public LevelUpSound(Vector3 position, int xpLevel) : base((short) LevelSoundEventType.LevelUp, position, xpLevel, SoundType.LevelSoundEvent)
	{
		ExtraData = unchecked(0x10000000 * (Math.Min(30, xpLevel) / 5));
	}
}

public class BowHitSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BowHit, position, pitch);

public class BulletHitSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BulletHit, position, pitch);

public class ExtinguishFireSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ExtinguishFire, position, pitch);

public class ItemFizzSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemFizz, position, pitch);

public class ChestOpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ChestOpen, position, pitch);

public class ChestClosedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ChestClosed, position, pitch);

public class ShulkerboxOpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ShulkerboxOpen, position, pitch);

public class ShulkerboxClosedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ShulkerboxClosed, position, pitch);

public class EnderchestOpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.EnderchestOpen, position, pitch);

public class EnderchestClosedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.EnderchestClosed, position, pitch);

public class PowerOnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PowerOn, position, pitch);

public class PowerOffSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PowerOff, position, pitch);

public class AttachSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Attach, position, pitch);

public class DetachSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Detach, position, pitch);

public class DenySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Deny, position, pitch);

public class TripodSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Tripod, position, pitch);

public class PopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Pop, position, pitch);

public class DropSlotSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DropSlot, position, pitch);

public class NoteSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Note, position, pitch);

public class ThornsSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Thorns, position, pitch);

public class PistonInSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PistonIn, position, pitch);

public class PistonOutSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PistonOut, position, pitch);

public class PortalSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Portal, position, pitch);

public class PrepareAttackSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PrepareAttack, position, pitch);

public class PrepareSummonSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PrepareSummon, position, pitch);

public class PrepareWololoSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PrepareWololo, position, pitch);

public class FangSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Fang, position, pitch);

public class ChargeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Charge, position, pitch);

public class LeashknotPlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LeashknotPlace, position, pitch);

public class LeashknotBreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LeashknotBreak, position, pitch);

public class GrowlSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Growl, position, pitch);

public class WhineSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Whine, position, pitch);

public class PantSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Pant, position, pitch);

public class PurrSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Purr, position, pitch);

public class PurreowSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Purreow, position, pitch);

public class DeathMinVolumeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DeathMinVolume, position, pitch);

public class DeathMidVolumeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DeathMidVolume, position, pitch);

public class ImitateBlazeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateBlaze, position, pitch);

public class ImitateCaveSpiderSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateCaveSpider, position, pitch);

public class ImitateCreeperSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateCreeper, position, pitch);

public class ImitateElderGuardianSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateElderGuardian, position, pitch);

public class ImitateEnderDragonSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateEnderDragon, position, pitch);

public class ImitateEndermanSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateEnderman, position, pitch);

public class ImitateEndermiteSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateEndermite, position, pitch);

public class ImitateEvocationIllagerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateEvocationIllager, position, pitch);

public class ImitateGhastSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateGhast, position, pitch);

public class ImitateHuskSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateHusk, position, pitch);

public class ImitateIllusionIllagerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateIllusionIllager, position, pitch);

public class ImitateMagmaCubeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateMagmaCube, position, pitch);

public class ImitatePolarBearSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitatePolarBear, position, pitch);

public class ImitateShulkerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateShulker, position, pitch);

public class ImitateSilverfishSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateSilverfish, position, pitch);

public class ImitateSkeletonSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateSkeleton, position, pitch);

public class LtReactionBleachSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionBleach, position, pitch);

public class LtReactionEpasteSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionEpaste, position, pitch);

public class LtReactionEpaste2Sound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionEpaste2, position, pitch);

public class LtReactionGlowStickSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionGlowStick, position, pitch);

public class LtReactionGlowStick2Sound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionGlowStick2, position, pitch);

public class LtReactionLuminolSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionLuminol, position, pitch);

public class LtReactionSaltSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionSalt, position, pitch);

public class LtReactionFertilizerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionFertilizer, position, pitch);

public class LtReactionFireballSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionFireball, position, pitch);

public class LtReactionMgsaltSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionMgsalt, position, pitch);

public class LtReactionMiscfireSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionMiscfire, position, pitch);

public class LtReactionFireSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionFire, position, pitch);

public class LtReactionMiscexplosionSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionMiscexplosion, position, pitch);

public class LtReactionMiscmysticalSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionMiscmystical, position, pitch);

public class LtReactionMiscmystical2Sound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionMiscmystical2, position, pitch);

public class LtReactionProductSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LtReactionProduct, position, pitch);

public class SparklerUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SparklerUse, position, pitch);

public class GlowstickUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.GlowstickUse, position, pitch);

public class SparklerActiveSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SparklerActive, position, pitch);

public class ConvertToDrownedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConvertToDrowned, position, pitch);

public class BucketFillFishSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BucketFillFish, position, pitch);

public class BucketEmptyFishSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BucketEmptyFish, position, pitch);

public class BubbleUpSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BubbleUp, position, pitch);

public class BubbleDownSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BubbleDown, position, pitch);

public class BubblePopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BubblePop, position, pitch);

public class BubbleUpinsideSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BubbleUpinside, position, pitch);

public class BubbleDowninsideSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BubbleDowninside, position, pitch);

public class HurtBabySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HurtBaby, position, pitch);

public class DeathBabySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DeathBaby, position, pitch);

public class StepBabySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.StepBaby, position, pitch);

public class SpawnBabySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SpawnBaby, position, pitch);

public class BornSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Born, position, pitch);

public class BlockTurtleEggBreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockTurtleEggBreak, position, pitch);

public class BlockTurtleEggCrackSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockTurtleEggCrack, position, pitch);

public class BlockTurtleEggHatchSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockTurtleEggHatch, position, pitch);

public class LayEggSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LayEgg, position, pitch);

public class BlockTurtleEggAttackSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockTurtleEggAttack, position, pitch);

public class BeaconActivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BeaconActivate, position, pitch);

public class BeaconAmbientSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BeaconAmbient, position, pitch);

public class BeaconDeactivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BeaconDeactivate, position, pitch);

public class BeaconPowerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BeaconPower, position, pitch);

public class ConduitActivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConduitActivate, position, pitch);

public class ConduitAmbientSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConduitAmbient, position, pitch);

public class ConduitAttackSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConduitAttack, position, pitch);

public class ConduitDeactivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConduitDeactivate, position, pitch);

public class ConduitShortSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConduitShort, position, pitch);

public class SwoopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Swoop, position, pitch);

public class BlockBambooSaplingPlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBambooSaplingPlace, position, pitch);

public class PresneezeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Presneeze, position, pitch);

public class SneezeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Sneeze, position, pitch);

public class AmbientTameSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientTame, position, pitch);

public class ScaredSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Scared, position, pitch);

public class BlockScaffoldingClimbSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockScaffoldingClimb, position, pitch);

public class CrossbowLoadingStartSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrossbowLoadingStart, position, pitch);

public class CrossbowLoadingMiddleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrossbowLoadingMiddle, position, pitch);

public class CrossbowLoadingEndSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrossbowLoadingEnd, position, pitch);

public class CrossbowShootSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrossbowShoot, position, pitch);

public class CrossbowQuickChargeStartSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrossbowQuickChargeStart, position, pitch);

public class CrossbowQuickChargeMiddleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrossbowQuickChargeMiddle, position, pitch);

public class CrossbowQuickChargeEndSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrossbowQuickChargeEnd, position, pitch);

public class AmbientAggressiveSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientAggressive, position, pitch);

public class AmbientWorriedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientWorried, position, pitch);

public class CantBreedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CantBreed, position, pitch);

public class ItemShieldBlockSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemShieldBlock, position, pitch);

public class ItemBookPutSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemBookPut, position, pitch);

public class BlockGrindstoneUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockGrindstoneUse, position, pitch);

public class BlockBellHitSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBellHit, position, pitch);

public class BlockCampfireCrackleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockCampfireCrackle, position, pitch);

public class RoarSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Roar, position, pitch);

public class StunSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Stun, position, pitch);

public class BlockSweetBerryBushHurtSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSweetBerryBushHurt, position, pitch);

public class BlockSweetBerryBushPickSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSweetBerryBushPick, position, pitch);

public class BlockComposterEmptySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockComposterEmpty, position, pitch);

public class BlockComposterFillSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockComposterFill, position, pitch);

public class BlockComposterFillSuccessSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockComposterFillSuccess, position, pitch);

public class BlockComposterReadySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockComposterReady, position, pitch);

public class BlockBarrelOpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBarrelOpen, position, pitch);

public class BlockBarrelCloseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBarrelClose, position, pitch);

public class RaidHornSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RaidHorn, position, pitch);

public class BlockLoomUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockLoomUse, position, pitch);

public class AmbientInRaidSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientInRaid, position, pitch);

public class UiCartographyTableTakeResultSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.UiCartographyTableTakeResult, position, pitch);

public class UiStonecutterTakeResultSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.UiStonecutterTakeResult, position, pitch);

public class UiLoomTakeResultSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.UiLoomTakeResult, position, pitch);

public class BlockSmokerSmokeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSmokerSmoke, position, pitch);

public class BlockBlastFurnaceFireCrackleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBlastFurnaceFireCrackle, position, pitch);

public class BlockSmithingTableUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSmithingTableUse, position, pitch);

public class ScreechSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Screech, position, pitch);

public class SleepSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Sleep, position, pitch);

public class BlockFurnaceLitSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockFurnaceLit, position, pitch);

public class ConvertMooshroomSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConvertMooshroom, position, pitch);

public class MilkSuspiciouslySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MilkSuspiciously, position, pitch);

public class CelebrateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Celebrate, position, pitch);

public class JumpPreventSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.JumpPrevent, position, pitch);

public class AmbientPollinateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientPollinate, position, pitch);

public class BlockBeehiveDripSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBeehiveDrip, position, pitch);

public class BlockBeehiveEnterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBeehiveEnter, position, pitch);

public class BlockBeehiveExitSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBeehiveExit, position, pitch);

public class BlockBeehiveWorkSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBeehiveWork, position, pitch);

public class BlockBeehiveShearSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockBeehiveShear, position, pitch);

public class DrinkHoneySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DrinkHoney, position, pitch);

public class AmbientCaveSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientCave, position, pitch);

public class RetreatSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Retreat, position, pitch);

public class ConvertedToZombifiedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConvertedToZombified, position, pitch);

public class AdmireSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Admire, position, pitch);

public class StepLavaSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.StepLava, position, pitch);

public class TemptSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Tempt, position, pitch);

public class PanicSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Panic, position, pitch);

public class AngrySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Angry, position, pitch);

public class AmbientWarpedForestMoodSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientWarpedForestMood, position, pitch);

public class AmbientSoulSandValleyMoodSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientSoulSandValleyMood, position, pitch);

public class AmbientNetherWastesMoodSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientNetherWastesMood, position, pitch);

public class AmbientBasaltDeltasMoodSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientBasaltDeltasMood, position, pitch);

public class AmbientCrimsonForestMoodSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientCrimsonForestMood, position, pitch);

public class RespawnAnchorChargeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RespawnAnchorCharge, position, pitch);

public class RespawnAnchorDepleteSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RespawnAnchorDeplete, position, pitch);

public class RespawnAnchorSetSpawnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RespawnAnchorSetSpawn, position, pitch);

public class RespawnAnchorAmbientSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RespawnAnchorAmbient, position, pitch);

public class ParticleSoulEscapeQuietSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ParticleSoulEscapeQuiet, position, pitch);

public class ParticleSoulEscapeLoudSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ParticleSoulEscapeLoud, position, pitch);

public class RecordPigstepSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RecordPigstep, position, pitch);

public class LodestoneCompassLinkSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LodestoneCompassLinkCompassToLodestone, position, pitch);

public class SmithingTableUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SmithingTableUse, position, pitch);

public class ArmorEquipNetheriteSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ArmorEquipNetherite, position, pitch);

public class AmbientWarpedForestLoopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientWarpedForestLoop, position, pitch);

public class AmbientSoulSandValleyLoopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientSoulSandValleyLoop, position, pitch);

public class AmbientNetherWastesLoopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientNetherWastesLoop, position, pitch);

public class AmbientBasaltDeltasLoopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientBasaltDeltasLoop, position, pitch);

public class AmbientCrimsonForestLoopSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientCrimsonForestLoop, position, pitch);

public class AmbientWarpedForestAdditionsSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientWarpedForestAdditions, position, pitch);

public class AmbientSoulSandValleyAdditionsSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientSoulSandValleyAdditions, position, pitch);

public class AmbientNetherWastesAdditionsSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientNetherWastesAdditions, position, pitch);

public class AmbientBasaltDeltasAdditionsSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientBasaltDeltasAdditions, position, pitch);

public class AmbientCrimsonForestAdditionsSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientCrimsonForestAdditions, position, pitch);

public class PowerOnSculkSensorSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PowerOnSculkSensor, position, pitch);

public class PowerOffSculkSensorSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PowerOffSculkSensor, position, pitch);

public class BucketFillPowderSnowSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BucketFillPowderSnow, position, pitch);

public class BucketEmptyPowderSnowSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BucketEmptyPowderSnow, position, pitch);

public class CauldronDripWaterPointedDripstoneSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CauldronDripWaterPointedDripstone, position, pitch);

public class CauldronDripLavaPointedDripstoneSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CauldronDripLavaPointedDripstone, position, pitch);

public class DripWaterPointedDripstoneSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DripWaterPointedDripstone, position, pitch);

public class DripLavaPointedDripstoneSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DripLavaPointedDripstone, position, pitch);

public class PickBerriesCaveVinesSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PickBerriesCaveVines, position, pitch);

public class TiltDownBigDripleafSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TiltDownBigDripleaf, position, pitch);

public class TiltUpBigDripleafSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TiltUpBigDripleaf, position, pitch);

public class CopperWaxOnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CopperWaxOn, position, pitch);

public class CopperWaxOffSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CopperWaxOff, position, pitch);

public class ScrapeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Scrape, position, pitch);

public class MobPlayerHurtDrownSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobPlayerHurtDrown, position, pitch);

public class MobPlayerHurtOnFireSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobPlayerHurtOnFire, position, pitch);

public class MobPlayerHurtFreezeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobPlayerHurtFreeze, position, pitch);

public class ItemSpyglassUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemSpyglassUse, position, pitch);

public class ItemSpyglassStopUsingSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemSpyglassStopUsing, position, pitch);

public class ChimeAmethystBlockSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ChimeAmethystBlock, position, pitch);

public class AmbientScreamerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientScreamer, position, pitch);

public class HurtScreamerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HurtScreamer, position, pitch);

public class DeathScreamerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DeathScreamer, position, pitch);

public class MilkScreamerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MilkScreamer, position, pitch);

public class JumpToBlockSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.JumpToBlock, position, pitch);

public class PreRamSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PreRam, position, pitch);

public class PreRamScreamerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PreRamScreamer, position, pitch);

public class RamImpactSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RamImpact, position, pitch);

public class RamImpactScreamerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RamImpactScreamer, position, pitch);

public class SquidInkSquirtSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SquidInkSquirt, position, pitch);

public class GlowSquidInkSquirtSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.GlowSquidInkSquirt, position, pitch);

public class ConvertToStraySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConvertToStray, position, pitch);

public class CakeAddCandleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CakeAddCandle, position, pitch);

public class ExtinguishCandleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ExtinguishCandle, position, pitch);

public class AmbientCandleSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientCandle, position, pitch);

public class BlockClickSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockClick, position, pitch);

public class BlockClickFailSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockClickFail, position, pitch);

public class BlockSculkCatalystBloomSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSculkCatalystBloom, position, pitch);

public class BlockSculkShriekerShriekSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSculkShriekerShriek, position, pitch);

public class NearbyCloseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.NearbyClose, position, pitch);

public class NearbyCloserSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.NearbyCloser, position, pitch);

public class NearbyClosestSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.NearbyClosest, position, pitch);

public class AgitatedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Agitated, position, pitch);

public class RecordOthersideSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RecordOtherside, position, pitch);

public class TongueSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Tongue, position, pitch);

public class IronGolemCrackSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.IronGolemCrack, position, pitch);

public class IronGolemRepairSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.IronGolemRepair, position, pitch);

public class ListeningSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Listening, position, pitch);

public class HeartbeatSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Heartbeat, position, pitch);

public class HornBreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornBreak, position, pitch);

public class BlockSculkSpreadSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSculkSpread, position, pitch);

public class ChargeSculkSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ChargeSculk, position, pitch);

public class BlockSculkSensorPlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSculkSensorPlace, position, pitch);

public class BlockSculkShriekerPlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSculkShriekerPlace, position, pitch);

public class HornCallPonderSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall0, position, pitch);

public class HornCallSingSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall1, position, pitch);

public class HornCallSeekSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall2, position, pitch);

public class HornCallFeelSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall3, position, pitch);

public class HornCallAdmireSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall4, position, pitch);

public class HornCallCallSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall5, position, pitch);

public class HornCallYearnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall6, position, pitch);

public class HornCallDreamSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HornCall7, position, pitch);

public class BlockFrogSpawnBreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockFrogSpawnBreak, position, pitch);

public class SonicBoomSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SonicBoom, position, pitch);

public class SonicChargeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SonicCharge, position, pitch);

public class ItemThrownSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemThrown, position, pitch);

public class Record5Sound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Record5, position, pitch);

public class ConvertToFrogSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ConvertToFrog, position, pitch);

public class BlockEnchantingTableUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockEnchantingTableUse, position, pitch);

public class StepSandSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.StepSand, position, pitch);

public class DashReadySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DashReady, position, pitch);

public class BundleDropContentsSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BundleDropContents, position, pitch);

public class BundleInsertSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BundleInsert, position, pitch);

public class BundleRemoveOneSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BundleRemoveOne, position, pitch);

public class PressurePlateOffSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PressurePlateOff, position, pitch);

public class PressurePlateOnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PressurePlateOn, position, pitch);

public class ButtonOffSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ButtonOff, position, pitch);

public class ButtonOnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ButtonOn, position, pitch);

public class DoorOpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DoorOpen, position, pitch);

public class DoorCloseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DoorClose, position, pitch);

public class TrapdoorOpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrapdoorOpen, position, pitch);

public class TrapdoorCloseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrapdoorClose, position, pitch);

public class FenceGateOpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.FenceGateOpen, position, pitch);

public class FenceGateCloseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.FenceGateClose, position, pitch);

public class InsertSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Insert, position, pitch);

public class PickupSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Pickup, position, pitch);

public class InsertEnchantedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.InsertEnchanted, position, pitch);

public class PickupEnchantedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PickupEnchanted, position, pitch);

public class BrushSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Brush, position, pitch);

public class BrushCompletedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BrushCompleted, position, pitch);

public class ShatterPotSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ShatterPot, position, pitch);

public class BreakPotSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BreakPot, position, pitch);

public class BlockSnifferEggCrackSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSnifferEggCrack, position, pitch);

public class BlockSnifferEggHatchSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSnifferEggHatch, position, pitch);

public class BlockSignWaxedInteractFailSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockSignWaxedInteractFail, position, pitch);

public class RecordRelicSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RecordRelic, position, pitch);

public class NoteBassSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.NoteBass, position, pitch);

public class PumpkinCarveSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.PumpkinCarve, position, pitch);

public class MobHuskConvertToZombieSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobHuskConvertToZombie, position, pitch);

public class MobPigDeathSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobPigDeath, position, pitch);

public class MobHoglinConvertedToZombifiedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobHoglinConvertedToZombified, position, pitch);

public class AmbientUnderwaterEnterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientUnderwaterEnter, position, pitch);

public class AmbientUnderwaterExitSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientUnderwaterExit, position, pitch);

public class BottleFillSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BottleFill, position, pitch);

public class BottleEmptySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BottleEmpty, position, pitch);

public class CrafterCraftSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrafterCraft, position, pitch);

public class CrafterFailSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrafterFail, position, pitch);

public class BlockDecoratedPotInsertSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockDecoratedPotInsert, position, pitch);

public class BlockDecoratedPotInsertFailSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockDecoratedPotInsertFail, position, pitch);

public class CrafterDisableSlotSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CrafterDisableSlot, position, pitch);

public class TrialSpawnerOpenShutterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerOpenShutter, position, pitch);

public class TrialSpawnerEjectItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerEjectItem, position, pitch);

public class TrialSpawnerDetectPlayerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerDetectPlayer, position, pitch);

public class TrialSpawnerSpawnMobSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerSpawnMob, position, pitch);

public class TrialSpawnerCloseShutterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerCloseShutter, position, pitch);

public class TrialSpawnerAmbientSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerAmbient, position, pitch);

public class BlockCopperBulbTurnOnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockCopperBulbTurnOn, position, pitch);

public class BlockCopperBulbTurnOffSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockCopperBulbTurnOff, position, pitch);

public class AmbientInAirSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.AmbientInAir, position, pitch);

public class BreezeWindChargeBurstSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BreezeWindChargeBurst, position, pitch);

public class ImitateBreezeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateBreeze, position, pitch);

public class MobArmadilloBrushSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobArmadilloBrush, position, pitch);

public class MobArmadilloScuteDropSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MobArmadilloScuteDrop, position, pitch);

public class ArmorEquipWolfSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ArmorEquipWolf, position, pitch);

public class ArmorUnequipWolfSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ArmorUnequipWolf, position, pitch);

public class ReflectSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Reflect, position, pitch);

public class VaultOpenShutterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultOpenShutter, position, pitch);

public class VaultCloseShutterSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultCloseShutter, position, pitch);

public class VaultEjectItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultEjectItem, position, pitch);

public class VaultInsertItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultInsertItem, position, pitch);

public class VaultInsertItemFailSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultInsertItemFail, position, pitch);

public class VaultAmbientSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultAmbient, position, pitch);

public class VaultActivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultActivate, position, pitch);

public class VaultDeactivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultDeactivate, position, pitch);

public class HurtReducedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.HurtReduced, position, pitch);

public class WindChargeBurstSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.WindChargeBurst, position, pitch);

public class ImitateBoggedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateBogged, position, pitch);

public class ArmorCrackWolfSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ArmorCrackWolf, position, pitch);

public class ArmorBreakWolfSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ArmorBreakWolf, position, pitch);

public class ArmorRepairWolfSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ArmorRepairWolf, position, pitch);

public class MaceSmashAirSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MaceSmashAir, position, pitch);

public class MaceSmashGroundSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MaceSmashGround, position, pitch);

public class TrialSpawnerChargeActivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerChargeActivate, position, pitch);

public class TrialSpawnerAmbientOminousSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.TrialSpawnerAmbientOminous, position, pitch);

public class OminousItemSpawnerSpawnItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.OminousItemSpawnerSpawnItem, position, pitch);

public class OminousBottleEndUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.OminousBottleEndUse, position, pitch);

public class MaceHeavySmashGroundSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.MaceHeavySmashGround, position, pitch);

public class OminousItemSpawnerSpawnItemBeginSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.OminousItemSpawnerSpawnItemBegin, position, pitch);

public class ApplyEffectBadOmenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ApplyEffectBadOmen, position, pitch);

public class ApplyEffectRaidOmenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ApplyEffectRaidOmen, position, pitch);

public class ApplyEffectTrialOmenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ApplyEffectTrialOmen, position, pitch);

public class OminousItemSpawnerAboutToSpawnItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.OminousItemSpawnerAboutToSpawnItem, position, pitch);

public class RecordCreatorSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RecordCreator, position, pitch);

public class RecordCreatorMusicBoxSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RecordCreatorMusicBox, position, pitch);

public class RecordPrecipiceSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.RecordPrecipice, position, pitch);

public class VaultRejectRewardedPlayerSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.VaultRejectRewardedPlayer, position, pitch);

public class ImitatedDrownedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateDrowned, position, pitch);

public class ImitateWardenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateWarden, position, pitch);

public class ListeningAngrySound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ListeningAngry, position, pitch);

public class ItemGivenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemGiven, position, pitch);

public class ItemTakenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ItemTaken, position, pitch);

public class DisappearedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Disappeared, position, pitch);

public class ReappearedSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Reappeared, position, pitch);

public class DrinkMilkSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.DrinkMilk, position, pitch);

public class BlockFrogSpawnHatchSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockFrogSpawnHatch, position, pitch);

public class LaySpawnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.LaySpawn, position, pitch);

public class ImitateCreakingSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.ImitateCreaking, position, pitch);

public class BundleInsertFailSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BundleInsertFail, position, pitch);

public class SpongeAbsorbSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.SpongeAbsorb, position, pitch);

public class BlockCreakingHeartTrailSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.BlockCreakingHeartTrail, position, pitch);

public class CreakingHeartSpawnSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CreakingHeartSpawn, position, pitch);

public class ActivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Activate, position, pitch);

public class DeactivateSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Deactivate, position, pitch);

public class FreezeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Freeze, position, pitch);

public class UnfreezeSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Unfreeze, position, pitch);

public class OpenSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Open, position, pitch);

public class OpenLongSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.OpenLong, position, pitch);

public class CloseSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.Close, position, pitch);

public class CloseLongSound(Vector3 position, int pitch = 0) : Sound((short) LevelSoundEventType.CloseLong, position, pitch);