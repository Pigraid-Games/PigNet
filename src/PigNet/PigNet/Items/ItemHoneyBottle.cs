using System.Collections.Generic;
using PigNet.Effects;

namespace PigNet.Items;

public partial class ItemHoneyBottle() : FoodItemBase(6, 1.2)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);

		foreach (KeyValuePair<EffectType, Effect> effect in player.Effects)
			if (effect.Key is EffectType.Poison or EffectType.FatalPoison)
				player.RemoveEffect(effect.Value);
	}
}