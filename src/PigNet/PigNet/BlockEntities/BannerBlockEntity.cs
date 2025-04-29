using System.Collections.Generic;
using fNbt.Serialization;

namespace PigNet.BlockEntities;

public class BannerBlockEntity() : BlockEntity(BlockEntityIds.Banner)
{
	/// <summary>
	///     The type of the block entity. 0 is normal banner. 1 is ominous banner.
	/// </summary>
	public int Type { get; set; }

	[NbtProperty("Base")] public int BaseColor { get; set; }

	public List<BannerPattern> Patterns { get; set; } = [];
}

[NbtObject]
public class BannerPattern
{
	public string Pattern { get; set; }
	public int Color { get; set; }
}

public static class BannerPatterns
{
	public const string Base = "b";

	public const string StripeBottom = "bs";
	public const string StripeTop = "ts";
	public const string StripeLeft = "ls";
	public const string StripeRight = "rs";
	public const string StripeCenter = "cs";
	public const string StripeMiddle = "ms";
	public const string StripeDownRight = "drs";
	public const string StripeDownLeft = "dls";

	public const string SmallStripes = "ss";

	public const string Cross = "cr";
	public const string StraightCross = "sc";

	public const string DiagonalLeft = "ld";
	public const string DiagonalRight = "rud";
	public const string DiagonalUpLeft = "lud";
	public const string DiagonalUpRight = "rd";

	public const string VerticalHalfLeft = "vh";
	public const string VerticalHalfRight = "vhr";
	public const string HorizontalHalfTop = "hh";
	public const string HorizontalHalfBottom = "hhb";

	public const string CornerBottomLeft = "bl";
	public const string CornerBottomRight = "br";
	public const string CornerTopLeft = "tl";
	public const string CornerTopRight = "tr";

	public const string TriangleBottom = "bt";
	public const string TriangleTop = "tt";

	public const string TrianglesBottom = "bts";
	public const string TrianglesTop = "tts";

	public const string Circle = "mc";
	public const string Rhombus = "mr";
	public const string Border = "bo";
	public const string CurlyBorder = "cbo";
	public const string Bricks = "bri";

	public const string GradientDown = "gra";
	public const string GradientUp = "gru";

	public const string Creeper = "cre";
	public const string Skull = "sku";
	public const string Flower = "flo";
	public const string Mojang = "moj";
	public const string Globe = "glb";
	public const string Piglin = "big";
	public const string Flow = "flw";
	public const string Guster = "gus";
}