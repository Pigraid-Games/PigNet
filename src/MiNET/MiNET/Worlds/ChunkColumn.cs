using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Net.Packets.Mcpe;
using MiNET.Utils.IO;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds;

public class ChunkColumn : ICloneable, IEnumerable<SubChunk>, IDisposable
{
	public const int WorldHeight = 256;
	public const int WorldMaxY = WorldHeight + WorldMinY;
	public const int WorldMinY = 0;

	private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkColumn));

	private readonly Random random = new();
	private McpeWrapper _cachedBatch;
	private object _cacheSync = new();

	private SubChunk[] _subChunks = new SubChunk[WorldHeight / 16];

	public byte[] biomeId;
	public short[] height;

	public ChunkColumn(bool clearBuffers = true)
	{
		biomeId = ArrayPool<byte>.Shared.Rent(256);
		height = ArrayPool<short>.Shared.Rent(256);

		if (clearBuffers) ClearBuffers();

		IsDirty = false;
	}

	public int X { get; set; }
	public int Z { get; set; }

	public bool IsAllAir { get; set; }

	//TODO: This dictionary need to be concurrent. Investigate performance before changing.
	public IDictionary<BlockCoordinates, NbtCompound> BlockEntities { get; private set; } = new Dictionary<BlockCoordinates, NbtCompound>();

	// Cache related. Should actually all be private, but well
	public bool IsDirty { get; set; }
	public bool NeedSave { get; set; }

	public bool DisableCache { get; set; }


	public SubChunk this[int chunkIndex, bool generateIfMissing = true]
	{
		get
		{
			SubChunk subChunk = _subChunks[chunkIndex];
			if (!generateIfMissing || subChunk != null) return subChunk;
			subChunk = SubChunk.CreateObject();
			_subChunks[chunkIndex] = subChunk;
			return subChunk;
		}
		set => _subChunks[chunkIndex] = value;
	}

	public object Clone()
	{
		var cc = (ChunkColumn) MemberwiseClone();

		cc._subChunks = new SubChunk[_subChunks.Length];
		for (int i = 0; i < _subChunks.Length; i++) cc._subChunks[i] = (SubChunk) _subChunks[i]?.Clone();

		cc.biomeId = (byte[]) biomeId.Clone();
		cc.height = (short[]) height.Clone();

		cc.BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();
		foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntityPair in BlockEntities) cc.BlockEntities.TryAdd(blockEntityPair.Key, (NbtCompound) blockEntityPair.Value.Clone());

		McpeWrapper batch = McpeWrapper.CreateObject();
		batch.payload = _cachedBatch.payload;
		batch.Encode();
		batch.MarkPermanent();

		cc._cachedBatch = batch;

		cc._cacheSync = new object();

		return cc;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public IEnumerator<SubChunk> GetEnumerator()
	{
		return _subChunks.Where(c => c != null).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void ClearBuffers()
	{
		Array.Clear(biomeId, 0, 256);
		Fill<byte>(biomeId, 1);
	}

	private void SetDirty()
	{
		IsDirty = true;
		NeedSave = true;
	}

	public int Count()
	{
		return _subChunks.Count(s => s != null);
	}

	public SubChunk GetSubChunk(int by)
	{
		by >>= 4;
		by += WorldMinY < 0 ? Math.Abs(WorldMinY >> 4) : 0;

		return this[Math.Clamp(by, 0, _subChunks.Length - 1)];
	}

	public int GetBlockId(int bx, int by, int bz)
	{
		SubChunk subChunk = GetSubChunk(by);
		return subChunk.GetBlockId(bx, by & 0xf, bz);
	}

	public Block GetBlockObject(int bx, int by, int bz)
	{
		SubChunk subChunk = GetSubChunk(by);
		return subChunk.GetBlockObject(bx, by & 0xf, bz);
	}

	public void SetBlock(int bx, int by, int bz, Block block)
	{
		SubChunk subChunk = GetSubChunk(by);
		subChunk.SetBlock(bx, by & 0xf, bz, block);
		SetDirty();
	}

	public void SetBlockByRuntimeId(int bx, int by, int bz, int runtimeId)
	{
		SubChunk subChunk = GetSubChunk(by);
		subChunk.SetBlockByRuntimeId(bx, by & 0xf, bz, runtimeId);
		SetDirty();
	}

	public void SetHeight(int bx, int bz, short h)
	{
		height[(bz << 4) + bx] = h;
		SetDirty();
	}

	public short GetHeight(int bx, int bz)
	{
		return height[(bz << 4) + bx];
	}

	public void SetBiome(int bx, int bz, byte biome)
	{
		biomeId[(bz << 4) + bx] = biome;
		SetDirty();
	}

	public byte GetBiome(int bx, int bz)
	{
		return biomeId[(bz << 4) + bx];
	}

	public byte GetBlocklight(int bx, int by, int bz)
	{
		SubChunk subChunk = GetSubChunk(by);
		return subChunk.GetBlocklight(bx, by & 0xf, bz);
	}

	public void SetBlocklight(int bx, int by, int bz, byte data)
	{
		SubChunk subChunk = GetSubChunk(by);
		subChunk.SetBlocklight(bx, by & 0xf, bz, data);
	}

	public byte GetSkylight(int bx, int by, int bz)
	{
		SubChunk subChunk = GetSubChunk(by);
		return subChunk.GetSkylight(bx, by & 0xf, bz);
	}

	public void SetSkyLight(int bx, int by, int bz, byte data)
	{
		SubChunk subChunk = GetSubChunk(by);
		subChunk.SetSkylight(bx, by & 0xf, bz, data);
	}

	public NbtCompound GetBlockEntity(BlockCoordinates coordinates)
	{
		BlockEntities.TryGetValue(coordinates, out NbtCompound nbt);

		// High cost clone. Consider alternative options on this.
		return (NbtCompound) nbt?.Clone();
	}

	public void SetBlockEntity(BlockCoordinates coordinates, NbtCompound nbt)
	{
		var blockEntity = (NbtCompound) nbt.Clone();
		BlockEntities[coordinates] = blockEntity;
		SetDirty();
	}

	public void RemoveBlockEntity(BlockCoordinates coordinates)
	{
		BlockEntities.Remove(coordinates);
		SetDirty();
	}


	/// <summary>Blends the specified colors together.</summary>
	/// <param name="color">Color to blend onto the background color.</param>
	/// <param name="backColor">Color to blend the other color onto.</param>
	/// <param name="amount">
	///     How much of <paramref name="color" /> to keep,
	///     “on top of” <paramref name="backColor" />.
	/// </param>
	/// <returns>The blended colors.</returns>
	public static Color Blend(Color color, Color backColor, double amount)
	{
		byte r = (byte) ((color.R * amount) + (backColor.R * (1 - amount)));
		byte g = (byte) ((color.G * amount) + (backColor.G * (1 - amount)));
		byte b = (byte) ((color.B * amount) + (backColor.B * (1 - amount)));
		return Color.FromArgb(r, g, b);
	}

	public Color CombineColors(params Color[] aColors)
	{
		int r = 0;
		int g = 0;
		int b = 0;
		foreach (Color c in aColors)
		{
			r += c.R;
			g += c.G;
			b += c.B;
		}

		r /= aColors.Length;
		g /= aColors.Length;
		b /= aColors.Length;

		return Color.FromArgb(r, g, b);
	}

	private void InterpolateBiomes()
	{
		for (int bx = 0; bx < 16; bx++)
		for (int bz = 0; bz < 16; bz++)
		{
			Color c = CombineColors(
				GetBiomeColor(bx, bz),
				GetBiomeColor(bx - 1, bz - 1),
				GetBiomeColor(bx - 1, bz),
				GetBiomeColor(bx, bz - 1),
				GetBiomeColor(bx + 1, bz + 1),
				GetBiomeColor(bx + 1, bz),
				GetBiomeColor(bx, bz + 1),
				GetBiomeColor(bx - 1, bz + 1),
				GetBiomeColor(bx + 1, bz - 1)
			);
			//SetBiomeColor(bx, bz, c.ToArgb());
		}

		//SetBiomeColor(0, 0, Color.GreenYellow.ToArgb());
		//SetBiomeColor(0, 15, Color.Blue.ToArgb());
		//SetBiomeColor(15, 0, Color.Red.ToArgb());
		//SetBiomeColor(15, 15, Color.Yellow.ToArgb());
	}

	private Color GetBiomeColor(int bx, int bz)
	{
		if (bx < 0) bx = 0;
		if (bz < 0) bz = 0;
		if (bx > 15) bx = 15;
		if (bz > 15) bz = 15;

		var utils = new BiomeUtils();
		byte biome = GetBiome(bx, bz);
		int color = utils.ComputeBiomeColor(biome, 0, true);

		if (random.Next(30) == 0)
		{
			var col = Color.FromArgb(color);
			color = Color.FromArgb(0, Math.Max(0, col.R - 160), Math.Max(0, col.G - 160), Math.Max(0, col.B - 160)).ToArgb();
		}

		return Color.FromArgb(color);
	}

	public static unsafe void FastFill<T>(ref T[] data, T value2, ulong value) where T : unmanaged
	{
		fixed (T* shorts = data)
		{
			byte* bytes = (byte*) shorts;
			int len = data.Length * sizeof(T);
			int rem = len % (sizeof(long) * 16);
			ulong* b = (ulong*) bytes;
			ulong* e = (ulong*) (shorts + len - rem);

			while (b < e)
			{
				*b = value;
				*(b + 1) = value;
				*(b + 2) = value;
				*(b + 3) = value;
				*(b + 4) = value;
				*(b + 5) = value;
				*(b + 6) = value;
				*(b + 7) = value;
				*(b + 8) = value;
				*(b + 9) = value;
				*(b + 10) = value;
				*(b + 11) = value;
				*(b + 12) = value;
				*(b + 13) = value;
				*(b + 14) = value;
				*(b + 15) = value;
				b += 16;
			}

			for (int i = 0; i < rem; i++) data[len - 1 - i] = value2;
		}
	}


	public static void Fill<T>(T[] destinationArray, params T[] value)
	{
		if (destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));

		if (value.Length >= destinationArray.Length) throw new ArgumentException("Length of value array must be less than length of destination");

		// set the initial array value
		Array.Copy(value, destinationArray, value.Length);

		int arrayToFillHalfLength = destinationArray.Length / 2;
		int copyLength;

		for (copyLength = value.Length; copyLength < arrayToFillHalfLength; copyLength <<= 1) Array.Copy(destinationArray, 0, destinationArray, copyLength, copyLength);

		Array.Copy(destinationArray, 0, destinationArray, copyLength, destinationArray.Length - copyLength);
	}

	public void RecalcHeight()
	{
		for (int x = 0; x < 16; x++)
		for (int z = 0; z < 16; z++)
			RecalcHeight(x, z);
	}

	public void RecalcHeight(int x, int z, int startY = WorldMaxY)
	{
		bool isInLight = true;
		bool isInAir = true;
		int warnCount = 0;
		const int maxWarns = 5;

		for (int y = startY; y >= 0; y--)
			if (isInLight)
			{
				SubChunk chunk = GetSubChunk(y);
				if (isInAir && chunk.IsAllAir())
				{
					if (chunk.IsDirty)
						Array.Fill<byte>(chunk._skylight.Data, 0xff);
					y -= 15;
					continue;
				}

				isInAir = false;

				int bid = GetBlockId(x, y, z);

				if (bid < 0 || bid >= BlockFactory.TransparentBlocks.Count || !BlockFactory.TransparentBlocks.ContainsKey(bid))
				{
					if (++warnCount >= maxWarns)
					{
						Log.Error($"Too many missing block warnings (ID {bid}) at ({x}, {y}, {z}). Stopping recalculation.");
						break;
					}

					if (bid != 0)
					{
						SetHeight(x, z, (short) (y + 1));
						SetSkyLight(x, y, z, 0);
						isInLight = false;
					}
					continue;
				}

				if (bid == 0 || (BlockFactory.TransparentBlocks[bid] == 1 && bid != 18 && bid != 30 && bid != 8 && bid != 9))
					SetSkyLight(x, y, z, 15);
				else
				{
					SetHeight(x, z, (short) (y + 1));
					SetSkyLight(x, y, z, 0);
					isInLight = false;
				}
			}
			else
				SetSkyLight(x, y, z, 0);
	}

	public int GetRecalatedHeight(int x, int z)
	{
		bool isInAir = true;

		try
		{
			for (int y = WorldHeight; y >= WorldMinY; y--)
			{
				SubChunk chunk = GetSubChunk(y);
				if (chunk == null) continue;

				if (isInAir && chunk.IsAllAir())
				{
					if (chunk.IsDirty && chunk._skylight?.Data != null) Array.Fill<byte>(chunk._skylight.Data, 0xff);
					y -= 15;
					continue;
				}

				isInAir = false;

				int bid = GetBlockId(x, y, z);
				if (bid == 0 || (BlockFactory.TransparentBlocks?.Count > bid && BlockFactory.TransparentBlocks![bid] == 1 && bid != 18 && bid != 30))
					continue;

				return y + 1;
			}
		}
		catch (Exception ex)
		{
			Log.Error($"Error in GetRecalatedHeight: {ex.Message}", ex);
		}

		return 0;
	}


	internal void ClearCache()
	{
		lock (_cacheSync)
			if (_cachedBatch != null)
			{
				_cachedBatch.MarkPermanent(false);
				_cachedBatch.PutPool();

				_cachedBatch = null;
			}
	}

	public McpeWrapper GetBatch()
	{
		lock (_cacheSync)
		{
			if (!DisableCache && !IsDirty && _cachedBatch != null) return _cachedBatch;

			ClearCache();

			int topEmpty = GetTopEmpty();
			byte[] chunkData = GetBytes(topEmpty);

			McpeLevelChunk fullChunkPacket = McpeLevelChunk.CreateObject();
			fullChunkPacket.cacheEnabled = false;
			fullChunkPacket.subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;
			fullChunkPacket.chunkX = X;
			fullChunkPacket.chunkZ = Z;
			fullChunkPacket.subChunkCount = (uint) topEmpty + 4;
			fullChunkPacket.chunkData = chunkData;
			byte[] bytes = fullChunkPacket.Encode();
			fullChunkPacket.PutPool();

			McpeWrapper batch = BatchUtils.CreateBatchPacket(new Memory<byte>(bytes, 0, bytes.Length), CompressionLevel.Fastest, true);
			batch.MarkPermanent();

			_cachedBatch = batch;
			IsDirty = false;

			return _cachedBatch;
		}
	}


	public byte[] GetBytes(int topEmpty)
	{
		using var stream = new MemoryStream();

		for (int i = 0; i < 4; i++) //fill up negative chunks to support world format
		{
			stream.WriteByte(8);
			stream.WriteByte(0); // empty
		}

		for (int ci = 0; ci < topEmpty; ci++) this[ci].Write(stream);

		byte[] biomePalette = GetBiomePalette(biomeId);
		stream.Write(biomePalette, 0, biomePalette.Length);

		stream.WriteByte(0); // Border blocks - nope (EDU)

		if (BlockEntities.Count == 0) return stream.ToArray();
		foreach (NbtCompound blockEntity in BlockEntities.Values.ToArray())
		{
			var file = new NbtFile(blockEntity)
			{
				BigEndian = false,
				UseVarInt = true
			};
			file.SaveToStream(stream, NbtCompression.None);
		}

		return stream.ToArray();
	}

	private byte[] GetBiomePalette(byte[] biomes)
	{
		for (int b = 0; b < biomes.Length; b++)
			if (biomes[b] == 255)
				biomes[b] = 0;
		using var stream = new MemoryStream();

		var uniqueBiomes = biomes.Distinct().Select(x => (int) x).ToList();

		short[] newBiomes = new short[16 * 16 * 16];
		for (int x = 0; x < 16; x++)
		for (int z = 0; z < 16; z++)
		{
			int currentBiome = biomes[(z << 4) + x];

			for (int y = 0; y < 16; y++)
				//var index = ((y >> 2) << 4) | ((z >> 2) << 2) | (x >> 2);
				newBiomes[(x << 8) | (z << 4) | y] = (short) uniqueBiomes.IndexOf(currentBiome);
		}

		for (int i = 0; i < 24; i++) SubChunk.WriteStore(stream, newBiomes, null, false, uniqueBiomes);

		return stream.ToArray();
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal int GetTopEmpty()
	{
		int topEmpty = WorldHeight / 16;
		for (int ci = (WorldHeight / 16) - 1; ci >= 0; ci--)
			// Maybe reconsider if this is what we really want to do. Pooling buffers may remove the need for it. It's just an object.
			if (_subChunks[ci] == null || _subChunks[ci].IsAllAir())
			{
				topEmpty = ci;
				_subChunks[ci]?.PutPool();
				_subChunks[ci] = null;
			}
			else
				break;
		return topEmpty;
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (biomeId != null) ArrayPool<byte>.Shared.Return(biomeId);
			if (height != null) ArrayPool<short>.Shared.Return(height);
		}
	}

	~ChunkColumn()
	{
		Dispose(false);
	}
}

public static class ArrayOf<T> where T : new()
{
	public static T[] Create(int size, T initialValue)
	{
		var array = (T[]) Array.CreateInstance(typeof(T), size);
		for (int i = 0; i < array.Length; i++)
			array[i] = initialValue;
		return array;
	}

	public static T[] Create(int size)
	{
		var array = (T[]) Array.CreateInstance(typeof(T), size);
		for (int i = 0; i < array.Length; i++)
			array[i] = new T();
		return array;
	}
}