using PigNet.Utils;

namespace PigNet.BlockEntities;

	public interface ICustomBlockEntityFactory
	{
		BlockEntity GetBlockEntityById(string blockEntityId);
	}

	public static class BlockEntityFactory
	{
		public static ICustomBlockEntityFactory CustomBlockEntityFactory { get; set; }

		public static BlockEntity GetBlockEntityById(string blockEntityId)
		{
			BlockEntity blockEntity = CustomBlockEntityFactory?.GetBlockEntityById(blockEntityId);

			if (blockEntity != null) return blockEntity;

			return blockEntityId switch
			{
				BlockEntityIds.Sign => new SignBlockEntity(),
				BlockEntityIds.HangingSign => new HangingSignBlockEntity(),
				BlockEntityIds.Chest => new ChestBlockEntity(),
				BlockEntityIds.EnderChest => new ChestBlockEntity(),
				BlockEntityIds.EnchantTable => new EnchantingTableBlockEntity(),
				BlockEntityIds.Furnace => new FurnaceBlockEntity(),
				BlockEntityIds.BlastFurnace => new BlastFurnaceBlockEntity(),
				BlockEntityIds.Skull => new SkullBlockEntity(),
				BlockEntityIds.ItemFrame => new ItemFrameBlockEntity(),
				BlockEntityIds.Bed => new BedBlockEntity(),
				BlockEntityIds.Banner => new BannerBlockEntity(),
				BlockEntityIds.FlowerPot => new FlowerPotBlockEntity(),
				BlockEntityIds.Beacon => new BeaconBlockEntity(),
				BlockEntityIds.MobSpawner => new MobSpawnerBlockEntity(),
				BlockEntityIds.ChalkboardBlock => new ChalkboardBlockEntity(),
				BlockEntityIds.ShulkerBox => new ShulkerBoxBlockEntity(),
				BlockEntityIds.StructureBlock => new StructureBlockBlockEntity(),
				BlockEntityIds.Cauldron => new CauldronBlockEntity(),
				_ => blockEntity
			};
		}

		public static bool TryGetWindowTypeById(string blockEntityId, out WindowType windowType)
		{
			windowType = blockEntityId switch
			{
				BlockEntityIds.Barrel => WindowType.Container,
				BlockEntityIds.Beacon => WindowType.Beacon,
				BlockEntityIds.BlastFurnace => WindowType.BlastFurnace,
				BlockEntityIds.BrewingStand => WindowType.BrewingStand,
				BlockEntityIds.Chest => WindowType.Container,
				BlockEntityIds.EnchantTable => WindowType.Enchantment,
				BlockEntityIds.EnderChest => WindowType.Container,
				BlockEntityIds.Furnace => WindowType.Furnace,
				BlockEntityIds.Hopper => WindowType.Hopper,
				BlockEntityIds.Lectern => WindowType.Lectern,
				BlockEntityIds.ShulkerBox => WindowType.Container,
				BlockEntityIds.Smoker => WindowType.Smoker,
				BlockEntityIds.StructureBlock => WindowType.StructureEditor,

				_ => WindowType.None
			};

			return windowType != WindowType.None;
		}
	}