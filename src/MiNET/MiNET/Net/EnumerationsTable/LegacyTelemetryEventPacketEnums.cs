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

public enum LegacyTelemetryEventPacketAgentResult
{
	ActionFail = 0,
	ActionSuccess = 1,
	QueryResultFalse = 2,
	QueryResultTrue = 3
}

public enum LegacyTelemetryEventPacketType
{
	Achievement = 0,
	Interaction = 1,
	PortalCreated = 2,
	PortalUsed = 3,
	MobKilled = 4,
	CauldronUsed = 5,
	PlayerDied = 6,
	BossKilled = 7,
	AgentCommand_OBSOLETE = 8,
	AgentCreated = 9,
	PatternRemoved_OBSOLETE = 10,
	SlashCommand = 11,
	Deprecated_FishBucketed = 12,
	MobBorn = 13,
	PetDied_OBSOLETE = 14,
	POICauldronUsed = 15,
	ComposterUsed = 16,
	BellUsed = 17,
	ActorDefinition = 18,
	RaidUpdate = 19,
	PlayerMovementAnomaly_OBSOLETE = 20,
	PlayerMovementCorrected_OBSOLETE = 21,
	HoneyHarvested = 22,
	TargetBlockHit = 23,
	PiglinBarter = 24,
	PlayerWaxedOrUnwaxedCopper = 25,
	CodeBuilderRuntimeAction = 26,
	CodeBuilderScoreboard = 27,
	StriderRiddenInLavaInOverworld = 28,
	SneakCloseToSculkSensor = 29,
	CarefulRestoration = 30,
	ItemUsedEvent = 31
}