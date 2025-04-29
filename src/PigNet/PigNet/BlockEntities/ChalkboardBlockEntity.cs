using fNbt.Serialization;
using fNbt.Serialization.NamingStrategy;
using PigNet.Utils.Vectors;

namespace PigNet.BlockEntities;

public class ChalkboardBlockEntity() : BlockEntity(BlockEntityIds.ChalkboardBlock)
{
	public string Text { get; set; } = string.Empty;
	public bool? Locked { get; set; }
	public bool? OnGround { get; set; }
	public long? Owner { get; set; }
	public int? Size { get; set; }

	[NbtFlatProperty(typeof(BaseNamingStrategy))]
	public BlockCoordinates BaseCoordinates { get; set; }

	private class BaseNamingStrategy : NbtNamingStrategy
	{
		public override string ResolveMemberName(string name)
		{
			return $"Base{name}";
		}
	}
}