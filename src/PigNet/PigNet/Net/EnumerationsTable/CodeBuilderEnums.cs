
namespace PigNet.Net.EnumerationsTable;

public enum CodeStatus
{
	None = 0,
	NotStarted = 1,
	InProgress = 2,
	Paused = 3,
	Error = 4,
	Succeeded = 5
}

public enum StorageQueryCategory
{
	None = 0,
	CodeStatus = 1,
	Instantiation = 2
}

public enum StorageQueryOperation
{
	None = 0,
	Get = 1,
	Set = 2,
	Reset = 3
}