using System.Collections.Generic;
using PigNet.Net;

namespace PigNet.Utils;

public class FogStack : IPacketDataObject
{
	public List<string> fogList = [];

	public FogStack(params string[] effects)
	{
		fogList.AddRange(effects);
	}

	public void Write(Packet packet)
	{
		packet.WriteUnsignedVarInt((uint) fogList.Count);
		foreach (string effect in fogList) packet.Write(effect);
	}
}