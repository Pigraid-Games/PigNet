using fNbt.Serialization.NamingStrategy;

namespace PigNet.Utils.Nbt;

public class NbtLowerCaseNamingStrategy : NbtNamingStrategy
{
	public override string ResolveMemberName(string name)
	{
		return name.ToLower();
	}
}