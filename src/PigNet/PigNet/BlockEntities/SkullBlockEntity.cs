using fNbt.Serialization;

namespace PigNet.BlockEntities;

public class SkullBlockEntity() : BlockEntity(BlockEntityIds.Skull)
{
	[NbtProperty("Rot")]
	public byte Rotation { get; set; }

	public bool MouthMoving { get; set; }

	public int MouthTickCount { get; set; }
}