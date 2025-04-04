﻿#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using log4net;
using PigNet.Blocks;
using PigNet.Utils;

namespace PigNet.Worlds;

public class SubChunk : IDisposable, ICloneable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(SubChunk));

	public static readonly Queue<SubChunk> Pool = new();

	private static readonly object poolLock = new();

	// Consider disabling these if we don't calculate lights
	public NibbleArray _blocklight;

	private byte[] _cache;

	private bool _isAllAir = true;

	public NibbleArray _skylight;

	public SubChunk(bool clearBuffers = true)
	{
		RuntimeIds = new List<int> { BlockFactory.GetBlockByName("minecraft:air").GetRuntimeId() };

		Blocks = ArrayPool<short>.Shared.Rent(4096);
		LoggedBlocks = ArrayPool<byte>.Shared.Rent(4096);
		_blocklight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));
		_skylight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));

		if (clearBuffers)
			ClearBuffers();
	}

	internal List<int> RuntimeIds { get; private set; }

	internal short[] Blocks { get; private set; }

	internal List<int> LoggedRuntimeIds { get; private set; } = new();

	internal byte[] LoggedBlocks { get; private set; }

	public bool IsDirty { get; private set; }

	public ulong Hash { get; set; }
	public bool DisableCache { get; set; } = true;

	public object Clone()
	{
		SubChunk cc = CreateObject();
		cc._isAllAir = _isAllAir;
		cc.IsDirty = IsDirty;

		cc.RuntimeIds = new List<int>(RuntimeIds);
		Blocks.CopyTo(cc.Blocks, 0);
		cc.LoggedRuntimeIds = new List<int>(LoggedRuntimeIds);
		LoggedBlocks.CopyTo(cc.LoggedBlocks, 0);
		_blocklight.Data.CopyTo(cc._blocklight.Data, 0);
		_skylight.Data.CopyTo(cc._skylight.Data, 0);

		if (_cache != null) cc._cache = (byte[]) _cache.Clone();

		return cc;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public void ClearBuffers()
	{
		Array.Clear(Blocks, 0, 4096);
		Array.Clear(LoggedBlocks, 0, 4096);
		Array.Clear(_blocklight.Data, 0, 2048);
		ChunkColumn.Fill<byte>(_skylight.Data, 0xff);
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsAllAir()
	{
		if (IsDirty) _isAllAir = AllZeroFast(Blocks);
		return _isAllAir;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AllZeroFast<T>(T[] data) where T : unmanaged
	{
		if (data == null || data.Length == 0)
			return true;

		int vectorSize = Vector<T>.Count;
		int i = 0;
		int length = data.Length;

		while (i <= length - (vectorSize * 4))
		{
			var v1 = new Vector<T>(data, i);
			var v2 = new Vector<T>(data, i + vectorSize);
			var v3 = new Vector<T>(data, i + (vectorSize * 2));
			var v4 = new Vector<T>(data, i + (vectorSize * 3));

			if (!Vector.EqualsAll(v1, Vector<T>.Zero) ||
				!Vector.EqualsAll(v2, Vector<T>.Zero) ||
				!Vector.EqualsAll(v3, Vector<T>.Zero) ||
				!Vector.EqualsAll(v4, Vector<T>.Zero))
				return false;

			i += vectorSize * 4;
		}

		return true;
	}

	private static int GetIndex(int bx, int by, int bz)
	{
		return (bx << 8) | (bz << 4) | by;
	}

	public int GetBlockId(int bx, int by, int bz)
	{
		if (RuntimeIds.Count == 0)
			return 0;

		int paletteIndex = Blocks[GetIndex(bx, by, bz)];
		int runtimeId = RuntimeIds[paletteIndex];
		if (runtimeId == -1) runtimeId = BlockFactory.GetBlockById(0).GetRuntimeId();
		BlockFactory.BlockPalette.TryGetValue(runtimeId, out BlockStateContainer blockState);
		int bid = blockState.Id;
		return bid == -1 ? 0 : bid;
	}

	public Block GetBlockObject(int bx, int by, int bz)
	{
		if (RuntimeIds.Count == 0)
			return new Air();

		int index = Blocks[GetIndex(bx, by, bz)];
		int runtimeId = RuntimeIds[index];
		if (runtimeId == -1) runtimeId = BlockFactory.GetBlockById(0).GetRuntimeId();
		BlockFactory.BlockPalette.TryGetValue(runtimeId, out BlockStateContainer blockState);
		Block block = BlockFactory.GetBlockById(blockState.Id);
		block.SetState(blockState.States);
		block.Metadata = (byte) blockState.Data; //TODO: REMOVE metadata. Not needed.

		return block;
	}

	public void SetBlock(int bx, int by, int bz, Block block)
	{
		int runtimeId = block.GetRuntimeId();
		if (runtimeId < 0)
			return;

		SetBlockByRuntimeId(bx, by, bz, runtimeId);
	}

	public void SetBlockByRuntimeId(int bx, int by, int bz, int runtimeId)
	{
		int paletteIndex = RuntimeIds.IndexOf(runtimeId);
		if (paletteIndex == -1)
		{
			RuntimeIds.Add(runtimeId);
			paletteIndex = RuntimeIds.IndexOf(runtimeId);
		}

		Blocks[GetIndex(bx, by, bz)] = (short) paletteIndex;
		_cache = null;
		IsDirty = true;
	}

	public void SetBlockIndex(int bx, int by, int bz, short paletteIndex)
	{
		Blocks[GetIndex(bx, by, bz)] = paletteIndex;
		_cache = null;
		IsDirty = true;
	}


	public void SetLoggedBlock(int bx, int by, int bz, Block block)
	{
		int runtimeId = block.GetRuntimeId();
		if (runtimeId < 0)
			return;

		SetLoggedBlockByRuntimeId(bx, by, bz, runtimeId);
	}

	public void SetLoggedBlockByRuntimeId(int bx, int by, int bz, int runtimeId)
	{
		int paletteIndex = LoggedRuntimeIds.IndexOf(runtimeId);
		if (paletteIndex == -1)
		{
			LoggedRuntimeIds.Add(runtimeId);
			paletteIndex = (byte) LoggedRuntimeIds.IndexOf(runtimeId);
		}

		LoggedBlocks[GetIndex(bx, by, bz)] = (byte) paletteIndex;
		_cache = null;
		IsDirty = true;
	}

	public void SetLoggedBlockIndex(int bx, int by, int bz, byte paletteIndex)
	{
		LoggedBlocks[GetIndex(bx, by, bz)] = paletteIndex;
		_cache = null;
		IsDirty = true;
	}

	public byte GetBlocklight(int bx, int by, int bz)
	{
		return _blocklight[GetIndex(bx, by, bz)];
	}

	public void SetBlocklight(int bx, int by, int bz, byte data)
	{
		_blocklight[GetIndex(bx, by, bz)] = data;
	}

	public byte GetSkylight(int bx, int by, int bz)
	{
		return _skylight[GetIndex(bx, by, bz)];
	}

	public void SetSkylight(int bx, int by, int bz, byte data)
	{
		_skylight[GetIndex(bx, by, bz)] = data;
	}

	public void Write(MemoryStream stream)
	{
		if (!DisableCache && !IsDirty && _cache != null)
		{
			stream.Write(_cache);
			return;
		}

		long startPos = stream.Position;

		stream.WriteByte(8); // version

		int numberOfStores = 0;

		List<int> runtimeIds = RuntimeIds;
		short[] blocks = Blocks;

		if (runtimeIds != null && runtimeIds.Count > 0)
			numberOfStores++;

		List<int> loggedRuntimeIds = LoggedRuntimeIds;
		byte[] loggedBlocks = LoggedBlocks;

		if (loggedRuntimeIds != null && loggedRuntimeIds.Count > 0)
			numberOfStores++;

		stream.WriteByte((byte) numberOfStores); // storage size

		if (WriteStore(stream, blocks, null, false, runtimeIds))
			//numberOfStores++;
			if (WriteStore(stream, null, loggedBlocks, false, loggedRuntimeIds))
			{
				//numberOfStores++;
			}

		int length = (int) (stream.Position - startPos);

		//stream.Position = storePosition;
		//stream.WriteByte((byte) numberOfStores); // storage size

		//if (DisableCache)
		{
			byte[] bytes = new byte[length];
			stream.Position = startPos;
			int read = stream.Read(bytes, 0, length);
			if (read != length)
				throw new InvalidDataException($"Read wrong amount of data. Expected {length} but read {read}");
			if (startPos + length != stream.Position)
				throw new InvalidDataException($"Expected {startPos + length} but was {stream.Position}");

			_cache = bytes;
		}

		IsDirty = false;
	}

	public static bool WriteStore(MemoryStream stream, short[] blocks, byte[] loggedBlocks, bool forceWrite, List<int> palette)
	{
		if (palette.Count == 0)
			return false;

		// log2(number of entries) => bits needed to store them
		int bitsPerBlock = (int) Math.Ceiling(Math.Log(palette.Count, 2));

		switch (bitsPerBlock)
		{
			case 0:
				if (!forceWrite && palette.Contains(0))
					return false;
				bitsPerBlock = 1;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				//Paletted1 = 1,   // 32 blocks per word
				//Paletted2 = 2,   // 16 blocks per word
				//Paletted3 = 3,   // 10 blocks and 2 bits of padding per word
				//Paletted4 = 4,   // 8 blocks per word
				//Paletted5 = 5,   // 6 blocks and 2 bits of padding per word
				//Paletted6 = 6,   // 5 blocks and 2 bits of padding per word
				break;
			case 7:
			case 8:
				//Paletted8 = 8,  // 4 blocks per word
				bitsPerBlock = 8;
				break;
			case int i when i > 8:
				//Paletted16 = 16, // 2 blocks per word
				bitsPerBlock = 16;
				break;
		}

		stream.WriteByte((byte) ((bitsPerBlock << 1) | 1)); // flags

		int blocksPerWord = (int) Math.Floor(32f / bitsPerBlock); // Floor to remove padding bits
		int wordsPerChunk = (int) Math.Ceiling(4096f / blocksPerWord);

		uint[] indexes = new uint[wordsPerChunk];

		int position = 0;
		for (int w = 0; w < wordsPerChunk; w++)
		{
			uint word = 0;
			for (int block = 0; block < blocksPerWord; block++)
			{
				if (position >= 4096)
					continue;

				uint state;
				if (blocks != null)
					state = (uint) blocks[position];
				else
					state = loggedBlocks[position];
				word |= state << (bitsPerBlock * block);

				position++;
			}
			indexes[w] = word;
		}

		byte[] ba = new byte[indexes.Length * 4];
		Buffer.BlockCopy(indexes, 0, ba, 0, indexes.Length * 4);

		stream.Write(ba, 0, ba.Length);

		VarInt.WriteSInt32(stream, palette.Count); // count
		foreach (int val in palette) VarInt.WriteSInt32(stream, val);

		return true;
	}

	public static SubChunk CreateObject()
	{
		return new SubChunk();
		//return GetObject(); would be nice to fix
	}

	public void PutPool()
	{
		Dispose();
		//REMOVEReset();
		//ReturnObject(this);
	}

	public void REMOVEReset()
	{
		_isAllAir = true;
		IsDirty = false;
		Hash = 0;
		DisableCache = true;

		RuntimeIds.Clear();
		LoggedRuntimeIds.Clear();

		Blocks = ArrayPool<short>.Shared.Rent(4096);
		LoggedBlocks = ArrayPool<byte>.Shared.Rent(4096);
		_blocklight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));
		_skylight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));

		Array.Clear(Blocks, 0, Blocks.Length);
		Array.Clear(LoggedBlocks, 0, LoggedBlocks.Length);
		Array.Clear(_blocklight.Data, 0, _blocklight.Data.Length);
		Array.Fill<byte>(_skylight.Data, 0xff);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (Blocks != null)
				ArrayPool<short>.Shared.Return(Blocks);
			if (LoggedBlocks != null)
				ArrayPool<byte>.Shared.Return(LoggedBlocks);
			if (_blocklight != null)
				ArrayPool<byte>.Shared.Return(_blocklight.Data);
			if (_skylight != null)
				ArrayPool<byte>.Shared.Return(_skylight.Data);
		}
	}

	public static SubChunk GetObject()
	{
		lock (poolLock)
		{
			if (Pool.Count > 0)
			{
				SubChunk subChunk = Pool.Dequeue();
				return subChunk;
			}

			return new SubChunk();
		}
	}

	public static void ReturnObject(SubChunk subChunk)
	{
		lock (poolLock) Pool.Enqueue(subChunk);
	}

	~SubChunk()
	{
		Dispose(false);
	}
}