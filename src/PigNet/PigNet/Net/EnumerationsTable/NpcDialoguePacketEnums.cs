
namespace PigNet.Net.EnumerationsTable;

public enum NpcDialogueActionType
{
	Open = 0,
	Close = 1
}

public enum NpcRequestType 
{
	SetActions = 0,
	ExecuteAction = 1,
	ExecuteClosingCommands = 2,
	SetName = 3,
	SetSkin = 4,
	SetInteractText = 5,
	ExecuteOpeningCommands = 6
}