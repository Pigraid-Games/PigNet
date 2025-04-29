using fNbt.Serialization;

namespace PigNet.BlockEntities;

public class ShulkerBoxBlockEntity() : ContainerBlockEntity(BlockEntityIds.ShulkerBox)
{
	[NbtProperty("facing")]
	public byte Facing { get; set; }

	protected override void OnInventoryOpened(object sender, InventoryOpenedEventArgs args)
	{
		base.OnInventoryOpened(sender, args);

		if (args.Opened)
		{
			args.Player.Level.BroadcastSound(Coordinates, LevelSoundEventType.ShulkerBoxOpen);
		}
	}

	protected override void OnInventoryClosed(object sender, InventoryClosedEventArgs args)
	{
		base.OnInventoryClosed(sender, args);

		if (args.Closed) args.Player.Level.BroadcastSound(Coordinates, LevelSoundEventType.ShulkerboxClosed);
	}
}