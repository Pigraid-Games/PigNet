using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemBoard : ItemBlock<Chalkboard>
{
	public ItemBoard() : this(2)
	{

	}

	protected ItemBoard(short size = 0)
	{
		Block.Size = (byte) (Metadata = size);
		MaxStackSize = 16;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		// block 230, data 32-35 (rotations) Slate, Poster or Board
		return face != BlockFace.Down && // At the bottom of block
				// Doesn't work, ignore if that happen. 
				base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
	}
}

public class ItemPoster : ItemBoard
{
	public ItemPoster() : base(1)
	{
	}
}