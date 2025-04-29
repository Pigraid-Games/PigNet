using System.Numerics;
using PigNet.Entities.World;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Sand : Block
{
	private int _tickRate = 1;

	public Sand()
	{
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		world.ScheduleBlockTick(this, _tickRate);
		return false;
	}

	public override void BlockUpdate(Level world, BlockCoordinates blockCoordinates)
	{
		world.ScheduleBlockTick(this, _tickRate);
	}

	public override void DoPhysics(Level level)
	{
		level.ScheduleBlockTick(this, _tickRate);
	}

	public override void OnTick(Level level, bool isRandom)
	{
		if (isRandom) return;

		if (level.GetBlock(Coordinates + Level.Down).IsSolid) return;
		level.SetAir(Coordinates);

		BoundingBox bbox = GetBoundingBox();
		Vector3 d = (bbox.Max - bbox.Min) / 2;

		new FallingBlock(level, RuntimeId) {KnownPosition = new PlayerLocation(Coordinates.X + d.X, Coordinates.Y - 0.03f, Coordinates.Z + d.Z)}.SpawnEntity();
	}
}