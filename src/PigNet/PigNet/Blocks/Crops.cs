using System;
using System.Numerics;
using log4net;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class Crops : Block
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Crops));

	protected Crops()
	{
		IsSolid = false;
		IsTransparent = true;
	}

	public abstract int Growth { get; set; }

	protected int MaxGrowth { get; set; } = Blocks.States.Growth.MaxValue;

	public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		Item itemInHand = player.Inventory.GetItemInHand();
		if (Growth >= MaxGrowth || itemInHand is not ItemBoneMeal) return false;
		Growth += (byte) new Random().Next(2, 6);
		if (Growth > MaxGrowth) Growth = MaxGrowth;

		level.SetBlock(this);

		return true;
	}

	public override void OnTick(Level level, bool isRandom)
	{
		if (!isRandom) return;

		if (Growth >= MaxGrowth || !CalculateGrowthChance(level, this)) return;
		Growth++;
		level.SetBlock(this);
	}

	private static bool CalculateGrowthChance(Level level, Block target)
	{
		double points = 0;

		if (level.GetBlock(target.Coordinates.BlockDown()) is not Farmland under) return false;

		points += under.MoisturizedAmount == 0 ? 2 : 4;

		{
			var west = level.GetBlock(under.Coordinates.BlockWest()) as Farmland;
			var east = level.GetBlock(under.Coordinates.BlockEast()) as Farmland;
			var south = level.GetBlock(under.Coordinates.BlockNorth()) as Farmland;
			var north = level.GetBlock(under.Coordinates.BlockSouth()) as Farmland;
			var southWest = level.GetBlock(under.Coordinates.BlockSouthWest()) as Farmland;
			var southEast = level.GetBlock(under.Coordinates.BlockSouthEast()) as Farmland;
			var northWest = level.GetBlock(under.Coordinates.BlockNorthWest()) as Farmland;
			var northEast = level.GetBlock(under.Coordinates.BlockNorthEast()) as Farmland;

			// For each of the 8 blocks around the block in which the crop is planted, dry farmland gives 0.25 points, and hydrated farmland gives 0.75
			points += west != null ? west.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			points += east != null ? east.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			points += south != null ? south.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			points += north != null ? north.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			points += northWest != null ? northWest.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			points += northEast != null ? northEast.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			points += southWest != null ? southWest.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			points += southEast != null ? southEast.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
		}

		{
			var west = level.GetBlock(target.Coordinates.BlockWest()) as Crops;
			var east = level.GetBlock(target.Coordinates.BlockEast()) as Crops;
			var south = level.GetBlock(target.Coordinates.BlockNorth()) as Crops;
			var north = level.GetBlock(target.Coordinates.BlockSouth()) as Crops;
			var southWest = level.GetBlock(target.Coordinates.BlockSouthWest()) as Crops;
			var southEast = level.GetBlock(target.Coordinates.BlockSouthEast()) as Crops;
			var northWest = level.GetBlock(target.Coordinates.BlockNorthWest()) as Crops;
			var northEast = level.GetBlock(target.Coordinates.BlockNorthEast()) as Crops;

			bool cutHalf = false;
			cutHalf |= west != null && west.GetType() == target.GetType();
			cutHalf |= east != null && east.GetType() == target.GetType();
			cutHalf |= south != null && south.GetType() == target.GetType();
			cutHalf |= north != null && north.GetType() == target.GetType();
			cutHalf |= northEast != null && northEast.GetType() == target.GetType();
			cutHalf |= northWest != null && northWest.GetType() == target.GetType();
			cutHalf |= southEast != null && southEast.GetType() == target.GetType();
			cutHalf |= southWest != null && southWest.GetType() == target.GetType();
			points /= cutHalf ? 2 : 1;
		}

		double chance = 1 / (Math.Floor(25 / points) + 1);

		bool calculateGrowthChance = level.Random.NextDouble() <= chance;
		return calculateGrowthChance;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		if (!base.CanPlace(world, player, blockCoordinates, targetCoordinates, face)) return false;
		Block under = world.GetBlock(Coordinates.BlockDown());
		return under is Farmland;
	}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		if (Coordinates.BlockDown() == blockCoordinates) level.BreakBlock(null, this);
	}
}