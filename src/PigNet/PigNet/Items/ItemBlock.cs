using System;
using System.Numerics;
using fNbt.Serialization;
using log4net;
using Newtonsoft.Json;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

/// <summary>
///     Generic Item that will simply place the block on use. No interaction or other use supported by the block.
/// </summary>
public abstract class ItemBlock<TBlock> : ItemBlock where TBlock : Block, new()
{
	public ItemBlock() : this(new TBlock())
	{
	}

	protected ItemBlock(TBlock block) : base(block)
	{
	}

	[JsonIgnore] public new TBlock Block => (TBlock) base.Block;
}

/// <summary>
///     Generic Item that will simply place the block on use. No interaction or other use supported by the block.
/// </summary>
public class ItemBlock : Item
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemBlock));

	protected ItemBlock()
	{
	}

	internal ItemBlock(Block block)
	{
		Block = block ?? throw new ArgumentNullException(nameof(block));

		Id ??= block.Id;

		FuelEfficiency = block.FuelEfficiency;
		Edu = block.Edu;
	}

	[JsonIgnore] [NbtProperty] public virtual Block Block { get; protected set; }

	public override int BlockRuntimeId => Block?.RuntimeId ?? -1;

	public override bool Edu { get => base.Edu || (Block?.Edu ?? false); protected set => base.Edu = value; }

	public override Item GetSmelt(string block)
	{
		return Block.GetSmelt(block) ?? base.GetSmelt(block);
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Block currentBlock = world.GetBlock(targetCoordinates);
		Block newBlock = BlockFactory.GetBlockById(Block.Id);
		newBlock.Coordinates = currentBlock.IsReplaceable ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

		newBlock.SetStates(Block);

		if (!newBlock.CanPlace(world, player, targetCoordinates, face)) return false;

		// TODO - invert logic
		if (newBlock.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return false;

		world.SetBlock(newBlock);

		if (player.GameMode == GameMode.Survival && newBlock is not Air)
		{
			Item itemInHand = player.Inventory.GetItemInHand();
			itemInHand.Count--;
			player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
		}

		// TODO - should move to the Block
		world.BroadcastSound(newBlock.Coordinates, LevelSoundEventType.Place, newBlock.RuntimeId);

		return true;
	}

	public override object Clone()
	{
		var item = (ItemBlock) base.Clone();

		item.Block = Block?.Clone() as Block;

		return item;
	}

	public override string ToString()
	{
		return $"{GetType().Name}(Id={Id}, Meta={Metadata}, UniqueId={UniqueId}) {{Block={Block?.GetType().Name}}} Count={Count}, NBT={ExtraData}";
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Block.GetHashCode());
	}

	internal void SetBlock(Block block)
	{
		Block = block;
	}

	protected override bool Equals(Item other)
	{
		return other is ItemBlock otherItemBlock
				&& base.Equals(other)
				&& Block.Equals(otherItemBlock.Block);
	}
}