using PigNet.Entities.World;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemEmptyMap
{
	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		var mapEntity = new MapEntity(world);
		mapEntity.SpawnEntity();

		// Initialize a new map and add it.
		var itemMap = new ItemMap { MapId = mapEntity.EntityId };
		player.Inventory.SetFirstEmptySlot(itemMap, true);
	}
}