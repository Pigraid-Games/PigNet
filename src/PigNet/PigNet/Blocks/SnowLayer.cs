using System.Numerics;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class SnowLayer
{
	public SnowLayer()
	{
		IsTransparent = true;
		BlastResistance = 0.5f;
		Hardness = 0.1f;
		IsReplaceable = true;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		Block down = world.GetBlock(Coordinates.BlockDown());
		switch (down)
		{
			case Air:
			case SnowLayer { Height: < 7 }:
				return false;
			default:
				return base.CanPlace(world, player, blockCoordinates, targetCoordinates, face);
		}
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (world.GetBlock(Coordinates) is not SnowLayer current) return false;
		if (current.Height < 6) Height = current.Height + 1;
		else
		{
			var snow = new Snow
			{
				Coordinates = Coordinates
			};
			world.SetBlock(snow);
			return true;
		}

		return false;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemSnowball { Count = (byte) (Height + 1) }];
	}
}