using System.Numerics;
using log4net;
using PigNet.Items;
using PigNet.Items.Tools;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Barrel
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ChestBase));

	public Barrel()
	{
		FuelEfficiency = 15;
		BlastResistance = 12.5f;
		Hardness = 2.5f;
	}

	public static bool IsBestTool(Item item)
	{
		return item is ItemAxeBase;
	}


	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return false;
	}


	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.BlockBarrelOpen);
		return true;
	}
}