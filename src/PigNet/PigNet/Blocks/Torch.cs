using System;
using System.Linq;
using System.Numerics;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Torch : Block
{
	public Torch()
	{
		IsTransparent = true;
		IsSolid = false;
		LightLevel = 14;
	}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		var face = (BlockFace) TorchFacingDirection;
		if (face == BlockFace.Up) face = BlockFace.Down;

		if (level.GetBlock(Coordinates + face).IsTransparent) level.BreakBlock(null, this);
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return !(PlaceInternal(world, face.Opposite()) || CanPlace(world));
	}

	private bool PlaceInternal(Level world, BlockFace face)
	{
		if (face == BlockFace.Up) return false;

		if (world.GetBlock(Coordinates + face).IsTransparent) return false;

		TorchFacingDirection = face;

		return true;
	}

	private bool CanPlace(Level level)
	{
		return PlaceInternal(level, BlockFace.Down) || 
				Enum.GetValues<BlockFace>().Any(direction => PlaceInternal(level, direction));
	}
}