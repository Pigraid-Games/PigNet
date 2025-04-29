using System;

namespace PigNet.Blocks.States;

public partial class TorchFacingDirection
{
	public static implicit operator TorchFacingDirection(PigNet.Utils.Direction direction)
	{
		return direction switch
		{
			PigNet.Utils.Direction.North => North,
			PigNet.Utils.Direction.South => South,
			PigNet.Utils.Direction.West => West,
			PigNet.Utils.Direction.East => East,
			_ => Top
		};
	}

	public static implicit operator PigNet.Utils.Direction(TorchFacingDirection direction)
	{
		return direction.Value switch
		{
			NorthValue => PigNet.Utils.Direction.North,
			SouthValue => PigNet.Utils.Direction.South,
			WestValue => PigNet.Utils.Direction.West,
			EastValue => PigNet.Utils.Direction.East,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}

	public static implicit operator TorchFacingDirection(PigNet.BlockFace face)
	{
		return face switch
		{
			PigNet.BlockFace.North => North,
			PigNet.BlockFace.South => South,
			PigNet.BlockFace.West => West,
			PigNet.BlockFace.East => East,
			_ => Top
		};
	}

	public static implicit operator PigNet.BlockFace(TorchFacingDirection direction)
	{
		return direction.Value switch
		{
			TopValue => PigNet.BlockFace.Up,
			NorthValue => PigNet.BlockFace.North,
			SouthValue => PigNet.BlockFace.South,
			WestValue => PigNet.BlockFace.West,
			EastValue => PigNet.BlockFace.East,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}
}