using System.Numerics;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class CarvedPumpkin : Block
{
	public CarvedPumpkin() : base(410)
	{
		BlastResistance = 5;
		Hardness = 1;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Direction = player.GetCardinalDirection();
		return false;
	}
}