using System.Collections.Generic;
using PigNet.Effects;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemPotion
{
	private bool _isUsing;

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (_isUsing)
		{
			List<Effect> effects = Effect.GetEffects(Metadata);

			foreach (Effect effect in effects) player.SetEffect(effect);

			if (player.GameMode is GameMode.Survival or GameMode.Adventure)
			{
				player.Inventory.ClearInventorySlot((byte) player.Inventory.InHandSlot);
				player.Inventory.SetFirstEmptySlot(new ItemGlassBottle(), true);
			}
			_isUsing = false;
			return;
		}

		_isUsing = true;
	}

	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		_isUsing = false;
	}
}