using System.Collections.Generic;

namespace PigNet.Net;

public class DimensionData : IPacketDataObject
{
	public int MaxHeight { get; set; }
	public int MinHeight { get; set; }
	public int Generator { get; set; }

	public void Write(Packet packet)
	{
		packet.WriteVarInt(MaxHeight);
		packet.WriteVarInt(MinHeight);
		packet.WriteVarInt(Generator);
	}

	public static DimensionData Read(Packet packet)
	{
		return new DimensionData
		{
			MaxHeight = packet.ReadVarInt(),
			MinHeight = packet.ReadVarInt(),
			Generator = packet.ReadVarInt()
		};
	}
}

public class DimensionDefinitions : Dictionary<string, DimensionData>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.WriteLength(Count);

		foreach (KeyValuePair<string, DimensionData> definition in this)
		{
			packet.Write(definition.Key);
			packet.Write(definition.Value);
		}
	}

	public static DimensionDefinitions Read(Packet packet)
	{
		var definitions = new DimensionDefinitions();
		int count = packet.ReadLength();
		for (int i = 0; i < count; i++) definitions.Add(packet.ReadString(), DimensionData.Read(packet));
		return definitions;
	}
}