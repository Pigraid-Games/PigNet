
namespace PigNet.Net.EnumerationsTable;

public enum GameRuleType
{
	Invalid = 0,
	Bool = 1,
	Int = 2,
	Float = 3
}

public enum GameType
{
	Undefined = -1,
	Survival = 0,
	Creative = 1,
	Adventure = 2,
	Default = 5,
	Spectator = 6,
	WorldDefault = Survival
}