using System.IO;
using fNbt;
using PigNet.Utils.Cryptography;

namespace PigNet.Utils.Nbt;

public static class NbtExtensions
{
	public static uint GetFnvHash(this NbtCompound compound, NbtFlavor flavor, NbtCompression compression = NbtCompression.None)
	{
		compound.Name ??= string.Empty;
		return GetFnvHash(new NbtFile() { RootTag = compound, Flavor = flavor }, compression);
	}

	public static uint GetFnvHash(this NbtFile file, NbtCompression compression = NbtCompression.None)
	{
		var buffer = file.SaveToBuffer(compression);

		return Fnv.ComputeHash(buffer);
	}

	public static byte[] ToBytes(this NbtCompound compound, NbtFlavor flavor = null, NbtCompression compression = NbtCompression.None)
	{
		compound.Name ??= string.Empty;
		return new NbtFile() { RootTag = compound, Flavor = flavor ?? NbtFlavor.Bedrock }.SaveToBuffer(compression);
	}

	public static void Write(Stream stream, NbtCompound compound, NbtFlavor flavor = null)
	{
		compound.Name ??= string.Empty;
		Write(stream, new NbtFile() { RootTag = compound, Flavor = flavor ?? NbtFlavor.Bedrock });
	}

	public static void Write(Stream stream, NbtFile file)
	{
		file.SaveToStream(stream, NbtCompression.None);
	}

	public static NbtCompound ReadNbtCompound(Stream stream, NbtFlavor flavor = null)
	{
		return ReadNbt(stream, flavor).RootTag;
	}

	public static NbtCompound ReadNbtCompound(byte[] buffer, NbtFlavor flavor = null)
	{
		return ReadNbt(buffer, flavor).RootTag;
	}

	public static NbtFile ReadNbt(byte[] buffer, NbtFlavor flavor = null)
	{
		var file = new NbtFile() { Flavor = flavor ?? NbtFlavor.Bedrock };

		file.LoadFromBuffer(buffer, 0, buffer.Length, NbtCompression.None);

		return file;
	}

	public static NbtFile ReadNbt(Stream stream, NbtFlavor flavor = null)
	{
		var file = new NbtFile() { Flavor = flavor ?? NbtFlavor.Bedrock };

		file.LoadFromStream(stream, NbtCompression.None);

		return file;
	}
}