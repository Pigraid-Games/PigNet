using System.Collections.Generic;
using PigNet.Utils;

namespace PigNet.Net;

public class EmoteIds : IPacketDataObject
{
	public List<UUID> emoteId = [];
	
	public void Write(Packet packet)
	{
		foreach(UUID id in emoteId) packet.Write(id);
	}
}