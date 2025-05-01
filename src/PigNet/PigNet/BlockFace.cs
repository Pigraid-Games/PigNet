using System;
using PigNet.Utils;

namespace PigNet;

public enum BlockFace
{
	Down = 0,
	Up = 1,
	North = 2, 
	South = 3,
	West = 4,
	East = 5,
	None = 255
}

public enum BlockAxis
{
	X,
	Y,
	Z
}

public static class BlockFaceExtensions
{
	public static BlockFace Opposite(this BlockFace face)
	{
		return face switch
		{
			BlockFace.Down => BlockFace.Up,
			BlockFace.Up => BlockFace.Down,
			BlockFace.South => BlockFace.North,
			BlockFace.West => BlockFace.East,
			BlockFace.North => BlockFace.South,
			BlockFace.East => BlockFace.West,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}

	public static Direction ToDirection(this BlockFace face)
	{
		return face switch
		{
			BlockFace.South => Direction.South,
			BlockFace.West => Direction.West,
			BlockFace.North => Direction.North,
			BlockFace.East => Direction.East,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}
}