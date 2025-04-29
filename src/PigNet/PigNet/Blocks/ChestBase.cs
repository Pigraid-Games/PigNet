using System.Numerics;
using log4net;
using PigNet.BlockEntities;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class ChestBase : Block
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ChestBase));

	public abstract CardinalDirection CardinalDirection { get; set; }

	public ChestBase()
	{
		FuelEfficiency = 15;
		IsTransparent = true;
		BlastResistance = 12.5f;
		Hardness = 2.5f;
	}


	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		CardinalDirection = player.KnownPosition.GetDirection();

		var blockEntity = new ChestBlockEntity
		{
			Coordinates = Coordinates
		};

		foreach (var coords in Coordinates.Get2dAroundCoordinates())
		{
			Block pairBlock = world.GetBlock(coords);

			if (pairBlock is not ChestBase chest
				|| pairBlock.Id != Id
				|| CardinalDirection != chest.CardinalDirection) continue;
			BlockEntity pairBlockEntity = world.GetBlockEntity(coords);

			if (pairBlockEntity is not ChestBlockEntity pairChestBlockEntity
				|| !pairChestBlockEntity.Pair(world, blockEntity)) continue;
			world.SetBlockEntity(blockEntity);
			world.SetBlockEntity(pairChestBlockEntity);

			return false;
		}

		world.SetBlockEntity(blockEntity);

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		Log.Debug($"Opening chest inventory at {blockCoordinates}");
		player.OpenInventory(blockCoordinates);
		return true;
	}
}