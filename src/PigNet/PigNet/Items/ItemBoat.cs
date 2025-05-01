using System.Numerics;
using log4net;
using PigNet.Entities.Vehicles;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemBoat : ItemBoatBase;

public abstract class ItemBoatBase : Item
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemBoat));

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		BlockCoordinates coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

		var entity = new Boat(world)
		{
			KnownPosition = coordinates
		};
		entity.SpawnEntity();

		if (player.GameMode != GameMode.Survival) return true;
		Item itemInHand = player.Inventory.GetItemInHand();
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);

		return true;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		Log.Debug("Use item boat");
		base.UseItem(world, player, blockCoordinates);
	}
}