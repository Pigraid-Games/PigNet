using System.Collections.Generic;
using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class SlabBase : Block
{
	static SlabBase()
	{
		foreach (string id in BlockFactory.Ids)
		{
			if (!id.Contains("_slab") || !id.Contains("double_")) continue;
			string slabId = id.Replace("double_", "");
			SlabToDoubleSlabMap.Add(slabId, id);
			DoubleSlabToSlabMap.Add(id, slabId);
		}
	}

	protected SlabBase()
	{
		BlastResistance = 30;
		Hardness = 2;
		IsTransparent = true; // Partial - blocks light.
		IsBlockingSkylight = false; // Partial - blocks light.
	}

	public static Dictionary<string, string> DoubleSlabToSlabMap { get; } = new();
	public static Dictionary<string, string> SlabToDoubleSlabMap { get; } = new();

	public abstract VerticalHalf VerticalHalf { get; set; }

	public override BoundingBox GetBoundingBox()
	{
		var bottom = (Vector3) Coordinates;

		if (VerticalHalf == VerticalHalf.Top) bottom.Y += 0.5f;

		Vector3 top = bottom + new Vector3(1f, 0.5f, 1f);

		return new BoundingBox(bottom, top);
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return base.CanPlace(world, player, blockCoordinates, targetCoordinates, face) || world.GetBlock(blockCoordinates).Id == Id;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block targetBlock = world.GetBlock(targetCoordinates);

		if (targetBlock != null && face == BlockFace.Up && faceCoords.Y == 0.5 && AreSameType(targetBlock))
		{
			// Replace with double block
			SetDoubleSlab(world, targetCoordinates);
			return true;
		}

		if (targetBlock != null && face == BlockFace.Down && faceCoords.Y == 0.5 && AreSameType(targetBlock))
		{
			// Replace with double block
			SetDoubleSlab(world, targetCoordinates);
			return true;
		}

		Block existingBlock = world.GetBlock(Coordinates);
		if (existingBlock == null || !AreSameType(existingBlock))
		{
			if ((face != BlockFace.Up && faceCoords.Y > 0.5) || (face == BlockFace.Down && faceCoords.Y == 0.0)) VerticalHalf = VerticalHalf.Top;
			return false;
		}

		// Same material in existing block, make double slab
		// Create double slab, replace existing
		SetDoubleSlab(world, Coordinates);

		return true;
	}

	public override Item GetItem(Level world, bool blockItem = false)
	{
		ItemBlock item = ItemFactory.GetItem(this);
		var block = item.Block as SlabBase;

		block.VerticalHalf = VerticalHalf.Bottom;

		return item;
	}

	protected virtual bool AreSameType(Block obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		return obj.GetType() == GetType();
	}

	protected void SetDoubleSlab(Level world, BlockCoordinates coordinates)
	{
		Block slab = BlockFactory.GetBlockById(SlabToDoubleSlabMap[Id]);
		slab.Coordinates = coordinates;
		slab.SetStates(this);
		world.SetBlock(slab);
	}
}