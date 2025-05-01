using System;
using PigNet.Effects;

namespace PigNet.Items;

public partial class ItemPoisonousPotato() : FoodItemBase(2, 1.2)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);
		var random = new Random();
		int chance = random.Next(100);
		if (chance >= 60) return;
		var poisonEffect = new Poison
		{
			Duration = 100,
			EffectId = EffectType.Poison,
			Level = 2
		};
		player.SetEffect(poisonEffect);
	}
}