using System;
using System.Numerics;
using log4net;
using PigNet.Blocks.States;
using PigNet.Items;
using PigNet.Particles;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Vine
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Vine));

	public Vine()
	{
		IsSolid = false;
		IsTransparent = true;
		BlastResistance = 1;
		Hardness = 0.2f;
		IsFlammable = true;
		IsReplaceable = true;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		if (!base.CanPlace(world, player, blockCoordinates, targetCoordinates, face)) return false;

		var onTop = world.GetBlock(Coordinates.BlockUp()) as Vine;
		if (face is BlockFace.Up or BlockFace.Down) return onTop != null;

		return CanPlace(world, this, onTop, face.Opposite().ToDirection());
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (world.GetBlock(Coordinates) is Vine block) VineDirectionBits = block.VineDirectionBits;

		if (face is BlockFace.Up or BlockFace.Down)
		{
			bool canPlace = false;
			var onTop = world.GetBlock(Coordinates.BlockUp()) as Vine;
			foreach (Direction direction in Enum.GetValues<Direction>())
			{
				if (VineDirectionBits.HasSide(direction)) continue;
				if (!CanPlace(world, this, onTop, direction)) continue;

				canPlace = true;
				face = direction.ToBlockFace().Opposite();
				break;
			}

			if (!canPlace) return true;
		}

		BlockFace vineFace = face.Opposite();

		if (VineDirectionBits.HasSide(vineFace)) return true;

		VineDirectionBits += vineFace;

		return false;
	}

	//public override void BreakBlock(Level level, BlockFace face, bool silent = false)
	//{
	//	Log.Debug($"Breaking vine face {face}, have direction: {VineDirectionBits}");
	//	int newValue = GetDirectionBits(level, this);
	//	switch (face)
	//	{
	//		case BlockFace.North:
	//			newValue &= ~North;
	//			break;
	//		case BlockFace.East:
	//			newValue &= ~East;
	//			break;
	//		case BlockFace.South:
	//			newValue &= ~South;
	//			break;
	//		case BlockFace.West:
	//			newValue &= ~West;
	//			break;
	//	}
	//	Log.Debug($"Breaking vine, new value: {newValue}, old {VineDirectionBits}");
	//	if (newValue != VineDirectionBits)
	//	{
	//		VineDirectionBits = newValue;
	//		if (VineDirectionBits != 0)
	//		{
	//			level.SetBlock(this);
	//		}
	//		else
	//		{
	//			base.BreakBlock(level, face, silent);
	//		}
	//	}
	//}

	public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
	{
		VineDirectionBits newValue = GetDirectionBits(level, this);
		if (newValue == VineDirectionBits) return;

		VineDirectionBits = newValue;

		if (VineDirectionBits == VineDirectionBits.None)
			level.BreakBlock(null, this);
		else
		{
			level.SetBlock(this);
			UpdateBlocks(level);
			new DestroyBlockParticle(level, this).Spawn();
		}
	}

	private static VineDirectionBits GetDirectionBits(Level level, Vine vine)
	{
		VineDirectionBits newVineDirectionBits = VineDirectionBits.None;

		var onTop = level.GetBlock(vine.Coordinates.BlockUp()) as Vine;
		foreach (Direction direction in Enum.GetValues<Direction>())
		{
			if (!vine.VineDirectionBits.HasSide(direction)) continue;
			if (!CanPlace(level, vine, onTop, direction)) continue;

			newVineDirectionBits += direction;
		}

		return newVineDirectionBits;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return tool.ItemType != ItemType.Sheers ? [] : base.GetDrops(world, tool);
	}

	private static bool CanPlace(Level level, Vine vine, Vine onTop, Direction direction)
	{
		bool hasSideTop = onTop != null && onTop.VineDirectionBits.HasSide(direction);
		bool hasFaceBlockSide = level.GetBlock(vine.Coordinates + direction).IsSolid;
		return hasSideTop || hasFaceBlockSide;
	}
}