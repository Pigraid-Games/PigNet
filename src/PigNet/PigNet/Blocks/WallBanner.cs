using System.Numerics;
using fNbt;
using PigNet.BlockEntities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class WallBanner : Block
{
	public int BaseColor { get; set; }
	public NbtCompound ExtraData { get; set; }

	public WallBanner()
	{
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return world.GetBlock(blockCoordinates).IsReplaceable;
	}

	public override BoundingBox GetBoundingBox()
	{
		return new BoundingBox(Coordinates, Coordinates + new BlockCoordinates(1, 2, 1));
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		FacingDirection = face;

		var bannerBlockEntity = new BannerBlockEntity
		{
			Coordinates = Coordinates,
			BaseColor = BaseColor
		};
		bannerBlockEntity.SetCompound(ExtraData);
		world.SetBlockEntity(bannerBlockEntity);

		return false;
	}
}