using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class GrassBlock
{
	public GrassBlock()
	{
		BlastResistance = 3;
		Hardness = 0.6f;
	}

	public override void OnTick(Level level, bool isRandom)
	{
		if (!isRandom) return;

		Block upBlock = level.GetBlock(Coordinates.BlockUp());
		if (!upBlock.IsTransparent)
		{
			var dirt = new Dirt
			{
				Coordinates = Coordinates
			};
			level.SetBlock(dirt, true, false, false);
		}
		else
		{
			int lightLevel = level.GetSubtractedLight(Coordinates.BlockUp());
			if (lightLevel < 9) return;
			var random = new Random();
			for (int i = 0; i < 4; i++)
			{
				BlockCoordinates coordinates = Coordinates + new BlockCoordinates(random.Next(3) - 1, random.Next(5) - 3, random.Next(3) - 1);
				if (level.GetBlock(coordinates) is not Dirt) continue;
				Block nextUp = level.GetBlock(coordinates.BlockUp());
				if (nextUp.IsTransparent) level.SetBlock(new GrassBlock { Coordinates = coordinates });
			}
		}
	}

	public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		Item itemInHand = player.Inventory.GetItemInHand();
		if (itemInHand is ItemBoneMeal)
		{
			// If bone meal is used on a grass block, 0–8(double) tall grass, 8–24 grass and 0–8 flowers form on the
			// targeted block and on randomly-selected adjacent grass blocks up to 7 blocks away (taxicab distance).
			// The flowers that appear are dependent on the biome, meaning that in order to obtain specific flowers,
			// the player must travel to biomes where the flowers are found naturally. See Flower § Flower biomes
			// for more information.
			//TODO: Grow grass and flowers randomly

			int grassPlanted = 0;
			int flowersPlanted = 0;

			var rnd = new Random();
			for (int i = 0; i < 128; i++)
			{
				BlockCoordinates coord = blockCoordinates;
				bool shouldContinue = false;
				for (int j = 0; j < i / 16; j++)
				{
					coord += new BlockCoordinates(rnd.Next(3) - 1, (rnd.Next(3) - 1) * (rnd.Next(3) / 2), rnd.Next(3) - 1);
					if (!level.GetBlock(coord).IsSolid)
					{
						shouldContinue = true;
						break;
					}
				}
				if (shouldContinue) continue;

				if (!(level.GetBlock(coord) is GrassBlock)) continue;
				coord += BlockCoordinates.Up;
				Block growthBlock = level.GetBlock(coord);

				if (growthBlock is ShortGrass)
				{
					if (grassPlanted >= 24) continue;

					if (rnd.Next(10) == 0)
					{
						level.SetBlock(new TallGrass { Coordinates = coord });
						level.SetBlock(new TallGrass
						{
							Coordinates = coord + BlockCoordinates.Up,
							UpperBlockBit = true
						});
						grassPlanted++;
					}
				}
				if (growthBlock is Fern)
				{
					if (grassPlanted >= 24) continue;

					if (rnd.Next(10) == 0)
					{
						level.SetBlock(new LargeFern { Coordinates = coord });
						level.SetBlock(new LargeFern
						{
							Coordinates = coord + BlockCoordinates.Up,
							UpperBlockBit = true
						});
						grassPlanted++;
					}
				}
				else if (growthBlock is Air)
				{
					if (rnd.Next(8) == 0)
					{
						if (flowersPlanted >= 8) continue;

						Block block = null;
						int biomeId = level.GetBiomeId(coord);
						switch (biomeId)
						{
							// [StateEnum(
							// "tulip_pink",
							// "houstonia",
							// "lily_of_the_valley",
							// "tulip_white",
							// "allium",
							// "tulip_red",
							// "poppy",
							// "cornflower",
							// "tulip_orange",
							// "ox eye",
							// "orchid")]
							case 1: // plains
							{
								if (rnd.Next(2) == 0)
									block = new Poppy();
								else
								{
									var flower = new Dandelion();
									block = flower;
								}
								break;
							}
						}
						if (block != null)
						{
							block.Coordinates = coord;
							level.SetBlock(block);
						}

						flowersPlanted++;
					}
					else
					{
						if (grassPlanted >= 24) continue;

						Block block = rnd.Next(10) != 0 ? new ShortGrass() : new Fern();

						block.Coordinates = coord;
						level.SetBlock(block);

						grassPlanted++;
					}
				}
			}

			return true;
		}

		return false;
	}

	private void DoSpawn()
	{
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [ItemFactory.GetItem<Dirt>()];
	}
}

public class RandomWeighted<T>(List<RandomRange<T>> items)
{
	private readonly Random _random = new();

	public T Next()
	{
		int targetWeight = _random.Next(items.Sum(i => i.Weight) + 1);
		int currentWeight = 0;
		foreach (RandomRange<T> range in items)
		{
			currentWeight += range.Weight;

			if (targetWeight < currentWeight) return range.Item;
		}

		return default;
	}
}

public class RandomRange<T>(T item, int weight)
{
	public T Item { get; } = item;
	public int Weight { get; } = weight;
}