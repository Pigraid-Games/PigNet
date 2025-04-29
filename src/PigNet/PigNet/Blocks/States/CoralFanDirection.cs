using System;

namespace PigNet.Blocks.States;

public partial class CoralFanDirection
{
	internal CoralFanDirection() { }

	private CoralFanDirection(int value)
	{
		Value = value;
	}

	/// <summary>
	/// Value = 0
	/// </summary>
	public static readonly CoralFanDirection EastWest = new(0);

	/// <summary>
	/// Value = 1
	/// </summary>
	public static readonly CoralFanDirection NorthSouth = new(1);

	public static implicit operator CoralFanDirection(PigNet.Utils.Direction direction)
	{
		return direction switch
		{
			PigNet.Utils.Direction.South => NorthSouth,
			PigNet.Utils.Direction.West => EastWest,
			PigNet.Utils.Direction.North => NorthSouth,
			PigNet.Utils.Direction.East => EastWest,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}

	public static implicit operator CoralFanDirection(PigNet.BlockFace face)
	{
		return face switch
		{
			PigNet.BlockFace.South => NorthSouth,
			PigNet.BlockFace.West => EastWest,
			PigNet.BlockFace.North => NorthSouth,
			PigNet.BlockFace.East => EastWest,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}
}