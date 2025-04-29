using System;

namespace PigNet.Blocks.States;

	public class OldFacingDirection4 : OldFacingDirection
	{
		/// <summary>0</summary>
		private const int DownValue = 0;
		/// <summary>1</summary>
		private const int UpValue = 1;
		/// <summary>2</summary>
		private const int NorthValue = 2;
		/// <summary>3</summary>
		private const int SouthValue = 3;
		/// <summary>4</summary>
		private const int WestValue = 4;
		/// <summary>5</summary>
		private const int EastValue = 5;

		internal OldFacingDirection4() { }

		private OldFacingDirection4(int value)
		{
			Value = value;
		}

		/// <summary>
		/// Value = <inheritdoc cref="DownValue"/>
		/// </summary>
		public static readonly OldFacingDirection4 Down = new OldFacingDirection4(DownValue);

		/// <summary>
		/// Value = <inheritdoc cref="UpValue"/>
		/// </summary>
		public static readonly OldFacingDirection4 Up = new OldFacingDirection4(UpValue);

		/// <summary>
		/// Value = <inheritdoc cref="NorthValue"/>
		/// </summary>
		public static readonly OldFacingDirection4 North = new OldFacingDirection4(NorthValue);

		/// <summary>
		/// Value = <inheritdoc cref="SouthValue"/>
		/// </summary>
		public static readonly OldFacingDirection4 South = new OldFacingDirection4(SouthValue);

		/// <summary>
		/// Value = <inheritdoc cref="EastValue"/>
		/// </summary>
		public static readonly OldFacingDirection4 East = new OldFacingDirection4(EastValue);

		/// <summary>
		/// Value = <inheritdoc cref="WestValue"/>
		/// </summary>
		public static readonly OldFacingDirection4 West = new OldFacingDirection4(WestValue);

		public static implicit operator OldFacingDirection4(PigNet.Utils.Direction direction)
		{
			return direction switch
			{
				PigNet.Utils.Direction.South => South,
				PigNet.Utils.Direction.West => West,
				PigNet.Utils.Direction.North => North,
				PigNet.Utils.Direction.East => East,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
			};
		}

		public static implicit operator PigNet.Utils.Direction(OldFacingDirection4 direction)
		{
			return direction.Value switch
			{
				SouthValue => PigNet.Utils.Direction.South,
				WestValue => PigNet.Utils.Direction.West,
				NorthValue => PigNet.Utils.Direction.North,
				EastValue => PigNet.Utils.Direction.East,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
			};
		}

		public static implicit operator OldFacingDirection4(PigNet.BlockFace face)
		{
			return face switch
			{
				PigNet.BlockFace.Down => Down,
				PigNet.BlockFace.Up => Up,
				PigNet.BlockFace.South => South,
				PigNet.BlockFace.West => West,
				PigNet.BlockFace.North => North,
				PigNet.BlockFace.East => East,
				_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
			};
		}

		public static implicit operator PigNet.BlockFace(OldFacingDirection4 direction)
		{
			return direction.Value switch
			{
				DownValue => PigNet.BlockFace.Down,
				UpValue => PigNet.BlockFace.Up,
				SouthValue => PigNet.BlockFace.South,
				WestValue => PigNet.BlockFace.West,
				NorthValue => PigNet.BlockFace.North,
				EastValue => PigNet.BlockFace.East,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
			};
		}
	}