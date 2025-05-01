using System;
using fNbt.Serialization;

namespace PigNet.Worlds.Anvil.Data;

[NbtObject]
public class BorderCoordinates : ICloneable
{
	[NbtProperty("BorderCenterX")]
	public double X { get; set; }

	[NbtProperty("BorderCenterZ")]
	public double Z { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}
}