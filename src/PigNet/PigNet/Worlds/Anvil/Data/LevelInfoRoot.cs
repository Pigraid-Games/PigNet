using fNbt.Serialization;

namespace PigNet.Worlds.Anvil.Data;

[NbtObject]
public class LevelInfoRoot
{
	public LevelInfo Data { get; set; }
}