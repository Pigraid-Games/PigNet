using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using PigNet.Blocks;
using PigNet.Entities;
using PigNet.Entities.World;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemFlintAndSteel
{
	public static int MaxPortalHeight = 30;
	public static int MaxPortalWidth = 30;

	public ItemFlintAndSteel()
	{
		MaxStackSize = 1;
		ItemType = ItemType.FlintAndSteel;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block block = world.GetBlock(blockCoordinates);
		if (block is Tnt)
		{
			world.SetAir(block.Coordinates);
			new PrimedTnt(world)
			{
				KnownPosition = new PlayerLocation
				{
					X = blockCoordinates.X + 0.5f,
					Y = blockCoordinates.Y + 0.5f,
					Z = blockCoordinates.Z + 0.5f
				},
				Fuse = 80
			}.SpawnEntity();
			player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
		}
		else if (block is Obsidian)
		{
			Block affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, face));
			if (affectedBlock is Air)
			{
				List<Block> blocks = Fill(world, affectedBlock.Coordinates, 10, BlockFace.West);
				if (blocks.Count == 0) blocks = Fill(world, affectedBlock.Coordinates, 10, BlockFace.North);

				if (blocks.Count > 0)
					foreach (Block portal in blocks.FindAll(b => b is Portal))
						world.SetBlock(portal);
				else
				{
					if (face == BlockFace.Up)
					{
						affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
						if (affectedBlock is Air)
						{
							var fire = new Fire { Coordinates = affectedBlock.Coordinates };
							world.SetBlock(fire);
						}
					}
				}
			}

			player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
			return true;
		}
		else if (block.IsSolid)
		{
			Block affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
			if (affectedBlock is Air)
			{
				var fire = new Fire { Coordinates = affectedBlock.Coordinates };
				world.SetBlock(fire);
			}

			player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
			return true;
		}

		return false;
	}

	public List<Block> Fill(Level level, BlockCoordinates origin, int radius, BlockFace direction)
	{
		var blocks = new List<Block>();
		float length = new Vector2(MaxPortalHeight, MaxPortalWidth).Length();

		var visits = new Queue<BlockCoordinates>();

		visits.Enqueue(origin); // Kick it off with some good stuff

		while (visits.Count > 0)
		{
			BlockCoordinates coordinates = visits.Dequeue();

			if (origin.DistanceTo(coordinates) >= length) return new List<Block>();

			if (level.IsAir(coordinates) && blocks.FirstOrDefault(b => b.Coordinates.Equals(coordinates)) == null)
			{
				Visit(coordinates, blocks, direction);

				if (direction == BlockFace.West)
				{
					visits.Enqueue(coordinates + Level.North);
					visits.Enqueue(coordinates + Level.South);
				}
				else if (direction == BlockFace.North)
				{
					visits.Enqueue(coordinates + Level.West);
					visits.Enqueue(coordinates + Level.East);
				}

				visits.Enqueue(coordinates + Level.Up);
				visits.Enqueue(coordinates + Level.Down);
			}
			else
			{
				Block block = level.GetBlock(coordinates);
				if (!IsValid(block, blocks)) return new List<Block>();
			}
		}

		return blocks;
	}

	private void Visit(BlockCoordinates coordinates, List<Block> blocks, BlockFace direction)
	{
		BlockAxis dir = direction switch
		{
			BlockFace.Down => BlockAxis.X,
			BlockFace.Up => BlockAxis.X,
			BlockFace.North => BlockAxis.X,
			BlockFace.South => BlockAxis.X,
			BlockFace.West => BlockAxis.Z,
			BlockFace.East => BlockAxis.Z,
			BlockFace.None => default,
			_ => default
		};

		blocks.Add(new Portal
		{
			Coordinates = coordinates,
			PortalAxis = dir
		});
	}

	private bool IsValid(Block block, List<Block> portals)
	{
		return block is Obsidian || portals.FirstOrDefault(b => b.Coordinates.Equals(block.Coordinates) && b is Portal) != null;
	}

	public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
	{
		switch (reason)
		{
			case ItemDamageReason.BlockInteract:
			{
				Metadata++;
				return Metadata >= GetMaxUses() - 1;
			}
			default:
				return false;
		}
	}

	protected override int GetMaxUses()
	{
		return 65;
	}
}