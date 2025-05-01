using log4net;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public abstract class FoodItemBase : Item
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(FoodItemBase));

	public int FoodPoints { get; set; }
	public double SaturationRestore { get; set; }

	public FoodItemBase(int foodPoints, double saturationRestore) : this(0, foodPoints, saturationRestore)
	{

	}

	public FoodItemBase(short metadata, int foodPoints, double saturationRestore)
	{
		Metadata = metadata;
		FoodPoints = foodPoints;
		SaturationRestore = saturationRestore;
	}

	public FoodItemBase()
	{
		Log.Warn($"Does not implemented food partial for [{Id}]");
	}

	private bool _isUsing;

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (player.GameMode != GameMode.Survival && player.GameMode != GameMode.Adventure) return;
		if (_isUsing)
		{
			Consume(player);

			Count--;
			player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, this);
			_isUsing = false;
			return;
		}

		if (player.HungerManager.CanEat()) _isUsing = true;
	}

	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		_isUsing = false;
	}

	protected virtual void Consume(Player player)
	{
		player.HungerManager.IncreaseFoodAndSaturation(this, FoodPoints, SaturationRestore);
	}
}