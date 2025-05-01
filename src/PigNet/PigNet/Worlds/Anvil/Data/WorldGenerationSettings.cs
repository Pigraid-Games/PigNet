using System;
using fNbt;
using fNbt.Serialization;

namespace PigNet.Worlds.Anvil.Data;

[NbtObject]
public class WorldGenerationSettings : ICloneable
{
	[NbtProperty("bonus_chest")]
	public bool BonusChest { get; set; }

	[NbtProperty("seed")]
	public long Seed { get; set; }

	[NbtProperty("generate_features")]
	public bool GenerateFeatures { get; set; }

	[NbtProperty("dimensions")]
	public NbtCompound Dimensions { get; set; }

	public object Clone()
	{
		var clone = (WorldGenerationSettings) MemberwiseClone();
		clone.Dimensions = (NbtCompound) Dimensions.Clone();

		return clone;
	}
}