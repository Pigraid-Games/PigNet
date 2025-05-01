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

using PigNet.Worlds.Utils;

namespace PigNet.Worlds.Anvil;

public class AnvilDataUtils
{
	public static void ReadAnyBitLengthShortFromLongs(long[] longs, short[] shorts, byte shortSize)
	{
		var longBitSize = sizeof(long) * 8;
		var valueBits = (1 << shortSize) - 1;

		var shortsInLongCount = longBitSize / shortSize;

		for (var i = 0; i < shorts.Length; i++)
		{
			var offset = i % shortsInLongCount * shortSize;
			var longsOffset = i / shortsInLongCount;

			shorts[i] = (short) (longs[longsOffset] >> offset & valueBits);
		}
	}

	public static void ReadAnvilPalettedContainerData(long[] longWords, PalettedContainerData data, byte longBlockSize)
	{
		var blockSize = data.DataProfile.BlockSize;
		var blockMask = (1 << longBlockSize) - 1;
		var blocksPerWord = data.DataProfile.BlocksPerWord;
		var blocksCount = data.BlocksCount;
		var words = data.Data;

		var longWordSize = sizeof(long) * 8;
		var blocksPerLongWord = longWordSize / longBlockSize;

		for (var i = 0; i != blocksCount; i++)
		{
			var index = (i & 0x0F0 | i >> 8 | i << 8) & 0xFFF;
			ref var word = ref words[index / blocksPerWord];

			var longShift = i % blocksPerLongWord * longBlockSize;
			var shift = index % blocksPerWord * blockSize;
			word |= (int) (longWords[i / blocksPerLongWord] >> longShift & blockMask) << shift;
		}
	}

	public static void ReadAnyBitLengthShortFromLongs(long[] longs, byte[] shorts, byte shortSize)
	{
		var longBitSize = sizeof(long) * 8;
		var valueBits = (1 << shortSize) - 1;

		var shortsInLongCount = longBitSize / shortSize;

		for (var i = 0; i < shorts.Length; i++)
		{
			var offset = i % shortsInLongCount * shortSize;
			var longsOffset = i / shortsInLongCount;

			shorts[i] = (byte) (longs[longsOffset] >> offset & valueBits);
		}
	}
}