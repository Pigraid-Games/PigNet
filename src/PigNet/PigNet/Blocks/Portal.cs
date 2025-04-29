using System.Collections.Generic;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public class PortalInfo
{
	public BlockCoordinates Coordinates { get; set; }
	public bool HasPlatform { get; set; }
	public BoundingBox Size { get; set; }
}

public partial class Portal : Block
{
	public Portal()
	{
		IsTransparent = true;
		IsSolid = false;
		LightLevel = 11;
		Hardness = 60000;
	}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		bool shouldKeep = true;
		shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockUp()));
		shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockDown()));

		//if (Metadata < 2)
		if (PortalAxis == PortalAxis.X)
		{
			shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockWest()));
			shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockEast()));
		}
		else
		{
			shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockSouth()));
			shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockNorth()));
		}

		if (!shouldKeep) Fill(level, Coordinates);
	}

	private bool IsValid(Block block)
	{
		return block is Obsidian or Portal;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [];
	}


	public void Fill(Level level, BlockCoordinates origin)
	{
		var visits = new Queue<BlockCoordinates>();

		visits.Enqueue(origin); // Kick it off with some good stuff

		while (visits.Count > 0)
		{
			BlockCoordinates coordinates = visits.Dequeue();

			if (!(level.GetBlock(coordinates) is Portal)) continue;

			level.SetAir(coordinates);

			//if (Metadata == 0)
			if (PortalAxis == PortalAxis.X)
			{
				visits.Enqueue(coordinates + Level.East);
				visits.Enqueue(coordinates + Level.West);
			}
			else
			{
				visits.Enqueue(coordinates + Level.North);
				visits.Enqueue(coordinates + Level.South);
			}

			visits.Enqueue(coordinates + Level.Down);
			visits.Enqueue(coordinates + Level.Up);
		}
	}
}