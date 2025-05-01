using System.Numerics;
using PigNet.Entities;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemCamera
{
	public ItemCamera()
	{
		Edu = true;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		BlockCoordinates coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
		var entity = new Camera(world) {KnownPosition = coordinates};
		entity.SpawnEntity();
		return true;
	}
}