using System;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class TallGrass
{
	public TallGrass()
	{
		BlastResistance = 3;
		Hardness = 0.6f;

		IsSolid = false;
		IsReplaceable = true;
		IsTransparent = true;
	}

	public override void OnTick(Level level, bool isRandom)
	{
		base.OnTick(level, isRandom);

		if (isRandom)
		{
		}
	}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		if (Coordinates.BlockDown() != blockCoordinates) return;
		level.SetAir(Coordinates);
		UpdateBlocks(level);
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		// 50% chance to drop seeds.
		var rnd = new Random();
		if (rnd.NextDouble() > 0.5) return [new ItemWheatSeeds()];

		return [];
	}
}

public partial class TallGrass
{
	public TallGrass()
	{
		BlastResistance = 3;
		Hardness = 0.6f;

		IsSolid = false;
		IsReplaceable = true;
		IsTransparent = true;
	}

	public override void OnTick(Level level, bool isRandom)
	{
		base.OnTick(level, isRandom);

		if (isRandom)
		{
		}
	}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		if (Coordinates.BlockDown() != blockCoordinates) return;
		level.SetAir(Coordinates);
		UpdateBlocks(level);
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		// 50% chance to drop seeds.
		var rnd = new Random();
		if (rnd.NextDouble() > 0.5) return [new ItemWheatSeeds()];

		return [];
	}
}