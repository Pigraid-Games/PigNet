using fNbt.Serialization;

namespace PigNet.BlockEntities;

public class MobSpawnerBlockEntity() : BlockEntity(BlockEntityIds.MobSpawner)
{
	public short Delay { get; set; } = 20;
	public float DisplayEntityHeight { get; set; } = 1.8f;
	public float DisplayEntityScale { get; set; } = 1.0f;
	public float DisplayEntityWidth { get; set; } = 0.8f;

	[NbtProperty("EntityId")]
	public int EntityTypeId { get; set; } = 1;
	public short MaxNearbyEntities { get; set; } = 4;
	public short MinSpawnDelay { get; set; } = 200;
	public short MaxSpawnDelay { get; set; } = 800;
	public short RequiredPlayerRange { get; set; } = 16;
	public short SpawnCount { get; set; } = 4;
	public short SpawnRange { get; set; } = 4;
}