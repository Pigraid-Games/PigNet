using System;
using fNbt.Serialization;

namespace PigNet.Worlds.Anvil.Data;

[NbtObject]
public class BorderInfo : ICloneable
{
	[NbtFlatProperty]
	public BorderCoordinates Center { get; set; }

	[NbtProperty("BorderDamagePerBlock")]
	public double DamagePerBlock { get; set; }

	[NbtProperty("BorderSize")]
	public double Size { get; set; }

	[NbtProperty("BorderSafeZone")]
	public double SafeZone { get; set; }

	[NbtProperty("BorderSizeLerpTarget")]
	public double SizeLerpTarget { get; set; }

	[NbtProperty("BorderSizeLerpTime")]
	public long SizeLerpTime { get; set; }

	[NbtProperty("BorderWarningBlocks")]
	public double WarningBlocks { get; set; }

	[NbtProperty("BorderWarningTime")]
	public double WarningTime { get; set; }

	public object Clone()
	{
		var clone = (BorderInfo) MemberwiseClone();
		clone.Center = (BorderCoordinates) Center.Clone();

		return clone;
	}
}