
using System;

namespace PigNet.Blocks.States;

public partial class CoralDirection
{
	internal CoralDirection() { }

	private CoralDirection(int value)
	{
		Value = value;
	}

	/// <summary>
	/// Value = 0
	/// </summary>
	public static readonly CoralDirection West = new(0);

	/// <summary>
	/// Value = 1
	/// </summary>
	public static readonly CoralDirection East = new(1);

	/// <summary>
	/// Value = 2
	/// </summary>
	public static readonly CoralDirection North = new CoralDirection(2);
		
	/// <summary>
	/// Value = 3
	/// </summary>
	public static readonly CoralDirection South = new CoralDirection(3);

	public static implicit operator CoralDirection(Utils.Direction direction)
	{
		return direction switch
		{
			Utils.Direction.South => South,
			Utils.Direction.West => West,
			Utils.Direction.North => North,
			Utils.Direction.East => East,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}

	public static implicit operator CoralDirection(PigNet.BlockFace face)
	{
		return face switch
		{
			PigNet.BlockFace.South => South,
			PigNet.BlockFace.West => West,
			PigNet.BlockFace.North => North,
			PigNet.BlockFace.East => East,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}
}