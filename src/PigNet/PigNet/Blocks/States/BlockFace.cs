using System;

namespace PigNet.Blocks.States;

public partial class BlockFace
{
	public static implicit operator BlockFace(PigNet.BlockFace face)
	{
		return face switch
		{
			PigNet.BlockFace.Down => Down,
			PigNet.BlockFace.Up => Up,
			PigNet.BlockFace.North => North,
			PigNet.BlockFace.South => South,
			PigNet.BlockFace.West => West,
			PigNet.BlockFace.East => East,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}

	public static implicit operator PigNet.BlockFace(BlockFace face)
	{
		return face.Value switch
		{
			DownValue => PigNet.BlockFace.Down,
			UpValue => PigNet.BlockFace.Up,
			NorthValue => PigNet.BlockFace.North,
			SouthValue => PigNet.BlockFace.South,
			WestValue => PigNet.BlockFace.West,
			EastValue => PigNet.BlockFace.East,
			_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
		};
	}
}