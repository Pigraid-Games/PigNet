
namespace PigNet.Net.EnumerationsTable;

public enum StructureBlockType
{
	Data = 0,
	Save = 1,
	Load = 2,
	Corner = 3,
	Invalid = 4,
	Export = 5,
	_count = 6
}

public enum StructureRedstoneSaveMode 
{
	SavesToMemory = 0,
	SavesToDisk = 1
}

public enum StructureTemplateRequestOperation
{
	None = 0,
	ExportFromSaveMode = 1,
	ExportFromLoadMode = 2,
	QuerySavedStructure = 3
}

public enum StructureTemplateResponseType
{
	None = 0,
	Export = 1,
	Query = 2
}