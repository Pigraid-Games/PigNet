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

namespace MiNET.Net.Packets.Mcpe;

public enum SubChunkRequestMode
{
	SubChunkRequestModeLegacy,
	SubChunkRequestModeLimitless,
	SubChunkRequestModeLimited
}

public class McpeLevelChunk : Packet<McpeLevelChunk>
{
	public ulong[] blobHashes = null;
	public bool cacheEnabled;
	public byte[] chunkData;
	public int chunkX;
	public int chunkZ;
	public int dimension;
	public uint count;
	//public bool subChunkRequestsEnabled;
	public uint subChunkCount;
	public SubChunkRequestMode subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;
	
	public McpeLevelChunk()
	{
		Id = 0x3a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(chunkX);
		WriteSignedVarInt(chunkZ);
		WriteSignedVarInt(0); //dimension id. TODO if dimensions will ever be added back again....
		switch (subChunkRequestMode)
		{
			case SubChunkRequestMode.SubChunkRequestModeLegacy:
			{
				WriteUnsignedVarInt(subChunkCount);

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
				Write((ushort) subChunkCount);
				break;
			}
		}

		Write(cacheEnabled);

		if (cacheEnabled)
			foreach (ulong blobHashe in blobHashes)
				Write(blobHashe);

		WriteByteArray(chunkData);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		chunkX = ReadSignedVarInt();
		chunkZ = ReadSignedVarInt();
		dimension = ReadSignedVarInt();
		uint subChunkCountButNotReally = ReadUnsignedVarInt();

		switch (subChunkCountButNotReally)
		{
			case uint.MaxValue:
				subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLimitless;
				break;
			case uint.MaxValue - 1:
				subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLimited;
				subChunkCount = ReadUshort();
				break;
			default:
				subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;
				subChunkCount = subChunkCountButNotReally;
				break;
		}

		cacheEnabled = ReadBool();

		if (cacheEnabled)
		{
			count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++) blobHashes[i] = ReadUlong();
		}

		chunkData = ReadByteArray();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		chunkX = default;
		chunkZ = default;
		dimension = default;
	}
}