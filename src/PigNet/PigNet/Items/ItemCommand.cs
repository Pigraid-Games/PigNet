using System;
using System.Numerics;
using log4net;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public class ItemCommand : Item
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemCommand));

	public override string Id { get; protected set; } = "minet:command";

	public Action<ItemCommand, Level, Player, BlockCoordinates> Action { get; set; }
	public bool NeedBlockRevert { get; set; }

	public ItemCommand(string id, short metadata, Action<ItemCommand, Level, Player, BlockCoordinates> action)
	{
		Metadata = metadata;
		Action = action ?? throw new ArgumentNullException(nameof(action));
		Item realItem = ItemFactory.GetItem(id, metadata);
		NeedBlockRevert = realItem is ItemBlock;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (NeedBlockRevert)
		{
			BlockCoordinates coord = GetNewCoordinatesFromFace(blockCoordinates, face);

			Log.Info("Reset block");
			// Resend the block to removed the new one
			Block block = world.GetBlock(coord);
			world.SetBlock(block);
		}

		Action(this, world, player, blockCoordinates);

		return true;
	}
}