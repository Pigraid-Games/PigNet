using System.Drawing;
using fNbt;

namespace MiNET.BlockEntities
{
	public class SignBlockEntity : BlockEntity
	{
		public SignText FrontText { get; set; } = new();
		public SignText BackText { get; set; } = new();
		public bool IsWaxed { get; set; }
		public long LockedForEditingBy { get; set; }

		public SignBlockEntity() : base("Sign")
		{
		}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
				new NbtCompound("FrontText")
				{
					new NbtString("Text", FrontText.Text ?? string.Empty),
					new NbtString("TextOwner", FrontText.TextOwner ?? string.Empty),
					new NbtInt("SignTextColor", FrontText.SignTextColor.ToArgb()),
					new NbtByte("HideGlowOutline", (byte)(FrontText.HideGlowOutline ? 1 : 0)),
					new NbtByte("IgnoreLighting", (byte)(FrontText.IgnoreLighting ? 1 : 0)),
					new NbtByte("PersistFormatting", (byte)(FrontText.PersistFormatting ? 1 : 0))
				},
				new NbtCompound("BackText")
				{
					new NbtString("Text", BackText.Text ?? string.Empty),
					new NbtString("TextOwner", BackText.TextOwner ?? string.Empty),
					new NbtInt("SignTextColor", BackText.SignTextColor.ToArgb()),
					new NbtByte("HideGlowOutline", (byte)(BackText.HideGlowOutline ? 1 : 0)),
					new NbtByte("IgnoreLighting", (byte)(BackText.IgnoreLighting ? 1 : 0)),
					new NbtByte("PersistFormatting", (byte)(BackText.PersistFormatting ? 1 : 0))
				},
				new NbtByte("IsWaxed", (byte)(IsWaxed ? 1 : 0)),
				new NbtLong("LockedForEditingBy", LockedForEditingBy)
			};

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			if (compound.TryGet("FrontText", out NbtCompound frontTextCompound))
			{
				FrontText = ParseSignText(frontTextCompound);
			}

			if (compound.TryGet("BackText", out NbtCompound backTextCompound))
			{
				BackText = ParseSignText(backTextCompound);
			}

			IsWaxed = compound.TryGet("IsWaxed", out NbtByte isWaxed) && isWaxed.ByteValue == 1;
			LockedForEditingBy = compound.TryGet("LockedForEditingBy", out NbtLong locked) ? locked.LongValue : 0;
		}

		private SignText ParseSignText(NbtCompound compound)
		{
			var signText = new SignText
			{
				Text = compound.TryGet("Text", out NbtString text) ? text.StringValue : string.Empty,
				TextOwner = compound.TryGet("TextOwner", out NbtString owner) ? owner.StringValue : string.Empty,
				SignTextColor = compound.TryGet("SignTextColor", out NbtInt color) ? Color.FromArgb(color.IntValue) : SignColor.Black,
				HideGlowOutline = compound.TryGet("HideGlowOutline", out NbtByte hideGlow) && hideGlow.ByteValue == 1,
				IgnoreLighting = compound.TryGet("IgnoreLighting", out NbtByte ignoreLight) && ignoreLight.ByteValue == 1,
				PersistFormatting = compound.TryGet("PersistFormatting", out NbtByte persist) && persist.ByteValue == 1
			};

			return signText;
		}
	}

	public class SignText
	{
		public bool HideGlowOutline { get; set; }
		public bool IgnoreLighting { get; set; }
		public bool PersistFormatting { get; set; } = true;
		public Color SignTextColor { get; set; } = SignColor.Black;
		public string Text { get; set; } = string.Empty;
		public string TextOwner { get; set; } = string.Empty;
	}

	public static class SignColor
	{
		public static Color Black { get; } = Color.FromArgb(0, 0, 0);
		public static Color White { get; } = Color.FromArgb(240, 240, 240);
		public static Color Orange { get; } = Color.FromArgb(249, 128, 29);
		public static Color Magenta { get; } = Color.FromArgb(199, 78, 189);
		public static Color LightBlue { get; } = Color.FromArgb(58, 179, 218);
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
	}
}
