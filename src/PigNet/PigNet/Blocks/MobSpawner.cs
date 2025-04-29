using System;
using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Entities;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class MobSpawner : Block
{
	public MobSpawner()
	{
		IsTransparent = true;
		LightLevel = 1;
		BlastResistance = 25;
		Hardness = 5;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [];
	}

	public override float GetExperiencePoints()
	{
		var random = new Random();
		return random.Next(15, 44);
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		var blockEntity = new MobSpawnerBlockEntity
		{
			Coordinates = Coordinates
		};
		world.SetBlockEntity(blockEntity);

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		if (player.Inventory.GetItemInHand() is not ItemSpawnEggBase monsterEgg) return true;
		if (world.GetBlockEntity(Coordinates) is not MobSpawnerBlockEntity blockEntity) return true;
		Entity entity = monsterEgg.Metadata.CreateEntity(world);
		if (entity == null) return true;
		
		blockEntity.EntityTypeId = (int)EntityHelpers.ToEntityType(entity.EntityTypeId);
		blockEntity.DisplayEntityHeight = (float) entity.Height;
		blockEntity.DisplayEntityWidth = (float) entity.Width;
		blockEntity.DisplayEntityScale = (float) entity.Scale;

		world.SetBlockEntity(blockEntity);

		return true;
	}
}