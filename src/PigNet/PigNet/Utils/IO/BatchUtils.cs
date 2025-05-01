using System;
using System.IO;
using System.IO.Compression;
using PigNet.Net;
using PigNet.Net.Packets.Mcpe;
using PigNet.Net.RakNet;

namespace PigNet.Utils.IO;

public class BatchUtils
{
	public static McpeWrapper CreateBatchPacket(CompressionLevel compressionLevel, params Packet[] packets)
	{
		using (var stream = new MemoryStream())
		{
			foreach (Packet packet in packets)
			{
				byte[] bytes = packet.Encode();
				WriteLength(stream, bytes.Length);
				stream.Write(bytes, 0, bytes.Length);
				packet.PutPool();
			}

			var buffer = new Memory<byte>(stream.GetBuffer(), 0, (int) stream.Length);
			return CreateBatchPacket(buffer, compressionLevel, false);
		}
	}

	public static McpeWrapper CreateBatchPacket(Memory<byte> input, CompressionLevel compressionLevel, bool writeLen)
	{
		var batch = McpeWrapper.CreateObject();
		batch.ReliabilityHeader.Reliability = Reliability.ReliableOrdered;
		batch.payload = CompressionManager.ZLibCompressionManager.Compress(input, writeLen, compressionLevel);
		batch.Encode(); // prepare
		return batch;
	}

	public static void WriteLength(Stream stream, int length)
	{
		VarInt.WriteUInt32(stream, (uint) length);
	}
}