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

public enum PlayerAuthInputPacketInputData
{
	Ascend = 0,
	Descend = 1,
	DeprecatedNorthJump = 2,
	JumpDown = 3,
	SprintDown = 4,
	ChangeHeight = 5,
	Jumping = 6,
	AutoJumpingInWater = 7,
	Sneaking = 8,
	SneakDown = 9,
	Up = 10,
	Down = 11,
	Left = 12,
	Right = 13,
	UpLeft = 14,
	UpRight = 15,
	WantUp = 16,
	WantDown = 17,
	WantDownSlow = 18,
	WantUpSlow = 19,
	Sprinting = 20,
	AscendBlock = 21,
	DescendBlock = 22,
	SneakToggleDown = 23,
	PersistSneak = 24,
	StartSprinting = 25,
	StopSprinting = 26,
	StartSneaking = 27,
	StopSneaking = 28,
	StartSwimming = 29,
	StopSwimming = 30,
	StartJumping = 31,
	StartGliding = 32,
	StopGliding = 33,
	PerformItemInteraction = 34,
	PerformBlockActions = 35,
	PerformItemStackRequest = 36,
	HandledTeleport = 37,
	Emoting = 38,
	MissedSwing = 39,
	StartCrawling = 40,
	StopCrawling = 41,
	StartFlying = 42,
	StopFlying = 43,
	ClientAckServerData = 44,
	IsInClientPredictedVehicle = 45,
	PaddingLeft = 46,
	PaddingRight = 47,
	BlockBreakingDelayEnabled = 48,
	HorizontalCollision = 49,
	VerticalCollision = 50,
	DownLeft = 51,
	DownRight = 52,
	StartUsingItem = 53,
	IsCameraRelativeMovementEnabled = 54,
	IsRotControlledByMoveDirection = 55,
	StartSpinAttack = 56,
	StopSpingAttack = 57,
	IsHotbarOnlyTouch = 58,
	JumpReleasedRaw = 59,
	JumpPressedRaw = 60,
	JumpCurrentRaw = 61,
	SneakReleasedRaw = 62,
	SneakPressedRaw = 63,
	SneakCurrentRaw = 64,
	InputNum = 65
}