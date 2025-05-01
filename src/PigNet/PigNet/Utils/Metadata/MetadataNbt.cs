using System.IO;
using fNbt;
using PigNet.Net;
using PigNet.Utils.Nbt;

namespace PigNet.Utils.Metadata;

public class MetadataNbt : MetadataEntry
{
	public override byte Identifier
	{
		get { return 5; }
	}

	public override string FriendlyName
	{
		get { return "nbt"; }
	}

	public NbtCompound Value { get; set; }

	public MetadataNbt()
	{
	}

	public MetadataNbt(NbtCompound value)
	{
		Value = value;
	}

	public override void FromStream(BinaryReader reader)
	{
		Value = NbtExtensions.ReadNbtCompound(reader.BaseStream);
	}

	public override void WriteTo(BinaryWriter stream)
	{
		NbtCompound nbt = Value;

		stream.Write((ushort) 0xffff);
		stream.Write((byte) 0x01);
		NbtExtensions.Write(stream.BaseStream, nbt);
	}
}