using System.Numerics;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Loom : Block
{
	public Loom()
	{
		IsTransparent = true;
		BlastResistance = 6000;
		Hardness = 5;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Direction = player.KnownPosition.GetDirection().Opposite();

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		new Inventory(Coordinates, WindowType.Loom).Open(player);

		return true;
	}
}