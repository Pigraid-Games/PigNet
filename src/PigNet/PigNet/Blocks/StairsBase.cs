using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class StairsBase : Block
{
	public abstract bool UpsideDownBit { get; set; }
	public abstract WeirdoDirection WeirdoDirection { get; set; }

	protected StairsBase() 
	{
		BlastResistance = 30;
		Hardness = 2;
		IsTransparent = true;
		IsBlockingSkylight = false;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		UpsideDownBit = ((faceCoords.Y > 0.5 && face != BlockFace.Up) || face == BlockFace.Down);
		WeirdoDirection = player.KnownPosition.GetDirection().Opposite();

		world.SetBlock(this);
		return true;
	}
}