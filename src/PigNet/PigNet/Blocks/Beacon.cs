using System.Numerics;
using PigNet.Net;
using PigNet.BlockEntities;
using PigNet.Inventories;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Beacon
{
	public Beacon()
	{
		LightLevel = 15;
		BlastResistance = 15;
		Hardness = 3;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		var blockEntity = new BeaconBlockEntity
		{
			Coordinates = Coordinates
		};
		world.SetBlockEntity(blockEntity);
		return false;
	}

	private void BuildPyramidLevels(Level level, int levels)
	{
		for (int i = 1; i < levels + 1; i++)
		{
			for (int x = -i; x < i + 1; x++)
			{
				for (int z = -i; z < i + 1; z++)
				{
					level.SetBlock(new IronBlock {Coordinates = Coordinates + new BlockCoordinates(x, -i, z)});
				}
			}
		}
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		new Inventory(Coordinates, WindowType.Beacon).Open(player);
		return true;
	}
}