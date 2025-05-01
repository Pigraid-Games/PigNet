using System;
using PigNet.Effects;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemEnchantedApple() : FoodItemBase(4, 9.6)
{
	private bool _isUsing;

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (_isUsing)
		{
			Count--;
			player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, this);

			Consume(player);
			_isUsing = false;
			return;
		}
		if (player.HungerManager.CanEat()) _isUsing = true;
	}

	protected override void Consume(Player player)
	{
		base.Consume(player);
		var absorptionEffect = new Absorption
		{
			Duration = 2400,
			EffectId = EffectType.Absorption,
			Level = 4
		};
		var regenerationEffect = new Regeneration
		{
			Duration = 600,
			EffectId = EffectType.Regeneration,
			Level = 2
		};
		var fireResistanceEffect = new FireResistance
		{
			Duration = 3600,
			EffectId = EffectType.FireResistance,
			Level = 1
		};
		var resistanceEffect = new Resistance
		{
			Duration = 3600,
			EffectId = EffectType.Resistance,
			Level = 1
		};

		player.SetEffect(absorptionEffect);
		player.SetEffect(regenerationEffect);
		player.SetEffect(fireResistanceEffect);
		player.SetEffect(resistanceEffect);
		Console.WriteLine($"Send all the effects to the player {player.Username}");
	}
}