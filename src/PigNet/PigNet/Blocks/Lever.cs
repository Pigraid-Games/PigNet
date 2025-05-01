using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Lever
{
	public Lever()
	{
		IsTransparent = true;
		IsSolid = false;
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		LeverDirection = LeverDirection.FromFaceAndDirections(face, player.KnownPosition.GetDirection());

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		OpenBit = !OpenBit;
		world.SetBlock(this);

		return true;
	}
}