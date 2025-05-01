using PigNet.Effects;

namespace PigNet.Items;

public partial class ItemGoldenApple() : FoodItemBase(4, 9.6) 
{
	protected override void Consume(Player player)
	{
		base.Consume(player);
		player.SetEffect(new Absorption { Duration = 2400 });
		player.SetEffect(new Regeneration
		{
			Duration = 100,
			Level = 1
		});
	}
}