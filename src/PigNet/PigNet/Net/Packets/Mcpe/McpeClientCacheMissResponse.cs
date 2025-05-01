
using System.Collections.Generic;
using log4net;

namespace PigNet.Net.Packets.Mcpe;

public class McpeClientCacheMissResponse : Packet<McpeClientCacheMissResponse>
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(McpeClientCacheMissResponse));
	public Dictionary<ulong, byte[]> blobs;
	
	public McpeClientCacheMissResponse()
	{
		Id = 0x88;
		IsMcpe = true;
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		blobs = new Dictionary<ulong, byte[]>();
		uint count = ReadUnsignedVarInt();
		for (int i = 0; i < count; i++)
		{
			ulong hash = ReadUlong();
			byte[] blob = ReadByteArray();
			if (!blobs.TryAdd(hash, blob)) Log.Warn($"Already had hash:{hash}. This is most likely air or water");
		}
	}
}