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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils.Vectors;
using Color = System.Drawing.Color;
// ReSharper disable MemberCanBePrivate.Global

namespace MiNET.Worlds;

public class SkyLightBlockAccess(IWorldProvider worldProvider, int heightForUnloadedChunk = 255)
	: IBlockAccess
{
	private readonly ChunkColumn _chunk;
	private readonly ChunkCoordinates _coordinates = ChunkCoordinates.None;

	public SkyLightBlockAccess(IWorldProvider worldProvider, ChunkColumn chunk) : this(worldProvider, -1)
	{
		_chunk = chunk;
		_coordinates = new ChunkCoordinates(chunk.X, chunk.Z);
	}

	public ChunkColumn GetChunk(BlockCoordinates coordinates, bool cacheOnly = false)
	{
		return GetChunk((ChunkCoordinates) coordinates, cacheOnly);
	}

	public ChunkColumn GetChunk(ChunkCoordinates coordinates, bool cacheOnly = false)
	{
		if (coordinates == _coordinates) return _chunk;

		if (_coordinates == ChunkCoordinates.None) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates + ChunkCoordinates.Backward) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates + ChunkCoordinates.Forward) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates + ChunkCoordinates.Left) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates + ChunkCoordinates.Right) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates + ChunkCoordinates.Backward + ChunkCoordinates.Left) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates + ChunkCoordinates.Backward + ChunkCoordinates.Right) return worldProvider.GenerateChunkColumn(coordinates, true);
		if (coordinates == _coordinates + ChunkCoordinates.Forward + ChunkCoordinates.Left) return worldProvider.GenerateChunkColumn(coordinates, true);
		return coordinates != _coordinates + ChunkCoordinates.Forward + ChunkCoordinates.Right ? null : worldProvider.GenerateChunkColumn(coordinates, true);
	}

	public void SetSkyLight(BlockCoordinates coordinates, byte skyLight)
	{
		ChunkColumn chunk = GetChunk(coordinates, true);
		chunk?.SetSkyLight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f, skyLight);
	}

	public int GetHeight(BlockCoordinates coordinates)
	{
		ChunkColumn chunk = GetChunk(coordinates, true);
		return chunk?.GetHeight(coordinates.X & 0x0f, coordinates.Z & 0x0f) ?? heightForUnloadedChunk;
	}

	public Block GetBlock(BlockCoordinates coordinates, ChunkColumn tryChunk = null)
	{
		return null;
	}

	public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true, ChunkColumn possibleChunk = null)
	{
	}
}

