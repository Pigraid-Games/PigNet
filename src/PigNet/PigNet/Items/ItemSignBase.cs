using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public abstract class ItemSignBase : ItemBlock
{
	protected ItemSignBase()
	{
		MaxStackSize = 16;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return !SetupSignBlock(face) || base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
	}

	protected virtual bool SetupSignBlock(BlockFace face)
	{
		if (face == BlockFace.Down) return false;
		string id = Id;
		id = id.Replace("dark_oak", "darkoak");
		if (this is ItemOakSign) id = id.Replace("oak_", "");
		Block = BlockFactory.GetBlockById(face == BlockFace.Up ? id.Replace("sign", "standing_sign") : id.Replace("sign", "wall_sign"));
		return true;
	}
}