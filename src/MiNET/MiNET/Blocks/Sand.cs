#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using log4net;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks;

public partial class Sand : Block
{
	private const int TickRate = 1;
	private static readonly ILog Log = LogManager.GetLogger(typeof(Sand));

	public Sand() : base(12)
	{
		BlastResistance = 2.5f;
		Hardness = 0.5f;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		world.ScheduleBlockTick(this, TickRate);
		return false;
	}

	public override void BlockUpdate(Level world, BlockCoordinates blockCoordinates)
	{
		world.ScheduleBlockTick(this, TickRate);
	}

	public override void DoPhysics(Level level)
	{
		level.ScheduleBlockTick(this, TickRate);
	}

	public override void OnTick(Level level, bool isRandom)
	{
		if (isRandom) return;

		if (level.GetBlock(Coordinates + Level.Down).IsSolid) return;
		level.SetAir(Coordinates);

		BoundingBox bbox = GetBoundingBox();
		Vector3 d = (bbox.Max - bbox.Min) / 2;

		new FallingBlock(level, GetRuntimeId()) { KnownPosition = new PlayerLocation(Coordinates.X + d.X, Coordinates.Y - 0.03f, Coordinates.Z + d.Z) }.SpawnEntity();
	}

	public override Item GetSmelt()
	{
		return ItemFactory.GetItem(20);
	}
}