public class SkyLightCalculations(bool trackResults = false)
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(SkyLightCalculations));

	private readonly ConcurrentDictionary<ChunkColumn, bool> _visitedColumns = new();

	static SkyLightCalculations()
	{
	}

	// Debug tracking, don't enable unless you really need to "see it".

	public bool TrackResults { get; } = trackResults;
	public ConcurrentDictionary<BlockCoordinates, int> Visits { get; } = new();
	public long StartTimeInMilliseconds { get; set; }

	public int CalculateSkyLights(IBlockAccess level, ChunkColumn[] chunks)
	{
		const int CalcCount = 0;
		var calcTime = new Stopwatch();

		foreach (ChunkColumn chunk in chunks)
		{
			if (!_visitedColumns.TryAdd(chunk, true)) continue;

			if (chunk.IsAllAir) continue;

			calcTime.Restart();
			if (RecalculateSkyLight(chunk, level))
			{
				//calcCount++;

				//var elapsedMilliseconds = calcTime.ElapsedMilliseconds;
				//var c = Visits.Sum(pair => pair.Value);
				//if (elapsedMilliseconds > 0) Log.Debug($"Recalc skylight chunk {chunk.x}, {chunk.z}, count #{calcCount} (air={chunk.isAllAir}) chunks. Time {elapsedMilliseconds}ms and {c - lastCount} visits");
				//lastCount = c;
				//PrintVisits();
			}
		}

		//Log.Debug($"Recalculate skylight for #{calcCount} chunk. Made {lastCount} visits.");

		return CalcCount;
	}

	public bool RecalculateSkyLight(ChunkColumn chunk, IBlockAccess level)
	{
		if (chunk == null) return false;

		var lightBfQueue = new Queue<BlockCoordinates>();
		var lightBfSet = new HashSet<BlockCoordinates>();
		for (int x = 0; x < 16; x++)
		for (int z = 0; z < 16; z++)
		{
			if (chunk.IsAllAir && !IsOnChunkBorder(x, z)) continue;

			int height = GetHighestSurrounding(x, z, chunk, level);
			if (height == 0) continue;
			var coordinates = new BlockCoordinates(x + (chunk.X * 16), height, z + (chunk.Z * 16));
			lightBfQueue.Enqueue(coordinates);
			lightBfSet.Add(coordinates);
		}

		Calculate(level, lightBfQueue, lightBfSet);
		return true;
	}

	public void Calculate(Level level, BlockCoordinates coordinates)
	{
		int currentLight = level.GetSkyLight(coordinates);

		ChunkColumn chunk = level.GetChunk(coordinates);
		int height = chunk.GetRecalatedHeight(coordinates.X & 0x0f, coordinates.Z & 0x0f);

		var sourceQueue = new Queue<BlockCoordinates>();
		sourceQueue.Enqueue(coordinates);
		if (currentLight != 0)
		{
			var resetQueue = new Queue<BlockCoordinates>();
			var visits = new HashSet<BlockCoordinates>();

			// Reset all lights that potentially derive from this
			resetQueue.Enqueue(coordinates);

			var deleteQueue = new Queue<BlockCoordinates>();
			while (resetQueue.Count > 0)
			{
				BlockCoordinates coord = resetQueue.Dequeue();
				if (!visits.Add(coord)) continue;

				if (coord.DistanceTo(coordinates) > 16) continue;

				ResetLight(level, resetQueue, sourceQueue, coord);
				if (!sourceQueue.Contains(coord)) deleteQueue.Enqueue(coord);
			}

			level.SetSkyLight(coordinates, 0);

			foreach (BlockCoordinates delete in deleteQueue) level.SetSkyLight(delete, 0);
		}
		else
		{
			sourceQueue.Enqueue(coordinates);
			sourceQueue.Enqueue(coordinates.BlockUp());
			sourceQueue.Enqueue(coordinates.BlockDown());
			sourceQueue.Enqueue(coordinates.BlockWest());
			sourceQueue.Enqueue(coordinates.BlockEast());
			sourceQueue.Enqueue(coordinates.BlockNorth());
			sourceQueue.Enqueue(coordinates.BlockSouth());
		}

		chunk.SetHeight(coordinates.X & 0x0f, coordinates.Z & 0x0f, (short) height);

		// Recalculate
		var lightBfQueue = new Queue<BlockCoordinates>(sourceQueue);
		var lightBfSet = new HashSet<BlockCoordinates>(sourceQueue);

		var blockAccess = new SkyLightBlockAccess(level.WorldProvider);
		Calculate(blockAccess, lightBfQueue, lightBfSet);
	}

	public static void ResetLight(Level level, Queue<BlockCoordinates> resetQueue, Queue<BlockCoordinates> sourceQueue, BlockCoordinates coordinates)
	{
		int currentLight = level.GetSkyLight(coordinates);

		if (coordinates.Y < 255) TestForSource(level, resetQueue, sourceQueue, coordinates.BlockUp(), currentLight);
		if (coordinates.Y > 0) TestForSource(level, resetQueue, sourceQueue, coordinates.BlockDown(), currentLight, true);
		TestForSource(level, resetQueue, sourceQueue, coordinates.BlockWest(), currentLight);
		TestForSource(level, resetQueue, sourceQueue, coordinates.BlockEast(), currentLight);
		TestForSource(level, resetQueue, sourceQueue, coordinates.BlockNorth(), currentLight);
		TestForSource(level, resetQueue, sourceQueue, coordinates.BlockSouth(), currentLight);
	}

	private static void TestForSource(Level level, Queue<BlockCoordinates> resetQueue, Queue<BlockCoordinates> sourceQueue, BlockCoordinates coordinates, int currentLight, bool down = false)
	{
		int light = level.GetSkyLight(coordinates);
		if (light == 0) return;

		if (light > currentLight || (light == 15 && !down))
		{
			if (!sourceQueue.Contains(coordinates)) sourceQueue.Enqueue(coordinates);
			return;
		}

		if (!resetQueue.Contains(coordinates)) resetQueue.Enqueue(coordinates);
	}

	public void Calculate(IBlockAccess level, Queue<BlockCoordinates> lightBfQueue, HashSet<BlockCoordinates> lightBfSet)
	{
		try
		{
			while (lightBfQueue.Count > 0)
			{
				BlockCoordinates coordinates = lightBfQueue.Dequeue();
				lightBfSet.Remove(coordinates);
				if (coordinates.Y < 0 || coordinates.Y > 255)
				{
					Log.Warn($"Y coord out of bounce {coordinates.Y}");
					continue;
				}

				ChunkColumn chunk = level.GetChunk(coordinates);
				if (chunk == null)
				{
					Log.Warn("Chunk was null");
					continue;
				}

				var newChunkCoordinates = (ChunkCoordinates) coordinates;
				if (chunk.X != newChunkCoordinates.X || chunk.Z != newChunkCoordinates.Z)
				{
					chunk = level.GetChunk(newChunkCoordinates);
					if (chunk == null)
					{
						Log.Warn("Chunk with new coords was null");
						continue;
					}
				}

				ProcessNode(level, chunk, coordinates, lightBfQueue, lightBfSet);
			}
		}
		catch (Exception e)
		{
			Log.Error("Calculation", e);
		}
	}

	private void ProcessNode(IBlockAccess level, ChunkColumn chunk, BlockCoordinates coordinates, Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet)
	{
		byte currentSkyLight = GetSkyLight(coordinates, chunk);

		int sectionIdx = coordinates.Y >> 4;
		SubChunk subChunk = chunk.GetSubChunk(coordinates.Y);

		byte maxSkyLight = currentSkyLight;
		if (coordinates.Y < 255)
		{
			BlockCoordinates up = coordinates.BlockUp();
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, subChunk, sectionIdx, lightBfsQueue, lightBfSet, up, currentSkyLight, up: true));
		}

		if (coordinates.Y > 0)
		{
			BlockCoordinates down = coordinates.BlockDown();
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, subChunk, sectionIdx, lightBfsQueue, lightBfSet, down, currentSkyLight, true));
		}

		BlockCoordinates west = coordinates.BlockWest();
		maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, subChunk, sectionIdx, lightBfsQueue, lightBfSet, west, currentSkyLight));


		BlockCoordinates east = coordinates.BlockEast();
		maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, subChunk, sectionIdx, lightBfsQueue, lightBfSet, east, currentSkyLight));


		BlockCoordinates south = coordinates.BlockSouth();
		maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, subChunk, sectionIdx, lightBfsQueue, lightBfSet, south, currentSkyLight));

		BlockCoordinates north = coordinates.BlockNorth();
		maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, subChunk, sectionIdx, lightBfsQueue, lightBfSet, north, currentSkyLight));

		if (!IsTransparent(coordinates, subChunk) || currentSkyLight == 15) return;
		int diffuseLevel = GetDiffuseLevel(coordinates, subChunk);
		maxSkyLight = (byte) Math.Max(currentSkyLight, maxSkyLight - diffuseLevel);

		if (maxSkyLight <= currentSkyLight) return;
		level.SetSkyLight(coordinates, maxSkyLight);

		if (lightBfSet.Contains(coordinates)) return;
		lightBfsQueue.Enqueue(coordinates);
		lightBfSet.Add(coordinates);
	}
	
	private byte SetLightLevel(IBlockAccess level, ChunkColumn chunk, SubChunk subChunk, int sectionIdx, Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet, BlockCoordinates coordinates, byte lightLevel, bool down = false, bool up = false)
	{
		ProcessCoordinates(coordinates);

		chunk = HandleChunkCoordinates(level, chunk, coordinates, up, down);
		subChunk = HandleSectionInChunk(subChunk, chunk, coordinates, sectionIdx, up, down);

		if (chunk == null /* || chunk.chunks == null*/) return lightLevel;

		lightLevel = CheckAndSetLightAboveGround(lightBfsQueue, lightBfSet, coordinates, chunk, subChunk, lightLevel, down, up);

		lightLevel = HandleTransparentBlocks(lightBfsQueue, lightBfSet, coordinates, chunk, subChunk, lightLevel, down);

		return lightLevel;
	}

	private void ProcessCoordinates(BlockCoordinates coordinates)
	{
		if (TrackResults) MakeVisit(coordinates);
	}

	private static ChunkColumn HandleChunkCoordinates(IBlockAccess level, ChunkColumn chunk, BlockCoordinates coordinates, bool up, bool down)
	{
		if (!(up || down) && (chunk.X != coordinates.X >> 4 || chunk.Z != coordinates.Z >> 4)) return level.GetChunk((ChunkCoordinates) coordinates);
		return chunk;
	}

	private static SubChunk HandleSectionInChunk(SubChunk subChunk, ChunkColumn chunk, BlockCoordinates coordinates, int sectionIdx, bool up, bool down)
	{
		if ((up || down) && coordinates.Y >> 4 != sectionIdx) return null;
		return subChunk ?? chunk.GetSubChunk(coordinates.Y);
	}

	private static byte CheckAndSetLightAboveGround(Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet, BlockCoordinates coordinates, ChunkColumn chunk, SubChunk subChunk, byte lightLevel, bool down, bool up)
	{
		if (down || up || coordinates.Y < GetHeight(coordinates, chunk)) return lightLevel;
		if (GetSkyLight(coordinates, subChunk) == 15) return 15;
		SetSkyLight(coordinates, 15, chunk);
		EnqueueCoordinates(lightBfsQueue, lightBfSet, coordinates);
		return 15;
	}

	private static byte HandleTransparentBlocks(Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet, BlockCoordinates coordinates, ChunkColumn chunk, SubChunk subChunk, byte lightLevel, bool down)
	{
		bool isTransparent = IsTransparent(coordinates, subChunk);
		byte skyLight = GetSkyLight(coordinates, subChunk);

		if (!down || !isTransparent || lightLevel != 15 || !IsNotBlockingSkylight(coordinates, chunk)) return !isTransparent ? skyLight : SetNewLightLevelAndEnqueueCoordinates(lightBfsQueue, lightBfSet, coordinates, chunk, subChunk, skyLight, lightLevel);
		if (skyLight != 15) SetSkyLight(coordinates, 15, chunk);
		EnqueueCoordinates(lightBfsQueue, lightBfSet, coordinates);
		return 15;

	}

	private static byte SetNewLightLevelAndEnqueueCoordinates(Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet, BlockCoordinates coordinates, ChunkColumn chunk, SubChunk subChunk, byte skyLight, byte lightLevel)
	{
		int diffuseLevel = GetDiffuseLevel(coordinates, subChunk);
		if (skyLight + 1 + diffuseLevel > lightLevel) return skyLight;
		byte newLevel = (byte) (lightLevel - diffuseLevel);
		SetSkyLight(coordinates, newLevel, chunk);
		EnqueueCoordinates(lightBfsQueue, lightBfSet, coordinates);
		return newLevel;
	}

	private static void EnqueueCoordinates(Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet, BlockCoordinates coordinates)
	{
		if (lightBfSet.Contains(coordinates)) return;
		lightBfsQueue.Enqueue(coordinates);
		lightBfSet.Add(coordinates);
	}

	public static void SetSkyLight(BlockCoordinates coordinates, byte skyLight, ChunkColumn chunk) => 
		chunk?.SetSkyLight(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f, skyLight);
	

	public static bool IsNotBlockingSkylight(BlockCoordinates blockCoordinates, ChunkColumn chunk)
	{
		if (chunk == null) return true;
		try
		{
			int bid = chunk.GetBlockId(blockCoordinates.X & 0x0F, blockCoordinates.Y, blockCoordinates.Z & 0x0F);
			if (bid == 0 || (BlockFactory.TransparentBlocks?.Count > bid && BlockFactory.TransparentBlocks![bid] == 1 && bid != 18 && bid != 161 && bid != 30 && bid != 8 && bid != 9)) 
				return true;
		}
		catch (Exception ex)
		{
			Log.Error($"Error in IsNotBlockingSkylight: {ex.Message}", ex);
		}
		return false;
	}

	public static int GetDiffuseLevel(BlockCoordinates blockCoordinates, SubChunk section)
	{
		//TODO: Figure out if this is really correct. Perhaps should be zero.
		if (section == null) return 15;

		int bx = blockCoordinates.X & 0x0f;
		int by = blockCoordinates.Y;
		int bz = blockCoordinates.Z & 0x0f;

		int bid = section.GetBlockId(bx, by - (16 * (by >> 4)), bz);
		return bid switch
		{
			8 or 9 => 3,
			18 => 2,
			161 => 2,
			30 => 2,
			_ => 1
		};
	}

	public static bool IsTransparent(BlockCoordinates blockCoordinates, SubChunk section)
	{
		const int Bitmask = 0x0F;
		const int SectionSize = 16;
		if (section == null) return true;
		try
		{
			int bx = blockCoordinates.X & Bitmask;
			int by = blockCoordinates.Y;
			int bz = blockCoordinates.Z & Bitmask;

			int calculatedBy = by - (SectionSize * (by >> 4));

			int bid = section.GetBlockId(bx, calculatedBy, bz);
			if (bid == 0 || (BlockFactory.TransparentBlocks?.Count > bid && BlockFactory.TransparentBlocks[bid] == 1))
				return true;
		}
		catch (Exception ex)
		{
			Log.Error($"Error in IsTransparent: {ex.Message}", ex);
		}
		return false;
	}


	public static byte GetSkyLight(BlockCoordinates blockCoordinates, SubChunk chunk)
	{
		if (chunk == null) return 15;

		int bx = blockCoordinates.X & 0x0f;
		int by = blockCoordinates.Y;
		int bz = blockCoordinates.Z & 0x0f;

		return chunk.GetSkylight(bx, by - (16 * (by >> 4)), bz);
	}

	public static byte GetSkyLight(BlockCoordinates blockCoordinates, ChunkColumn chunk) => 
		chunk?.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y, blockCoordinates.Z & 0x0f) ?? 15;
	

	public static int GetHeight(BlockCoordinates blockCoordinates, ChunkColumn chunk) => 
		chunk?.GetHeight(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f) ?? 256;
	

	private void MakeVisit(BlockCoordinates inc)
	{
		var coordinates = new BlockCoordinates(inc.X, 0, inc.Z);
		if (Visits.TryGetValue(coordinates, out int value)) Visits[coordinates] = value + 1;
		else Visits.TryAdd(coordinates, 1);
	}

	public static void CheckIfSpawnIsMiddle(IOrderedEnumerable<ChunkColumn> chunks, Vector3 spawnPoint)
	{
		int xMin = chunks.OrderBy(kvp => kvp.X).First().X;
		int xMax = chunks.OrderByDescending(kvp => kvp.X).First().X;
		int xd = Math.Abs(xMax - xMin);

		int zMin = chunks.OrderBy(kvp => kvp.Z).First().Z;
		int zMax = chunks.OrderByDescending(kvp => kvp.Z).First().Z;
		int zd = Math.Abs(zMax - zMin);

		int xm = (int) ((xd / 2f) + xMin);
		int zm = (int) ((zd / 2f) + zMin);

		if (xm != (int) spawnPoint.X >> 4) Log.Warn($"Wrong spawn X={xm}, {(int) spawnPoint.X >> 4}");
		if (zm != (int) spawnPoint.Z >> 4) Log.Warn($"Wrong spawn Z={zm}, {(int) spawnPoint.Z >> 4}");

		if (zm == (int) spawnPoint.Z >> 4 && xm == (int) spawnPoint.X >> 4) Log.Warn($"Spawn correct {xm}, {zm} and {(int) spawnPoint.X >> 4}, {(int) spawnPoint.Z >> 4}");
	}

	private int GetMidX(ChunkColumn[] chunks)
	{
		if (!TrackResults) return 0;

		ChunkColumn[] visits = chunks.ToArray();

		int xMin = visits.OrderBy(kvp => kvp.X).First().X;
		int xMax = visits.OrderByDescending(kvp => kvp.X).First().X;
		int xd = Math.Abs(xMax - xMin);

		return xMin + (xd / 2);
	}

	private int GetWidth(ChunkColumn[] chunks)
	{
		if (!TrackResults) return 0;

		ChunkColumn[] visits = chunks.ToArray();

		int xMin = visits.OrderBy(kvp => kvp.X).First().X;
		int xMax = visits.OrderByDescending(kvp => kvp.X).First().X;
		int xd = Math.Abs(xMax - xMin);

		return xd;
	}

	private int GetWidth()
	{
		if (!TrackResults) return 0;

		KeyValuePair<BlockCoordinates, int>[] visits = Visits.ToArray();

		int xMin = visits.MinBy(kvp => kvp.Key.X).Key.X;
		int xMax = visits.MaxBy(kvp => kvp.Key.X).Key.X;
		int xd = Math.Abs(xMax - xMin);

		return xd + 1;
	}

	private int GetHeight()
	{
		if (!TrackResults) return 0;

		KeyValuePair<BlockCoordinates, int>[] visits = Visits.ToArray();

		int zMin = visits.MinBy(kvp => kvp.Key.Z).Key.Z;
		int zMax = visits.MaxBy(kvp => kvp.Key.Z).Key.Z;
		int zd = Math.Abs(zMax - zMin);

		return zd + 1;
	}


	private static Color CreateHeatColor(double value, double max)
	{
		double pct = value / max;
		if (pct < 0) pct = 0;
		return Color.FromArgb(255, (byte) (255.0f * pct), (byte) (255.0f * (1 - pct)), 0);
	}

	private static bool IsOnChunkBorder(int x, int z)
	{
		return !(x is > 0 and < 15 && z is > 0 and < 15);
	}

	private static int GetHighestSurrounding(int x, int z, ChunkColumn chunk, IBlockAccess level)
	{
		int h = chunk.GetHeight(x, z);
		if (h == 255) return h;

		if (x == 0 || x == 15 || z == 0 || z == 15)
		{
			var coords = new BlockCoordinates(x + (chunk.X * 16), h, z + (chunk.Z * 16));
			h = Math.Max(h, level.GetHeight(coords.BlockWest()));
			h = Math.Max(h, level.GetHeight(coords.BlockEast()));
			h = Math.Max(h, level.GetHeight(coords.BlockNorth()));
			h = Math.Max(h, level.GetHeight(coords.BlockSouth()));
			if (h > 255) h = 255;
			if (h < 0) h = 0;
			return h;
		}

		h = Math.Max(h, chunk.GetHeight(x, z + 1));
		h = Math.Max(h, chunk.GetHeight(x, z - 1));
		h = Math.Max(h, chunk.GetHeight(x + 1, z));
		h = Math.Max(h, chunk.GetHeight(x - 1, z));

		return h;
	}

	public void ShowHeights(ChunkColumn chunk)
	{
		if (chunk == null) return;

		for (int x = 0; x < 16; x++)
		for (int z = 0; z < 16; z++)
		{
			short y = chunk.GetHeight(x, z);
			chunk.SetBlock(x, y, z, new GoldBlock());
		}
	}
}