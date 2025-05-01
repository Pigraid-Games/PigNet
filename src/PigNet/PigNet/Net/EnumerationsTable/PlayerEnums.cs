
namespace PigNet.Net.EnumerationsTable;

public enum PlayerListPacketType
{
	Add = 0,
	Remove = 1
}

public enum PlayerPermissionLevel  
{
	Visitor = 0,
	Member = 1,
	Operator = 2,
	Custom = 3
}

public enum PositionMode : byte
{
	Normal = 0,
	Respawn = 1, 
	Teleport = 2,
	OnlyHeadRot = 3
}

public enum PlayerRespawnState : byte
{
	SearchingForSpawn = 0,
	ReadyToSpawn = 1,
	ClientReadyToSpawn = 2
}