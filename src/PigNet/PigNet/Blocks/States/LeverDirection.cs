namespace PigNet.Blocks.States;

using System;
using Direction = PigNet.Utils.Direction;

public partial class LeverDirection
{
	public static implicit operator LeverDirection(PigNet.BlockFace face)
	{
		return face switch
		{
			PigNet.BlockFace.Up => UpNorthSouth,
			PigNet.BlockFace.Down => DownNorthSouth,
			PigNet.BlockFace.South => South,
			PigNet.BlockFace.West => West,
			PigNet.BlockFace.North => North,
			PigNet.BlockFace.East => East,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}

	public static LeverDirection FromFaceAndDirections(PigNet.BlockFace face, Direction direction)
	{
		return (face, direction) switch
		{
			(PigNet.BlockFace.Down, Direction.North or Direction.South) => DownNorthSouth,
			(PigNet.BlockFace.Down, Direction.East or Direction.West) => DownEastWest,

			(PigNet.BlockFace.Up, Direction.North or Direction.South) => UpNorthSouth,
			(PigNet.BlockFace.Up, Direction.East or Direction.West) => UpEastWest,

			_ => face
		};
	}
}