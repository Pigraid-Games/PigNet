using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class LogBase : Block
{
	public abstract PillarAxis PillarAxis { get; set; }

	protected LogBase()
	{
		FuelEfficiency = 15;
		BlastResistance = 10;
		Hardness = 2;
		IsFlammable = true;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		PillarAxis = (PillarAxis) face;

		return false;
	}
}