using System;
using System.Numerics;
using log4net;
using PigNet.Entities;
using PigNet.Entities.Hostile;
using PigNet.Entities.Passive;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemSpawnEgg
{
	public ItemSpawnEgg()
	{
	}

	public ItemSpawnEgg(EntityType entityType) : base(entityType)
	{
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return SpawnMob((EntityType) Metadata, world, player, blockCoordinates, face);
	}
}

public abstract class ItemSpawnEggBase : Item
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemSpawnEgg));

	protected ItemSpawnEggBase(EntityType entityType)
	{
		Metadata = (short) entityType;
	}

	public ItemSpawnEggBase()
	{
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return SpawnMob(EntityHelpers.ToEntityType(Id.Replace("_spawn_egg", "")), world, player, blockCoordinates, face);
	}

	protected bool SpawnMob(EntityType type, Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
	{
		Log.WarnFormat("Player {0} trying to spawn Mob #{1}.", player.Username, Metadata);

		BlockCoordinates coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

		Mob mob = null;

		switch (type)
		{
			case EntityType.Chicken:
				mob = new Chicken(world);
				break;
			case EntityType.Cow:
				mob = new Cow(world);
				break;
			case EntityType.Pig:
				mob = new Pig(world);
				break;
			case EntityType.Sheep:
				mob = new Sheep(world);
				break;
			case EntityType.Wolf:
				mob = new Wolf(world) { Owner = player };
				break;
			case EntityType.Villager:
				mob = new Villager(world);
				break;
			case EntityType.MushroomCow:
				mob = new MushroomCow(world);
				break;
			case EntityType.Squid:
				mob = new Squid(world);
				break;
			case EntityType.Rabbit:
				mob = new Rabbit(world);
				break;
			case EntityType.Bat:
				mob = new Bat(world);
				break;
			case EntityType.IronGolem:
				mob = new IronGolem(world);
				break;
			case EntityType.SnowGolem:
				mob = new SnowGolem(world);
				break;
			case EntityType.Ocelot:
				mob = new Ocelot(world);
				break;
			case EntityType.Zombie:
				mob = new Zombie(world);
				break;
			case EntityType.Creeper:
				mob = new Creeper(world);
				break;
			case EntityType.Skeleton:
				mob = new Skeleton(world);
				break;
			case EntityType.Spider:
				mob = new Spider(world);
				break;
			case EntityType.ZombiePigman:
				mob = new ZombiePigman(world);
				break;
			case EntityType.Slime:
				mob = new Slime(world);
				break;
			case EntityType.Enderman:
				mob = new Enderman(world);
				break;
			case EntityType.Silverfish:
				mob = new Silverfish(world);
				break;
			case EntityType.CaveSpider:
				mob = new CaveSpider(world);
				break;
			case EntityType.Ghast:
				mob = new Ghast(world);
				break;
			case EntityType.MagmaCube:
				mob = new MagmaCube(world);
				break;
			case EntityType.Blaze:
				mob = new Blaze(world);
				break;
			case EntityType.ZombieVillager:
				mob = new ZombieVillager(world);
				break;
			case EntityType.Witch:
				mob = new Witch(world);
				break;
			case EntityType.Stray:
				mob = new Stray(world);
				break;
			case EntityType.Husk:
				mob = new Husk(world);
				break;
			case EntityType.WitherSkeleton:
				mob = new WitherSkeleton(world);
				break;
			case EntityType.Guardian:
				mob = new Guardian(world);
				break;
			case EntityType.ElderGuardian:
				mob = new ElderGuardian(world);
				break;
			case EntityType.Horse:
				var random = new Random();
				mob = new Horse(world, random.NextDouble() < 0.10, random);
				break;
			case EntityType.PolarBear:
				mob = new PolarBear(world);
				break;
			case EntityType.Shulker:
				mob = new Shulker(world);
				break;
			case EntityType.Dragon:
				mob = new Dragon(world);
				break;
			case EntityType.SkeletonHorse:
				mob = new SkeletonHorse(world);
				break;
			case EntityType.Wither:
				mob = new Wither(world);
				break;
			case EntityType.Evoker:
				mob = new Evoker(world);
				break;
			case EntityType.Vindicator:
				mob = new Vindicator(world);
				break;
			case EntityType.Vex:
				mob = new Vex(world);
				break;
			case EntityType.Npc:
				mob = new PlayerMob("test", world);
				break;
		}

		if (mob == null) return false;

		mob.KnownPosition = new PlayerLocation(coordinates.X, coordinates.Y, coordinates.Z) + new Vector3(0.5f, 0, 0.5f);
		mob.NoAi = true;
		mob.SpawnEntity();

		Log.WarnFormat("Player {0} spawned Mob #{1}.", player.Username, Metadata);

		if (player.GameMode != GameMode.Survival) return true;
		Item itemInHand = player.Inventory.GetItemInHand();
		itemInHand.Count--;
		player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);

		return true;
	}
}