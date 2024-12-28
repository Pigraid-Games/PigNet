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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2024 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System.Numerics;

namespace MiNET.Sounds;

#region Anvil Sounds
public class AnvilBreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundAnvilBroken, position, pitch);
public class AnvilFallSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundAnvilLand, position, pitch);
public class AnvilUseSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundAnvilUsed, position, pitch);
#endregion

#region Entity Sounds
public class BlazeFireballSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundBlazeFireball, position, pitch);
public class GhastSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundGhastFireball, position, pitch);
public class GhastWarningSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundGhastWarning, position, pitch);
public class ZombieDoorCrashSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundZombieDoorCrash, position, pitch);
public class ZombieInfectedSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundZombieInfected, position, pitch);
public class ZombieWoodenDoorSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundZombieWoodenDoor, position, pitch);
public class ZombieConvertedSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundZombieConverted, position, pitch);
public class EndermanTeleportSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundEndermanTeleport, position, pitch);
public class InfinityArrowPickupSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundInfinityArrowPickup, position, pitch);
public class TeleportEnderPearlSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundTeleportEnderPearl, position, pitch);
#endregion

#region Interaction Sounds
public class ClickSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundClick, position, pitch);
public class ClickFailSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundClickFail, position, pitch);
public class LaunchSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundLaunch, position, pitch);
public class OpenDoorSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundOpenDoor, position, pitch);
public class AddItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundAddItem, position, pitch);
public class ItemFrameBreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundItemFrameBreak, position, pitch);
public class ItemFramePlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundItemFramePlace, position, pitch);
public class ItemFrameRemoveItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundItemFrameRemoveItem, position, pitch);
public class ItemFrameRotateItemSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundItemFrameRotateItem, position, pitch);
#endregion

#region Environment Sounds
public class FizzSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundFizz, position, pitch);
public class FuseSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundFuse, position, pitch);
public class PlayRecordingSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundPlayRecording, position, pitch);
public class ExperienceOrbSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundExperienceOrbPickup, position, pitch);
public class TotemUsedSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundTotemUsed, position, pitch);
public class ArmorStandBreakSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundArmorStandBreak, position, pitch);
public class ArmorStandLandSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundArmorStandLand, position, pitch);
public class ArmorStandHitSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundArmorStandHit, position, pitch);
public class ArmorStandPlaceSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundArmorStandPlace, position, pitch);
public class PointedDripstoneLandSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundPointedDripstoneLand, position, pitch);
public class DyeUsedSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundDyeUsed, position, pitch);
public class InkSacUsedSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundInkSacUsed, position, pitch);
#endregion

#region Camera Sounds
public class CameraTakePictureSound(Vector3 position, int pitch = 0) : Sound((short) LevelEventType.SoundCameraTakePicture, position, pitch);
#endregion