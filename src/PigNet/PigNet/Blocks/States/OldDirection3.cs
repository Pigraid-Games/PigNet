using System;

namespace PigNet.Blocks.States;

	public class OldDirection3 : OldDirection
	{
		/// <summary>0</summary>
		private const int EastValue = 0;
		/// <summary>1</summary>
		private const int SouthValue = 1;
		/// <summary>2</summary>
		private const int WestValue = 2;
		/// <summary>3</summary>
		private const int NorthValue = 3;

		internal OldDirection3() { }

		private OldDirection3(int value)
		{
			Value = value;
		}

		/// <summary>
		/// Value = <inheritdoc cref="SouthValue"/>
		/// </summary>
		public static readonly OldDirection3 South = new OldDirection3(SouthValue);

		/// <summary>
		/// Value = <inheritdoc cref="WestValue"/>
		/// </summary>
		public static readonly OldDirection3 West = new OldDirection3(WestValue);

		/// <summary>
		/// Value = <inheritdoc cref="NorthValue"/>
		/// </summary>
		public static readonly OldDirection3 North = new OldDirection3(NorthValue);

		/// <summary>
		/// Value = <inheritdoc cref="EastValue"/>
		/// </summary>
		public static readonly OldDirection3 East = new OldDirection3(EastValue);

		public static implicit operator OldDirection3(PigNet.Utils.Direction direction)
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

		public static implicit operator OldDirection3(PigNet.BlockFace face)
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

		public static implicit operator PigNet.BlockFace(OldDirection3 direction)
		{
			return direction.Value switch
			{
				SouthValue => PigNet.BlockFace.South,
				WestValue => PigNet.BlockFace.West,
				NorthValue => PigNet.BlockFace.North,
				EastValue => PigNet.BlockFace.East,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
			};
		}

		public static implicit operator PigNet.Utils.Direction(OldDirection3 direction)
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
	}