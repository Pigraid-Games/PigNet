
using System;

namespace PigNet.Blocks.States;

	public class OldFacingDirection1 : OldFacingDirection
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
		private const int EastValue = 4;
		/// <summary>5</summary>
		private const int WestValue = 5;


		internal OldFacingDirection1() { }

		private OldFacingDirection1(int value)
		{
			Value = value;
		}

		/// <summary>
		/// Value = <inheritdoc cref="DownValue"/>
		/// </summary>
		public static readonly OldFacingDirection1 Down = new OldFacingDirection1(DownValue);
		
		/// <summary>
		/// Value = <inheritdoc cref="UpValue"/>
		/// </summary>
		public static readonly OldFacingDirection1 Up = new OldFacingDirection1(UpValue);
		
		/// <summary>
		/// Value = <inheritdoc cref="NorthValue"/>
		/// </summary>
		public static readonly OldFacingDirection1 North = new OldFacingDirection1(NorthValue);
		
		/// <summary>
		/// Value = <inheritdoc cref="SouthValue"/>
		/// </summary>
		public static readonly OldFacingDirection1 South = new OldFacingDirection1(SouthValue);
		
		/// <summary>
		/// Value = <inheritdoc cref="EastValue"/>
		/// </summary>
		public static readonly OldFacingDirection1 East = new OldFacingDirection1(EastValue);
		
		/// <summary>
		/// Value = <inheritdoc cref="WestValue"/>
		/// </summary>
		public static readonly OldFacingDirection1 West = new OldFacingDirection1(WestValue);

		public static implicit operator OldFacingDirection1(PigNet.Utils.Direction direction)
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

		public static implicit operator PigNet.Utils.Direction(OldFacingDirection1 direction)
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

		public static implicit operator OldFacingDirection1(PigNet.BlockFace face)
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

		public static implicit operator PigNet.BlockFace(OldFacingDirection1 direction)
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