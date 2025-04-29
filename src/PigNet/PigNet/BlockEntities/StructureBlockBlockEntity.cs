using fNbt.Serialization;
using fNbt.Serialization.NamingStrategy;
using PigNet.Utils.Vectors;

namespace PigNet.BlockEntities;

public class StructureBlockBlockEntity() : BlockEntity(BlockEntityIds.StructureBlock)
{
	[NbtFlatProperty(typeof(StructureOffsetNamingStrategy))]
	public BlockCoordinates Offset { get; set; } = new(0, -1, 0);

	[NbtFlatProperty(typeof(StructureSizeNamingStrategy))]
	public BlockCoordinates Size { get; set; } = new(5, 5, 5);

	[NbtProperty("showBoundingBox")]
	public bool ShowBoundingBox { get; set; } = true;

	private class StructureSizeNamingStrategy : NbtNamingStrategy
	{
		public override string ResolveMemberName(string name)
		{
			return $"{name.ToLower()}StructureSize";
		}
	}

	private class StructureOffsetNamingStrategy : NbtNamingStrategy
	{
		public override string ResolveMemberName(string name)
		{
			return $"{name.ToLower()}StructureOffset";
		}
	}
}