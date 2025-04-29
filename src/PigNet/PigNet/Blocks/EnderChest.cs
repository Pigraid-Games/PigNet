using System.Numerics;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class EnderChest : Block
{
	public EnderChest()
	{
		IsTransparent = true;
		LightLevel = 7;
		BlastResistance = 3000;
		Hardness = 22.5f;
		FuelEfficiency = 0;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		// each enderchest inventory is a players individual inventory stored in their main inventory

		return base.Interact(world, player, blockCoordinates, face, faceCoord);
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [ItemFactory.GetItem<Obsidian>(count: 8)];
	}
}