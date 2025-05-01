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
using System.Security.Cryptography;
using PigNet.Utils.Vectors;

namespace PigNet.Worlds.Anvil;

public class AnvilBiomeManager
{
	private static readonly int MinNoiseY = FromBlock(ChunkColumn.WorldMinY);
	private static readonly int MaxNoiseY = MinNoiseY + FromBlock(ChunkColumn.WorldHeight) - 1;

	private Lazy<long> _obfuscatedSeed;

	public long ObfuscatedSeed => _obfuscatedSeed.Value;

	private readonly AnvilWorldProvider _worldProvider;

	public AnvilBiomeManager(AnvilWorldProvider worldProvider)
	{
		_worldProvider = worldProvider;

		_obfuscatedSeed = new Lazy<long>(() => ObfuscateSeed(_worldProvider.LevelInfo.GenerationSettings.Seed));
	}

	public byte GetNoiseBiome(int x, int y, int z)
	{
		var chunk = _worldProvider.GenerateChunkColumn(new ChunkCoordinates(FromBlock(x), FromBlock(z)));

		int fixedY = Math.Clamp(y, MinNoiseY, MaxNoiseY);
		int j = GetSectionIndex(ToBlock(fixedY));
		var subChunk = chunk[j];

		if (subChunk is AnvilSubChunk section)
		{
			return section.GetNoiseBiome(x, fixedY, z);
		}
		else
		{
			return subChunk.GetBiome((x << 2) & 0xf, (fixedY << 2) & 0xf, (z << 2) & 0xf);
		}
	}

	private static int FromBlock(int value)
	{
		return value >> 2;
	}

	private static int ToBlock(int value)
	{
		return value << 2;
	}

	private static int GetSectionIndex(int value)
	{
		return BlockToSectionCoord(value) + 4;
	}

	private static int BlockToSectionCoord(int value)
	{
		return value >> 4;
	}

	private static long ObfuscateSeed(long seed)
	{
		using (var sha256Hash = SHA256.Create())
		{
			var bytes = sha256Hash.ComputeHash(BitConverter.GetBytes(seed));
			return BitConverter.ToInt64(bytes);
		}
	}
}