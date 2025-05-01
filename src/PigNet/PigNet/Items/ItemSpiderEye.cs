using PigNet.Effects;

namespace PigNet.Items;

public partial class ItemSpiderEye() : FoodItemBase(2, 3.2)
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