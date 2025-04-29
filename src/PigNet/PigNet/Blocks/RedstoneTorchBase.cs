using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class RedstoneTorchBase : Block
{
	public abstract TorchFacingDirection TorchFacingDirection { get; set; }

	public RedstoneTorchBase()
	{
		IsTransparent = true;
		IsSolid = false;
	}

	//protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	//{
	//	Block block = world.GetBlock(blockCoordinates);
	//	if (block is Farmland
	//		|| block is Ice
	//		/*|| block is Glowstone || block is Leaves  */
	//		|| block is Tnt
	//		|| block is BlockStairs
	//		|| block is StoneSlab
	//		|| block is WoodenSlab)
	//		return true;

	//	//TODO: More checks here, but PE blocks it pretty good right now
	//	if (block is Glass && face == BlockFace.Up) return true;

	//	return !block.IsTransparent;
	//}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (face == BlockFace.Down) return true;

		TorchFacingDirection = face;

		return false;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [ItemFactory.GetItem<RedstoneTorch>()];
	}
}