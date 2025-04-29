using System.Numerics;
using PigNet.Net;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class CraftingTable : Block
{
	public CraftingTable()
	{
		FuelEfficiency = 15;
		BlastResistance = 12.5f;
		Hardness = 2.5f;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		new Inventory(Coordinates, WindowType.Workbench).Open(player);

		return true;
	}
}