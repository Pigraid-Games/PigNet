﻿namespace PigNet.Items.Food;

public class ItemRabbit() : FoodItem("minecraft:rabbit", 411, 0, 1, 0.6)
{
	public override Item GetSmelt()
	{
		return new ItemCookedRabbit();
	}
}