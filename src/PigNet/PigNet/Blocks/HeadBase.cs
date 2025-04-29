using System.Numerics;
using PigNet.Blocks.States;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class HeadBase : Block
{
	public abstract OldFacingDirection4 FacingDirection { get; set; }

	public HeadBase()
	{
		IsTransparent = true;
		BlastResistance = 5;
		Hardness = 1;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (face == BlockFace.Down)
		{
			return false;
		}

		FacingDirection = face;

			
		return false;
	}

	public HeadType GetHeadType()
	{
		switch (this)
		{
			case SkeletonSkull:
				return HeadType.Skeleton;
			case WitherSkeletonSkull:
				return HeadType.WitherSkeleton;
			case ZombieHead:
				return HeadType.Zombie;
			case PlayerHead:
				return HeadType.Player;
			case CreeperHead:
				return HeadType.Creeper;
			case DragonHead:
				return HeadType.Dragon;
			case PiglinHead:
				return HeadType.Piglin;

			default:
				return HeadType.Skeleton;
		}
	}
}

public enum HeadType : byte
{
	Skeleton,
	WitherSkeleton,
	Zombie,
	Player,
	Creeper,
	Dragon,
	Piglin
}