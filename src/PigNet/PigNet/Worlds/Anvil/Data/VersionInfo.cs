using System;
using fNbt.Serialization;

namespace PigNet.Worlds.Anvil.Data;

[NbtObject]
public class VersionInfo : ICloneable
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Series { get; set; }

	public bool Snapshot { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}
}