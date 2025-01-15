namespace MiNET.Items.Food;

public class ItemHoneyBottle() : FoodItem("minecraft:honey_bottle", 0, 6, 1.2)
{
	protected override void Consume(Player player)
	{
		base.Consume(player);
		
		foreach(var effect in player.Effects)
		{
			if(effect.Key == Effects.EffectType.Poison || effect.Key == Effects.EffectType.FatalPoison)
			{
				player.RemoveEffect(effect.Value);
			}
		}
	}
}