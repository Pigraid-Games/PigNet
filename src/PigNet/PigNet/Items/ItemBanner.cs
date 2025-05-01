using System.Numerics;
using PigNet.Blocks;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemBanner : ItemBlock
{
	public ItemBanner()
	{
		MaxStackSize = 16;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		switch (face)
		{
			// At the bottom of block
			case BlockFace.Down:
				// Doesn't work, ignore if that happen. 
				return false;
			case BlockFace.Up:
			{
				var banner = new StandingBanner
				{
					ExtraData = ExtraData,
					BaseColor = Metadata
				};
				Block = banner;
				break;
			}
			default:
			{
				var banner = new WallBanner
				{
					ExtraData = ExtraData,
					BaseColor = Metadata
				};
				Block = banner;
				break;
			}
		}

		return base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
	}
}