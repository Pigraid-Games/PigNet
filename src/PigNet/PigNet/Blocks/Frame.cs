using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Frame
{
	public Frame()
	{
		IsTransparent = true;
		IsSolid = false;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return world.GetBlock(blockCoordinates).IsReplaceable;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		FacingDirection = face;

		var itemFrameBlockEntity = new ItemFrameBlockEntity {Coordinates = Coordinates};
		world.SetBlockEntity(itemFrameBlockEntity);

		return false;
	}
	
	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		Item itemInHand = player.Inventory.GetItemInHand();

		if (world.GetBlockEntity(blockCoordinates) is not ItemFrameBlockEntity blockEntity) return true;
		var rotation = blockEntity.GetLagacyRotation();

		if (itemInHand.Equals(blockEntity.Item) && itemInHand.Equals(new ItemAir())) return true;

		if (itemInHand.Equals(blockEntity.Item) || itemInHand.Equals(new ItemAir()))
		{
			rotation++;
			if (rotation > 7) rotation = 0;
		}
		else
		{
			rotation = 0;
			blockEntity.Item = itemInHand;
		}

		blockEntity.SetLagacyRotation(rotation);

		world.SetBlockEntity(blockEntity);

		return true;
	}

	public void ClearItem(Level world)
	{
		if (world.GetBlockEntity(Coordinates) is not ItemFrameBlockEntity blockEntity) return;
		Item item = blockEntity.Item;
		blockEntity.Item = new ItemAir();
		blockEntity.Rotation = 0;

		world.SetBlockEntity(blockEntity);
		if (item != null) world.DropItem(Coordinates, item);
	}
}