using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public abstract class ItemHeadBase : ItemBlock
{
	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		if (!base.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return false;
		var skullBlockEntity = new SkullBlockEntity
		{
			Coordinates = GetNewCoordinatesFromFace(targetCoordinates, face),
			Rotation = (byte) player.KnownPosition.GetDirection16()
		};
		world.SetBlockEntity(skullBlockEntity);
		return true;
	}
}