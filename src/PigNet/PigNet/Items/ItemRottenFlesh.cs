using System;
using PigNet.Effects;

namespace PigNet.Items;

public partial class ItemRottenFlesh() : FoodItemBase(4, 0.8)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);
		var random = new Random();
		int chance = random.Next(100);
		if (chance >= 80) return;
		var hungerEffect = new Hunger
		{
			Duration = 600,
			EffectId = EffectType.Hunger,
			Level = 1
		};
		player.SetEffect(hungerEffect);
	}
}