using System;
using System.Numerics;
using PigNet.Items;
using PigNet.Items.Food;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Beetroot : Crops
{
	public Beetroot()
	{
		MaxGrowth = 4;
	}

	public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		Item itemInHand = player.Inventory.GetItemInHand();
		if (Growth >= MaxGrowth || itemInHand is not ItemBoneMeal || !(new Random().NextDouble() > 0.25)) return false;
		Growth++;
		level.SetBlock(this);

		return true;

	}


	public override Item[] GetDrops(Level world, Item tool)
	{
		if (Growth != MaxGrowth) return [new ItemBeetrootSeeds()];
		var rnd = new Random();
		int count = rnd.Next(4);
		if (count > 0) return [new ItemBeetroot(), new ItemBeetrootSeeds { Count = (byte) count }];
		return [new ItemBeetroot()];

	}
}