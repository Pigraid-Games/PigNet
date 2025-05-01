using System;
using System.Collections.Generic;
using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils;
using PigNet.Utils.Vectors;

namespace PigNet.Worlds;

public class SuperflatGenerator : IWorldGenerator
{
	public string Seed { get; set; }
	public List<Block> BlockLayers { get; set; }
	public Dimension Dimension { get; set; }

	public SuperflatGenerator(Dimension dimension)
	{
		Dimension = dimension;
		switch (dimension)
		{
			case Dimension.Overworld:
				Seed = Config.GetProperty("superflat.overworld", "3;minecraft:bedrock,2*minecraft:dirt,minecraft:grass_block;1;village");
				break;
			case Dimension.Nether:
				Seed = Config.GetProperty("superflat.nether", "3;minecraft:bedrock,2*minecraft:netherrack,3*minecraft:lava,2*minecraft:netherrack,20*minecraft:air,minecraft:bedrock;1;village");
				break;
			case Dimension.TheEnd:
				Seed = Config.GetProperty("superflat.theend", "3;40*minecraft:air,minecraft:bedrock,7*minecraft:end_stone;1;village");
				break;
		}
	}

	public void Initialize(IWorldProvider worldProvider)
	{
		BlockLayers = ParseSeed(Seed);
	}

	public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
	{
		var chunk = new ChunkColumn();
		chunk.X = chunkCoordinates.X;
		chunk.Z = chunkCoordinates.Z;

		PopulateChunk(chunk);

		var random = new Random((chunk.X * 397) ^ chunk.Z);
		if (random.NextDouble() > 0.99)
		{
			GenerateLake(random, chunk, Dimension == Dimension.Overworld ? new Water() : Dimension == Dimension.Nether ? (Block) new Lava() : new Air());
		}
		else if (random.NextDouble() > 0.97)
		{
			GenerateGlowStone(random, chunk);
		}

		return chunk;
	}

	private void GenerateGlowStone(Random random, ChunkColumn chunk)
	{
		if (Dimension != Dimension.Nether) return;

		int h = FindGroundLevel();

		if (h < 0) return;

		Vector2 center = new Vector2(7, 8);

		for (int x = 0; x < 16; x++)
		{
			for (int z = 0; z < 16; z++)
			{
				Vector2 v = new Vector2(x, z);
				if (random.Next((int) Vector2.DistanceSquared(center, v)) < 1)
				{
					chunk.SetBlock(x, BlockLayers.Count - 2, z, new Glowstone());
					if (random.NextDouble() > 0.85)
					{
						chunk.SetBlock(x, BlockLayers.Count - 3, z, new Glowstone());
						if (random.NextDouble() > 0.50)
						{
							chunk.SetBlock(x, BlockLayers.Count - 4, z, new Glowstone());
						}
					}
				}
			}
		}
	}

	private void GenerateLake(Random random, ChunkColumn chunk, Block block)
	{
		int h = FindGroundLevel();

		if (h < 0) return;

		Vector2 center = new Vector2(7, 8);

		for (int x = 0; x < 16; x++)
		{
			for (int z = 0; z < 16; z++)
			{
				Vector2 v = new Vector2(x, z);
				if (random.Next((int) Vector2.DistanceSquared(center, v)) < 4)
				{
					if (Dimension == Dimension.Overworld)
					{
						chunk.SetBlock(x, h, z, block);
					}
					else if (Dimension == Dimension.Nether)
					{
						chunk.SetBlock(x, h, z, block);

						if (random.Next(30) == 0)
						{
							for (int i = h; i < BlockLayers.Count - 1; i++)
							{
								chunk.SetBlock(x, i, z, block);
							}
						}
					}
					else if (Dimension == Dimension.TheEnd)
					{
						for (int i = 0; i < BlockLayers.Count; i++)
						{
							chunk.SetBlock(x, i, z, new Air());
						}
					}
				}
				else if (Dimension == Dimension.TheEnd && random.Next((int) Vector2.DistanceSquared(center, v)) < 15)
				{
					chunk.SetBlock(x, h, z, new Air());
				}
			}
		}
	}

	private int FindGroundLevel()
	{
		int h = 0;
		bool foundSolid = false;
		foreach (var block in BlockLayers)
		{
			if (foundSolid && block is Air) return h - 1;

			if (block.IsSolid) foundSolid = true;

			h++;
		}

		return foundSolid ? h - 1 : -1;
	}

	public void PopulateChunk(ChunkColumn chunk)
	{
		List<Block> layers = BlockLayers;

		for (int x = 0; x < 16; x++)
		{
			for (int z = 0; z < 16; z++)
			{
				int h = 0;

				foreach (Block layer in layers)
				{
					chunk.SetBlock(x, h, z, layer);
					h++;
				}

				chunk.SetHeight(x, z, (short) h);
				for (int i = h + Dimension == Dimension.Overworld ? 1 : 0; i >= 0; i--)
				{
					chunk.SetSkyLight(x, i, z, 0);
				}

				// need to take care of skylight for non overworld to make it 0.

				// TODO - 1.20 - update
				chunk.SetBiome(x, ChunkColumn.WorldMaxY, z, 1); // use pattern for this
			}
		}
	}

	public static List<Block> ParseSeed(string inputSeed)
	{
		if (string.IsNullOrEmpty(inputSeed)) return new List<Block>();

		var blocks = new List<Block>();

		var components = inputSeed.Split(';');

		var blockPattern = components[1].Split(',');
		foreach (var pattern in blockPattern)
		{
			var countAndBlock = pattern.Replace("minecraft:", "").Split('*');

			var blockAndMeta = countAndBlock[0].Split(':');
			int count = 1;
			if (countAndBlock.Length > 1)
			{
				count = int.Parse(countAndBlock[0]);
				blockAndMeta = countAndBlock[1].Split(':');
			}

			if (blockAndMeta.Length == 0) continue;

			Block block;
			if (blockAndMeta.Length > 1 && short.TryParse(blockAndMeta[1], out var meta))
			{
				//TODO: Replace with new state-based data from JE patterns.
				block = BlockFactory.GetBlockById($"minecraft:{blockAndMeta[0]}", meta);
			}
			else
			{
				block = BlockFactory.GetBlockById($"minecraft:{blockAndMeta[0]}");
			}

			if (block != null)
			{
				for (int i = 0; i < count; i++)
				{
					blocks.Add(block);
				}
			}
			else
			{
				throw new Exception($"Expected block, but didn't fine one for pattern {pattern}, {string.Join("^", blockAndMeta)} ");
			}
		}

		return blocks;
	}
}