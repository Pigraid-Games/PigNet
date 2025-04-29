using System;
using System.Drawing;
using fNbt;
using fNbt.Serialization;

namespace PigNet.BlockEntities;

	public class SignBlockEntity : BlockEntity
	{
		/// <summary>
		/// A compound which discribes back text. 
		/// </summary>
		public SignText BackText { get; set; } = new();

		/// <summary>
		/// A compound which discribes front text.
		/// </summary>
		public SignText FrontText { get; set; } = new();

		/// <summary>
		///  true if the text is locked with honeycomb.
		/// </summary>
		public bool IsWaxed { get; set; }

		public long LockedForEditingBy { get; set; }


		public SignBlockEntity() : base(BlockEntityIds.Sign)
		{
			
		}

		protected SignBlockEntity(string name) : base(name)
		{

		}
	}

	[NbtObject]
	public class SignText
	{
		/// <summary>
		/// true if the outer glow of a sign with glowing text does not show.
		/// </summary>
		public bool HideGlowOutline { get; set; }

		/// <summary>
		/// true if the sign has been dyed with a glow ink sac.
		/// </summary>
		public bool IgnoreLighting { get; set; }

		/// <summary>
		/// Unknown. Defaults to true.
		/// </summary>
		public bool PersistFormatting { get; set; } = true;

		/// <summary>
		/// The color that has been used to dye the sign. Default is Black.
		/// </summary>
		public Color SignTextColor { get; set; } = SignColor.Black;

		/// <summary>
		/// The text on it.
		/// </summary>
		public string Text { get; set; } = string.Empty;

		/// <summary>
		/// Unknown.
		/// </summary>
		public string TextOwner { get; set; } = string.Empty;
	}

	public static class SignColor
	{
		public static Color Black { get; } = Color.FromArgb(0, 0, 0);

		public static Color White { get; } = Color.FromArgb(240, 240, 240);

		public static Color Orange { get; } = Color.FromArgb(249, 128, 29);

		public static Color Magenta { get; } = Color.FromArgb(199, 78, 189);

		public static Color LightBlue  { get; } = Color.FromArgb(58, 179, 218);

		public static Color Yellow { get; } = Color.FromArgb(254, 216, 61);

		public static Color Lime { get; } = Color.FromArgb(128, 199, 31);

		public static Color Pink { get; } = Color.FromArgb(243, 139, 170);

		public static Color Gray { get; } = Color.FromArgb(71, 79, 82);

		public static Color LightGray { get; } = Color.FromArgb(157, 157, 151);

		public static Color Cyan { get; } = Color.FromArgb(22, 156, 156);

		public static Color Purple { get; } = Color.FromArgb(137, 50, 184);

		public static Color Blue { get; } = Color.FromArgb(60, 68, 170);

		public static Color Brown { get; } = Color.FromArgb(131, 84, 50);

		public static Color Green { get; } = Color.FromArgb(94, 124, 22);

		public static Color Red { get; } = Color.FromArgb(176, 46, 38);

		public static Color Parse(string value)
		{
			if (TryParse(value, out var color))
			{
				return color;
			}

			throw new ArgumentOutOfRangeException(nameof(value), value, $"Unexpected color name");
		}

		public static bool TryParse(string value, out Color color)
		{
			Color? c = value.ToLower() switch
			{
				"black" => Black,
				"white" => White,
				"orange" => Orange,
				"magenta" => Magenta,
				"light_blue" => LightBlue,
				"yellow" => Yellow,
				"lime" => Lime,
				"pink" => Pink,
				"gray" => Gray,
				"light_gray" => LightGray,
				"cyan" => Cyan,
				"purple" => Purple,
				"blue" => Blue,
				"brown" => Brown,
				"green" => Green,
				"red" => Red,
				_ => null
			};

			color = c ?? default;
			return c != null;
		}
	}