
namespace PigNet.Net.EnumerationsTable;

public enum HeightMapDataType
{
	NoData = 0,
	HasData = 1,
	AllToolHigh = 2,
	AllTooLow = 3
}

public enum SubChunkRequestResult
{
	Undefined = 0,
	Success = 1,
	LevelChunkDoesntExist = 2,
	WrongDimension = 3,
	PlayerDoesntExist = 4,
	IndexOutOfBounds = 5,
	SuccessAllAir = 6
}