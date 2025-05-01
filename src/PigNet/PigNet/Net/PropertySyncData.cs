using System.Collections.Generic;

namespace PigNet.Net;

public class PropertySyncData : IPacketDataObject
{
	public Dictionary<uint, int> IntProperties = new();
	public Dictionary<uint, float> FloatProperties = new();

	public void Write(Packet packet)
	{
		packet.WriteLength(IntProperties.Count);

		foreach (KeyValuePair<uint, int> intP in IntProperties)
		{
			packet.WriteUnsignedVarInt(intP.Key);
			packet.WriteSignedVarInt(intP.Value);
		}

		packet.WriteLength(FloatProperties.Count);

		foreach (KeyValuePair<uint, float> intF in FloatProperties)
		{
			packet.WriteUnsignedVarInt(intF.Key);
			packet.Write(intF.Value);
		}
	}

	public static PropertySyncData Read(Packet packet)
	{
		var syncData = new PropertySyncData();
		int countInt = packet.ReadLength();
		for (int i = 0; i < countInt; i++) syncData.IntProperties.Add(packet.ReadUnsignedVarInt(), packet.ReadVarInt());

		int countFloat = packet.ReadLength();
		for (int i = 0; i < countFloat; i++) syncData.FloatProperties.Add(packet.ReadUnsignedVarInt(), packet.ReadFloat());

		return syncData;
	}
}