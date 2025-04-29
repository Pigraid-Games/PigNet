using System.Numerics;
using PigNet.BlockEntities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class StructureBlock : Block
{
	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		var blockEntity = new StructureBlockBlockEntity {Coordinates = Coordinates};
		world.SetBlockEntity(blockEntity);

		return false;
	}
}