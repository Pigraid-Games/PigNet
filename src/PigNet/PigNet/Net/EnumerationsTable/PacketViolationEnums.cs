
namespace PigNet.Net.EnumerationsTable;

public enum PacketViolationSeverity
{
	Unknown = -1,
	Warning = 0,
	FinalWarning = 1,
	TerminatingConnection = 2
}

public enum PacketViolationType
{
	Unknown = -1,
	PacketMalformed = 0
}