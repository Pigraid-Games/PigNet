using PigNet.Effects;

namespace PigNet.Items.Food;

public class ItemSpiderEye() : FoodItem("minecraft:spider_eye", 375, 0, 2, 3.2)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);

		var poisonEffect = new Poison
		{
			EffectId = EffectType.Poison,
			Level = 1,
			Duration = 200
		};
		player.SetEffect(poisonEffect);
	}
}