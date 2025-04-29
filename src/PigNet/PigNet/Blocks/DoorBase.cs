using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class DoorBase : Block
{
	public abstract OldDirection3 Direction { get; set; }
	public abstract bool DoorHingeBit { get; set; }
	public abstract bool OpenBit { get; set; }
	public abstract bool UpperBlockBit { get; set; }

	private BlockCoordinates SecondPartCoordinates => UpperBlockBit ? Coordinates.BlockDown() : Coordinates.BlockUp();

	protected DoorBase()
	{
		IsTransparent = true;
		BlastResistance = 15;
		Hardness = 3;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return world.GetBlock(blockCoordinates).IsReplaceable && world.GetBlock(blockCoordinates.BlockUp()).IsReplaceable;
	}

	public override void BreakBlock(Level level, BlockFace face, bool silent = false)
	{
		Block secondPart = level.GetBlock(SecondPartCoordinates);

		BreakBlockInternal(level, face, silent);
		if (secondPart is DoorBase secondPartDoor) secondPartDoor.BreakBlockInternal(level, face, silent);
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		DoorBase block = this;
		if (UpperBlockBit)
		{
			block = (DoorBase) world.GetBlock(SecondPartCoordinates);
		}

		block.OpenBit = !block.OpenBit;
		world.SetBlock(block);

		return true;
	}

	private void BreakBlockInternal(Level level, BlockFace face, bool silent = false)
	{
		base.BreakBlock(level, face, silent);
	}
}