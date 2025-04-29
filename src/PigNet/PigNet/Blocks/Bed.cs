using System.Numerics;
using log4net;
using PigNet.BlockEntities;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Bed : Block
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Bed));
	public byte? Color { get; set; }

	public Bed()
	{
		BlastResistance = 1;
		Hardness = 0.2f;
		IsTransparent = true;
	}

	public override Item GetItem(Level world, bool blockItem = false)
	{
		Item item = base.GetItem(world, blockItem);

		if (world.GetBlockEntity(Coordinates) is BedBlockEntity bedBlockEntity) item.Metadata = Color ?? bedBlockEntity.Color;

		return item;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		Direction = player.KnownPosition.GetDirection().Opposite();

		return world.GetBlock(blockCoordinates).IsReplaceable && world.GetBlock(GetOtherPart()).IsReplaceable;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Item inHandItem = player.Inventory.GetItemInHand();
		if (inHandItem is not ItemBed) return false;

		HeadPieceBit = false;

		world.SetBlockEntity(new BedBlockEntity
		{
			Coordinates = Coordinates,
			Color = Color ?? (byte) inHandItem.Metadata
		});

		Bed blockOther = new Bed
		{
			Coordinates = GetOtherPart(),
			Direction = Direction,
			HeadPieceBit = true
		};

		world.SetBlock(blockOther);
		world.SetBlockEntity(new BedBlockEntity
		{
			Coordinates = blockOther.Coordinates,
			Color = Color ?? (byte) inHandItem.Metadata
		});

		return false;
	}

	public override void BreakBlock(Level level, BlockFace face, bool silent = false)
	{
		base.BreakBlock(level, face, silent);

		BlockCoordinates other = GetOtherPart();
		level.SetAir(other);
		level.RemoveBlockEntity(other);
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		if (OccupiedBit)
		{
			Log.Debug($"Bed at {Coordinates} is already occupied");
			return true;
		}

		SetOccupied(world, true);
		player.IsSleeping = true;
		player.SpawnPosition = blockCoordinates;
		player.BroadcastSetEntityData();
		return true;
	}

	public void SetOccupied(Level world, bool isOccupied)
	{
		if (world.GetBlock(GetOtherPart()) is not Bed other) return;

		OccupiedBit = isOccupied;
		other.OccupiedBit = isOccupied;
		world.SetBlock(this);
		world.SetBlock(other);
	}

	private BlockCoordinates GetOtherPart()
	{
		var face = (BlockFace) Direction;

		if (HeadPieceBit) face = face.Opposite();

		return GetNewCoordinatesFromFace(Coordinates, face);
	}
}