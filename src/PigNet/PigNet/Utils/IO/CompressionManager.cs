#region LICENSE
// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using PigNet.Net;

namespace PigNet.Utils.IO;

	public class CompressionManager
	{
		public static CompressionManager NoneCompressionManager { get; } = new CompressionManager();
		public static CompressionManager ZLibCompressionManager { get; } = new CompressionManager() { CompressionAlgorithm = CompressionAlgorithm.ZLib };

		public CompressionAlgorithm CompressionAlgorithm { get; set; } = CompressionAlgorithm.None;

		public short CompressionThreshold { get; set; } = 1000;

		public byte[] Compress(Memory<byte> input, bool writeLen = false, CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				var compressor = GetCompressor(input.Length);

				if (CompressionAlgorithm != CompressionAlgorithm.None)
				{
					WriteCompressorAlgorithm(stream, compressor.CompressionAlgorithm);
				}

				compressor.Write(stream, input, writeLen, compressionLevel);

				return stream.ToArray();
			}
		}

		public byte[] CompressPacketsForWrapper(List<Packet> packets, CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			long length = 0;
			foreach (Packet packet in packets)
			{
				length += packet.Encode().Length;
			}

			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				var compressor = GetCompressor(length);

				if (CompressionAlgorithm != CompressionAlgorithm.None)
				{
					WriteCompressorAlgorithm(stream, compressor.CompressionAlgorithm);
				}

				compressor.Write(stream, packets, compressionLevel);

				return stream.ToArray();
			}
		}

		private ICompressor GetCompressor(long length)
		{
			return length > CompressionThreshold 
				? GetCompressor(CompressionAlgorithm) 
				: GetCompressor(CompressionAlgorithm.None);
		}

		public IEnumerable<Packet> Decompress(ReadOnlyMemory<byte> payload)
		{
			if (CompressionAlgorithm == CompressionAlgorithm.None)
			{
				return GetCompressor(CompressionAlgorithm.None).ReadPackets(payload);
			}
			else
			{
				return GetCompressor((CompressionAlgorithm) payload.Span[0]).ReadPackets(payload.Slice(1));
			}
		}

		public static ICompressor GetCompressor(CompressionAlgorithm compressionAlgorithm)
		{
			return compressionAlgorithm switch
			{
				CompressionAlgorithm.ZLib => ZLibCompressor.Instance,
				CompressionAlgorithm.Snappy => throw new NotImplementedException(),

				_ => NoneCompressor.Instance
			};
		}

		private static void WriteCompressorAlgorithm(MemoryStream stream, CompressionAlgorithm compressionAlgorithm)
		{
			stream.WriteByte((byte) compressionAlgorithm);
		}
	}