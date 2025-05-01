using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class TrapdoorBase : Block
{
	public abstract OldDirection4 Direction { get; set; }
	public abstract bool OpenBit { get; set; }
	public abstract bool UpsideDownBit { get; set; }

	protected TrapdoorBase()
	{
		IsTransparent = true;
		BlastResistance = 15;
		Hardness = 5;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Direction = player.KnownPosition.GetDirection().Opposite();

		UpsideDownBit = (faceCoords.Y > 0.5 && face != BlockFace.Up) || face == BlockFace.Down;

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		OpenBit = !OpenBit;
		world.SetBlock(this);

		return true;
	}
}