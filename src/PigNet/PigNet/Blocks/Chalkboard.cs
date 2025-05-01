using System;
using System.Numerics;
using log4net;
using PigNet.BlockEntities;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Chalkboard
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Chalkboard));

	public short Size { get; set; }

	public Chalkboard()
	{
		Edu = true;
		IsTransparent = true;
		IsSolid = false;
		BlastResistance = 5;
		Hardness = 1;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return world.GetBlock(blockCoordinates).IsReplaceable;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		switch (Size)
		{
			case 0:
			{
				Size = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180) * 16 / 360) + 0.5) & 0x0f);

				var block = new Chalkboard
				{
					Coordinates = Coordinates,
					Size = Size
				};
				world.SetBlock(block);

				var blockEntity = new ChalkboardBlockEntity
				{
					BaseCoordinates = Coordinates,
					Coordinates = Coordinates,
					Owner = player.EntityId,
					Size = Size,
					OnGround = true,
					Text = string.Empty
				};

				world.SetBlockEntity(blockEntity);
				break;
			}
			case 1:
			{
				Size = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180) * 4 / 360) + 0.5) & 0x0f);

				var current = Coordinates;

				for (int x = 0; x < 2; x++)
				{
					var block = new Chalkboard
					{
						Coordinates = current + GetDirCoord() * x,
						Size = Size
					};
					world.SetBlock(block);
					var blockEntity = new ChalkboardBlockEntity
					{
						BaseCoordinates = Coordinates,
						Coordinates = current + GetDirCoord() * x,
						Owner = player.EntityId,
						Size = Size,
						OnGround = true,
					};
					world.SetBlockEntity(blockEntity);
				}
				break;
			}
			case 2:
			{
				Size = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180) * 4 / 360) + 0.5) & 0x0f);

				for (int y = 0; y < 2; y++)
				{
					var current = Coordinates + BlockCoordinates.Up * y;

					for (int x = -1; x < 2; x++)
					{
						var block = new Chalkboard
						{
							Coordinates = current + GetDirCoord() * x,
							Size = Size
						};
						world.SetBlock(block);
						var blockEntity = new ChalkboardBlockEntity
						{
							BaseCoordinates = Coordinates,
							Coordinates = current + GetDirCoord() * x,
							Owner = player.EntityId,
							Size = Size,
							OnGround = true,
						};
						world.SetBlockEntity(blockEntity);
					}
				}
				break;
			}
		}

		return true;
	}

	public override void BreakBlock(Level world, BlockFace face, bool silent = false)
	{
		var blockEntity = world.GetBlockEntity(Coordinates) as ChalkboardBlockEntity;
		if (blockEntity == null)
		{
			Log.Warn($"Found no block entity at {Coordinates}");
			return;
		}
		BlockCoordinates baseCoord = blockEntity.BaseCoordinates;
		var baseBlockEntity = world.GetBlockEntity(baseCoord) as ChalkboardBlockEntity;
		if (baseBlockEntity == null)
		{
			Log.Warn($"Found no base block entity at {baseCoord}");
			return;
		}

		int size = baseBlockEntity.Size ?? 0;

		switch (size)
		{
			case 0:
				world.SetAir(Coordinates);
				world.RemoveBlockEntity(Coordinates);
				break;
			case 1:
			{
				for (int x = 0; x < 2; x++)
				{
					world.SetAir(baseCoord + GetDirCoord() * x);
					world.RemoveBlockEntity(baseCoord + GetDirCoord() * x);
				}
				break;
			}
			case 2:
			{
				for (int y = 0; y < 2; y++)
				{
					BlockCoordinates current = baseCoord + BlockCoordinates.Up * y;
					for (int x = -1; x < 2; x++)
					{
						world.SetAir(current + GetDirCoord() * x);
						world.RemoveBlockEntity(current + GetDirCoord() * x);
					}
				}
				break;
			}
		}
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		return true;
	}

	public override Item[] GetDrops(Level world, Item tool)
	{
		return [new ItemOakSign()];
	}

	private BlockCoordinates GetDirCoord()
	{
		BlockCoordinates direction = (Size & 0x07) switch
		{
			0 => Level.West,
			1 => Level.South,
			2 => Level.East,
			3 => Level.North,
			_ => new BlockCoordinates()
		};

		return direction;
	}
}