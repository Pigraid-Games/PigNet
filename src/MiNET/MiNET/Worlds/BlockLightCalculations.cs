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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using MiNET.Blocks;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds;

public static class BlockLightCalculations
{
	public static void Calculate(Level level, BlockCoordinates blockCoordinates)
	{
		Queue<BlockCoordinates> lightBfsQueue = new();
		lightBfsQueue.Enqueue(blockCoordinates);

		while (lightBfsQueue.Count > 0) ProcessNode(level, lightBfsQueue.Dequeue(), lightBfsQueue);
	}

	private static void ProcessNode(Level level, BlockCoordinates coord, Queue<BlockCoordinates> lightBfsQueue)
	{
		ChunkColumn chunk = GetChunk(level, coord);
		if (chunk == null) return;

		int lightLevel = chunk.GetBlocklight(coord.X & 0x0f, coord.Y, coord.Z & 0x0f);

		Span<BlockCoordinates> neighbors = stackalloc BlockCoordinates[6] { coord.BlockUp(), coord.BlockDown(), coord.BlockWest(), coord.BlockEast(), coord.BlockSouth(), coord.BlockNorth() };

		foreach (BlockCoordinates neighbor in neighbors) Test(level, neighbor, lightBfsQueue, chunk, lightLevel);
	}

	private static ChunkColumn GetChunk(Level level, BlockCoordinates blockCoordinates)
	{
		return (level.WorldProvider as AnvilWorldProvider)?._chunkCache.GetValueOrDefault((ChunkCoordinates) blockCoordinates);
	}

	private static void Test(Level level, BlockCoordinates newCoord, Queue<BlockCoordinates> lightBfsQueue, ChunkColumn chunk, int lightLevel)
	{
		var newChunkCoord = (ChunkCoordinates) newCoord;
		if (chunk.X != newChunkCoord.X || chunk.Z != newChunkCoord.Z) chunk = GetChunk(level, newCoord);

		if (chunk == null) return;

		if (chunk.GetBlockId(newCoord.X & 0x0f, newCoord.Y, newCoord.Z & 0x0f) == 0)
			SetLightLevel(chunk, lightBfsQueue, newCoord, lightLevel);
		else
			SetLightLevel(chunk, lightBfsQueue, level.GetBlock(newCoord, chunk), lightLevel);
	}

	private static void SetLightLevel(ChunkColumn chunk, Queue<BlockCoordinates> lightBfsQueue, Block b1, int lightLevel)
	{
		if (b1.LightLevel > 0 && b1.LightLevel < lightLevel)
		{
			b1.BlockLight = (byte) Math.Max(b1.LightLevel, lightLevel - 1);
			chunk.SetBlocklight(b1.Coordinates.X & 0x0f, b1.Coordinates.Y, b1.Coordinates.Z & 0x0f, b1.BlockLight);
		}

		if ((!b1.IsSolid || b1.IsTransparent) && b1.BlockLight + 2 <= lightLevel)
		{
			b1.BlockLight = (byte) (lightLevel - 1);
			chunk.SetBlocklight(b1.Coordinates.X & 0x0f, b1.Coordinates.Y, b1.Coordinates.Z & 0x0f, b1.BlockLight);
			lightBfsQueue.Enqueue(b1.Coordinates);
		}
	}

	private static void SetLightLevel(ChunkColumn chunk, Queue<BlockCoordinates> lightBfsQueue, BlockCoordinates coord, int lightLevel)
	{
		if (chunk.GetBlocklight(coord.X & 0x0f, coord.Y, coord.Z & 0x0f) + 2 <= lightLevel)
		{
			chunk.SetBlocklight(coord.X & 0x0f, coord.Y, coord.Z & 0x0f, (byte) (lightLevel - 1));
			lightBfsQueue.Enqueue(coord);
		}
	}
}