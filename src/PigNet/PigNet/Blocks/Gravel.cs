using System;
using System.Numerics;
using PigNet.Entities.World;
using PigNet.Items;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public partial class Gravel : Block
{
	private int _tickRate = 1;

	public Gravel()
	{
		BlastResistance = 3;
		Hardness = 0.6f;
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


	public override Item[] GetDrops(Level world, Item tool)
	{
		var rnd = new Random();
		return rnd.NextDouble() <= 0.1 ? [new ItemFlint()] : base.GetDrops(world, tool);
	}
}