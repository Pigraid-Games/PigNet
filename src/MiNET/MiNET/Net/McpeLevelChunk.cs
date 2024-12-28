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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using log4net;

namespace MiNET.Net;

public enum SubChunkRequestMode
{
	SubChunkRequestModeLegacy,
	SubChunkRequestModeLimitless,
	SubChunkRequestModeLimited
}
public partial class McpeLevelChunk
{
	public readonly ulong[] BlobHashes = null;
	public byte[] ChunkData;
	public bool CacheEnabled;
	public uint SubChunkCount;
	public uint Count;
	public SubChunkRequestMode SubChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;

	partial void AfterEncode()
	{
		switch (SubChunkRequestMode)
		{
			case SubChunkRequestMode.SubChunkRequestModeLegacy:
			{
				WriteUnsignedVarInt(SubChunkCount);

				break;
			}

			case SubChunkRequestMode.SubChunkRequestModeLimitless:
			{
				WriteUnsignedVarInt(uint.MaxValue);
				break;
			}

			case SubChunkRequestMode.SubChunkRequestModeLimited:
			{
				WriteUnsignedVarInt(uint.MaxValue - 1);
				Write((ushort) SubChunkCount);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
		}

		Write(CacheEnabled);

		if (CacheEnabled)
		{
			foreach (ulong blobHashe in BlobHashes)
			{
				Write(blobHashe);
			}
		}

		WriteByteArray(ChunkData);
	}

	partial void AfterDecode()
	{
		uint subChunkCountButNotReally = ReadUnsignedVarInt();

		switch (subChunkCountButNotReally)
		{
			case uint.MaxValue:
				SubChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLimitless;
				break;
			case uint.MaxValue -1:
				SubChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLimited;
				SubChunkCount = (uint) ReadUshort();
				break;
			default:
				SubChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;
				SubChunkCount = subChunkCountButNotReally;
				break;
		}

		CacheEnabled = ReadBool();

		if (CacheEnabled)
		{
			Count = ReadUnsignedVarInt();
			for (int i = 0; i < Count; i++)
			{
				BlobHashes[i] = ReadUlong();
			}
		}

		ChunkData = ReadByteArray();
	}
}

public partial class McpeClientCacheBlobStatus : Packet<McpeClientCacheBlobStatus>
{
	public ulong[] hashMisses; // = null;
	public ulong[] hashHits; // = null;

	partial void AfterEncode()
	{
		WriteUnsignedVarInt((uint) hashMisses.Length);
		WriteUnsignedVarInt((uint) hashHits.Length);
		WriteSpecial(hashMisses);
		WriteSpecial(hashHits);
	}

	partial void AfterDecode()
	{
		uint lenMisses = ReadUnsignedVarInt();
		uint lenHits = ReadUnsignedVarInt();

		hashMisses = ReadUlongsSpecial(lenMisses);
		hashHits = ReadUlongsSpecial(lenHits);
	}

	public void WriteSpecial(ulong[] values)
	{
		if (values == null) return;

		if (values.Length == 0) return;
		foreach (ulong val in values)
		{
			Write(val);
		}
	}

	public ulong[] ReadUlongsSpecial(uint len)
	{
		ulong[] values = new ulong[len];
		for (int i = 0; i < values.Length; i++)
		{
			values[i] = ReadUlong();
		}
		return values;
	}
}

public partial class McpeClientCacheMissResponse : Packet<McpeClientCacheMissResponse>
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(McpeClientCacheMissResponse));

	public Dictionary<ulong, byte[]> Blobs;

	partial void AfterEncode()
	{
	}

	partial void AfterDecode()
	{
		Blobs = new Dictionary<ulong, byte[]>();
		uint count = ReadUnsignedVarInt();
		for (int i = 0; i < count; i++)
		{
			ulong hash = ReadUlong();
			byte[] blob = ReadByteArray();
			if (!Blobs.TryAdd(hash, blob))
			{
				Log.Warn($"Already had hash:{hash}. This is most likely air or water");
			}
		}
	}
}