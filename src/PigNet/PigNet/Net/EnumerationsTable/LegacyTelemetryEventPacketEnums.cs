
namespace PigNet.Net.EnumerationsTable;

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