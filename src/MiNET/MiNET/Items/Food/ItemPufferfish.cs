using MiNET.Effects;

namespace MiNET.Items.Food;

public class ItemPufferFish() : FoodItem("minecraft:pufferfish", 0, 1, 0.2)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);

		var hungerEffect = new Hunger()
		{
			EffectId = EffectType.Hunger,
			Level = 3,
			Duration = 300,
		};
		player.SetEffect(hungerEffect);

		var nauseaEffect = new Nausea()
		{
			EffectId = EffectType.Nausea,
			Level = 2,
			Duration = 300,
		};
		player.SetEffect(nauseaEffect);

		var poisonEffect = new Poison()
		{
			EffectId = EffectType.Poison,
			Level = 2,
			Duration = 1200
		};
		player.SetEffect(poisonEffect);
	}
}