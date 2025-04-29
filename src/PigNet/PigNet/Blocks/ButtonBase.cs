using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class ButtonBase : Block
{
	public int TickRate { get; set; }

	public abstract bool ButtonPressedBit { get; set; }
	public abstract OldFacingDirection4 FacingDirection { get; set; }

	protected ButtonBase()
	{
		IsSolid = false;
		IsTransparent = true;
		BlastResistance = 2.5f;
		Hardness = 0.5f;

		TickRate = 30;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		FacingDirection = face;

		world.SetBlock(this);
		return true;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		ButtonPressedBit = true;
		world.SetBlock(this);
		world.ScheduleBlockTick(this, TickRate);
		return true;
	}

	public override void OnTick(Level level, bool isRandom)
	{
		if (isRandom) return;

		ButtonPressedBit = false;
		level.SetBlock(this);
	}
}