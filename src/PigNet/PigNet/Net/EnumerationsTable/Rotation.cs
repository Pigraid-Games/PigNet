
namespace PigNet.Net.EnumerationsTable;

public enum Rotation
{
	None = 0,
	Rotate90 = 1,
	Rotate180 = 2,
	Rotate270 = 3,
	Clockwise90 = Rotate90,
	Clockwise180 = Rotate180, 
	CounterClockwise90 = Rotate270,
	Total = 4
}