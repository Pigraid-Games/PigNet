#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using PigNet.Entities;
using PigNet.Items;
using PigNet.Items.Tools;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public class TrapdoorBase : Block
{
	protected TrapdoorBase(int id) : base(id)
	{
		IsTransparent = true;
		BlastResistance = 15;
		Hardness = 5;
	}

	[StateRange(0, 3)] public virtual int Direction { get; set; }
	[StateBit] public virtual bool OpenBit { get; set; }
	[StateBit] public virtual bool UpsideDownBit { get; set; }

	public override bool IsBestTool(Item item)
	{
		if (this is IronTrapdoor) return item is ItemPickaxe ? true : false;
		return item is ItemAxe ? true : false;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Direction = Entity.DirectionByRotationFlat(player.KnownPosition.Yaw) switch
		{
			0 => 1, // East
			1 => 3, // South
			2 => 0, // West
			3 => 2, // North 
			_ => 0
		};

		UpsideDownBit = (faceCoords.Y > 0.5 && face != BlockFace.Up) || face == BlockFace.Down;

		return false;
	}

	public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		var sound = new Sound((short) LevelEventType.SoundOpenDoor, blockCoordinates);
		sound.Spawn(world);
		OpenBit = !OpenBit;
		world.SetBlock(this);

		return true;
	}
}

public partial class Trapdoor : TrapdoorBase
{
	public Trapdoor() : base(96) { }
}

public partial class AcaciaTrapdoor : TrapdoorBase
{
	public AcaciaTrapdoor() : base(400) { }
}

public partial class BirchTrapdoor : TrapdoorBase
{
	public BirchTrapdoor() : base(401) { }
}

public partial class DarkOakTrapdoor : TrapdoorBase
{
	public DarkOakTrapdoor() : base(402) { }
}

public partial class JungleTrapdoor : TrapdoorBase
{
	public JungleTrapdoor() : base(403) { }
}

public partial class SpruceTrapdoor : TrapdoorBase
{
	public SpruceTrapdoor() : base(404) { }
}