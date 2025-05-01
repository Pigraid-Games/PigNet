using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

//A door specifies its hinge side in the block data of its upper block, 
// and its facing and opened status in the block data of its lower block
public abstract class ItemDoorBase : ItemBlock
{
	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		//TODO - should move to the DoorBase

		Direction direction = player.KnownPosition.GetDirection().Opposite();

		BlockCoordinates coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

		// Base block, meta sets orientation
		DoorBase block = BlockFactory.GetBlockById<DoorBase>(Id);
		block.Coordinates = coordinates;
		block.Direction = direction;
		block.UpperBlockBit = false;

		int x = blockCoordinates.X;
		int y = blockCoordinates.Y;
		int z = blockCoordinates.Z;

		int xd = 0;
		int zd = 0;

		if (direction == Direction.East) zd = 1;
		else if (direction == Direction.South) xd = -1;
		else if (direction == Direction.West) zd = -1;
		else if (direction == Direction.North) xd = 1;

		int i1 = (world.GetBlock(x - xd, y, z - zd).IsSolid ? 1 : 0) + (world.GetBlock(x - xd, y + 1, z - zd).IsSolid ? 1 : 0);
		int j1 = (world.GetBlock(x + xd, y, z + zd).IsSolid ? 1 : 0) + (world.GetBlock(x + xd, y + 1, z + zd).IsSolid ? 1 : 0);
		bool flag = world.GetBlock(x - xd, y, z - zd).Id == block.Id || world.GetBlock(x - xd, y + 1, z - zd).Id == block.Id;
		bool flag1 = world.GetBlock(x + xd, y, z + zd).Id == block.Id || world.GetBlock(x + xd, y + 1, z + zd).Id == block.Id;
		bool flag2 = false;

		if (flag && !flag1)
			flag2 = true;
		else if (j1 > i1) flag2 = true;

		if (!block.CanPlace(world, player, blockCoordinates, face)) return false;

		block.DoorHingeBit = flag2;

		// The upper door block, meta marks upper and
		// sets orientation based on adjacent blocks
		DoorBase blockUpper = BlockFactory.GetBlockById<DoorBase>(Id);
		blockUpper.Coordinates = coordinates.BlockUp();
		blockUpper.Direction = direction;
		blockUpper.UpperBlockBit = true;
		blockUpper.DoorHingeBit = flag2;

		world.SetBlock(block);
		world.SetBlock(blockUpper);

		if (player.GameMode == GameMode.Survival)
		{
			Item itemInHand = player.Inventory.GetItemInHand();
			itemInHand.Count--;
			player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
		}

		return true;
	}
}