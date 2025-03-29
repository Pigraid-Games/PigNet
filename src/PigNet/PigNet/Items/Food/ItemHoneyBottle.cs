using System.Collections.Generic;
using PigNet.Effects;

namespace PigNet.Items.Food;

public class ItemHoneyBottle() : FoodItem("minecraft:honey_bottle", 737, 0, 6, 1.2)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);

		foreach (KeyValuePair<EffectType, Effect> effect in player.Effects)
			if (effect.Key == EffectType.Poison || effect.Key == EffectType.FatalPoison)
				player.RemoveEffect(effect.Value);
	}
}