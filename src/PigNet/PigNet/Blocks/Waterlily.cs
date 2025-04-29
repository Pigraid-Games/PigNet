using System.Numerics;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Waterlily : Block
{
	public Waterlily()
	{
		IsTransparent = true;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return world.GetBlock(targetCoordinates) is Water;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Coordinates = GetNewCoordinatesFromFace(targetCoordinates, face);
		return base.PlaceBlock(world, player, targetCoordinates, face, faceCoords);
	}
}