using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Inventories;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class AnvilBase : Block
{
	public abstract CardinalDirection Direction { get; set; }

	public AnvilBase()
	{
		IsTransparent = true;
		BlastResistance = 6000;
		Hardness = 5;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Direction = player.KnownPosition.GetDirection().Shift();

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		new Inventory(Coordinates, WindowType.Anvil).Open(player);

		//var sendSlot = McpeInventorySlot.CreateObject();
		//sendSlot.inventoryId = 14;
		//sendSlot.slot = (uint) 1;
		//sendSlot.uniqueid = 1;
		//sendSlot.item = new ItemIronShovel();
		//player.SendPacket(sendSlot);

		return true;
	}
}