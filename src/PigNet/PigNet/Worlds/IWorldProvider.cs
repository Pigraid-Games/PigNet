using System.Numerics;
using PigNet.Utils.Vectors;

namespace PigNet.Worlds;

public interface IWorldProvider
{
	bool IsCaching { get; }

	void Initialize();

	ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates, bool cacheOnly = false);

	Vector3 GetSpawnPoint();
	string GetName();

	long GetTime();
	long GetDayTime();

	int SaveChunks();
	bool HaveNether();
	bool HaveTheEnd();
}

public interface IWorldGenerator
{
	void Initialize(IWorldProvider worldProvider);

	ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates);
}