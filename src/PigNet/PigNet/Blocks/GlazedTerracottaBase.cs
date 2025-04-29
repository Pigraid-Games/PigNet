using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class GlazedTerracottaBase : Block
{
	public abstract OldFacingDirection4 FacingDirection { get; set; }

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		FacingDirection = player.KnownPosition.GetDirection();

		return false;
	}
}