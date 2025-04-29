using System;
using System.Numerics;
using fNbt;
using PigNet.BlockEntities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class StandingBanner : Block
{
	public int BaseColor { get; set; }
	public NbtCompound ExtraData { get; set; }

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
		// metadata for banner is a value 0-15 that signify the orientation of the banner. Same as PC metadata.
		GroundSignDirection = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180) * 16 / 360) + 0.5) & 0x0f);

		var bannerBlockEntity = new BannerBlockEntity
		{
			Coordinates = Coordinates,
			BaseColor = BaseColor,
		};
		bannerBlockEntity.SetCompound(ExtraData);
		world.SetBlockEntity(bannerBlockEntity);

		return false;
	}
}