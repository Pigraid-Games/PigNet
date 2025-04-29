using System;

namespace PigNet.Blocks.States;

public partial class CardinalDirection
{
	public static implicit operator CardinalDirection(Utils.Direction direction)
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
	public static implicit operator Utils.Direction(CardinalDirection direction)
	{
		return direction.Value switch
		{
			SouthValue => Utils.Direction.South,
			WestValue => Utils.Direction.West,
			NorthValue => Utils.Direction.North,
			EastValue => Utils.Direction.East,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}
}