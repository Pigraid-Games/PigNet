using fNbt.Serialization;
using log4net;
using PigNet.Blocks;
using PigNet.Inventories;
using PigNet.Items;
using PigNet.Net.Packets.Mcpe;
using PigNet.Worlds;

namespace PigNet.BlockEntities;

	public class FurnaceBlockEntityBase<TFurnace, TLitFurnace> : ContainerBlockEntityBase 
		where TFurnace : FurnaceBase, new() 
		where TLitFurnace : FurnaceBase, new()
	{

		private static readonly ILog Log = LogManager.GetLogger(typeof(FurnaceBlockEntityBase<,>));

		private static readonly string BlockId = BlockFactory.GetIdByType<TFurnace>(false);

		public short CookTime { get; set; }
		public short BurnTime { get; set; }

		[NbtProperty("BurnDuration")]
		public short FuelEfficiency { get; set; }

		[NbtIgnore]
		public short SmeltingTime { get; }

		public FurnaceBlockEntityBase(string id, short smeltingTime) : base(id, 3)
		{
			SmeltingTime = smeltingTime;
			UpdatesOnTick = true;
		}

		public override void OnTick(Level level)
		{
			if (Inventory == null) return;

			var fuel = GetFuel();
			var ingredient = GetIngredient();
			var smelt = ingredient.GetSmelt(BlockId);

			var isLit = BurnTime > 0 || CanBurn(fuel, smelt) && BurnFuel(fuel);

			if (isLit)
			{
				BurnTime--;
				if (smelt != null)
				{
					if (++CookTime >= SmeltingTime)
					{
						if (Inventory.DecreaseSlot(0))
						{
							Inventory.IncreaseSlot(2, smelt.Id, smelt.Metadata);
						}

						CookTime = 0;
					}
				}
				else
				{
					CookTime = 0;
				}
			}

			UpdateStates(level, isLit);
		}

		protected override void OnInventoryOpened(object sender, InventoryOpenedEventArgs args)
		{
			base.OnInventoryOpened(sender, args);

			SendContainerData(args.Player);
		}

		private bool BurnFuel(Item fuel)
		{
			if (!Inventory.DecreaseSlot(1)) return false;

			FuelEfficiency = (short) (fuel.FuelEfficiency * SmeltingTime / 10);
			BurnTime = FuelEfficiency;

			return true;
		}

		private bool CanBurn(Item fuel, Item smelt)
		{
			// To light a furnace you need both fule and proper ingredient.
			if (fuel is ItemAir || fuel.FuelEfficiency <= 0 || smelt == null) return false;

			Item result = GetResult();
			return result is ItemAir || result.Equals(smelt);
		}

		private void UpdateStates(Level level, bool lit)
		{
			bool needReset = CookTime > 0 || BurnTime > 0 || FuelEfficiency > 0;

			if (!lit)
			{
				FuelEfficiency = 0;
				BurnTime = 0;
				CookTime = 0;

				if (!needReset)
				{
					return;
				}
			}

			BroadcastContainerData();

			var oldFurnace = level.GetBlock(Coordinates) as FurnaceBase;
			if (oldFurnace == null)
			{
				Log.Warn($"Attempt to update [{Id}] at [{Coordinates}] without a block");

				UpdatesOnTick = false;
				return;
			}

			if (lit == oldFurnace is TLitFurnace) return;
			FurnaceBase newFurnace = lit ? new TLitFurnace() : new TFurnace();

			newFurnace.Coordinates = oldFurnace.Coordinates;
			newFurnace.Direction = oldFurnace.Direction;

			level.SetBlock(newFurnace);
		}

		private void BroadcastContainerData()
		{
			foreach (var observer in Inventory.Observers) SendContainerData(observer);
		}

		private void SendContainerData(Player player)
		{
			McpeContainerSetData cookTimeSetData = McpeContainerSetData.CreateObject();
			cookTimeSetData.containerId = (byte) Inventory.WindowId;
			cookTimeSetData.id = 0;
			cookTimeSetData.value = CookTime;
			player.SendPacket(cookTimeSetData);

			McpeContainerSetData burnTimeSetData = McpeContainerSetData.CreateObject();
			burnTimeSetData.containerId = (byte) Inventory.WindowId;
			burnTimeSetData.id = 1;
			burnTimeSetData.value = BurnTime;
			player.SendPacket(burnTimeSetData);

			McpeContainerSetData fuelEfficientySetData = McpeContainerSetData.CreateObject();
			fuelEfficientySetData.containerId = (byte) Inventory.WindowId;
			fuelEfficientySetData.id = 2;
			fuelEfficientySetData.value = FuelEfficiency;
			player.SendPacket(fuelEfficientySetData);
		}

		private Item GetResult()
		{
			return Items[2];
		}

		private Item GetFuel()
		{
			return Items[1];
		}

		private Item GetIngredient()
		{
			return Items[0];
		}
	}