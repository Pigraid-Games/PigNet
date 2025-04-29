using System;

namespace PigNet.Blocks.States;

public partial class PillarAxis
{
	public static implicit operator PillarAxis(BlockAxis axis)
	{
		return axis switch
		{
			BlockAxis.X => X,
			BlockAxis.Y => Y,
			BlockAxis.Z => Z,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
		};
	}

	public static implicit operator BlockAxis(PillarAxis axis)
	{
		return axis.Value switch
		{
			XValue => BlockAxis.X,
			YValue => BlockAxis.Y,
			ZValue => BlockAxis.Z,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
		};
	}

	public static explicit operator PillarAxis(PigNet.BlockFace face)
	{
		return face switch
		{
			PigNet.BlockFace.Down => Y,
			PigNet.BlockFace.Up => Y,
			PigNet.BlockFace.North => Z,
			PigNet.BlockFace.South => Z,
			PigNet.BlockFace.West => X,
			PigNet.BlockFace.East => X,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}

	public static explicit operator PillarAxis(BlockFace face)
	{
		return (PillarAxis) (PigNet.BlockFace) face;
	}
}