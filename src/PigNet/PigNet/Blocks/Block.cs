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
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer. The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2024 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using System.Numerics;
using log4net;
using PigNet.Items;
using PigNet.Particles;
using PigNet.Utils;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Blocks;

public abstract class Block : BlockStateContainer, ICloneable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Block));
	public const uint UnknownRuntimeId = uint.MaxValue;

	public BlockCoordinates Coordinates { get; set; }

	public float Hardness { get; protected set; } = 0;
	public float BlastResistance { get; protected set; } = 0;
	public short FuelEfficiency { get; protected set; } = 0;
	public float FrictionFactor { get; protected set; } = 0.6f;
	public int LightLevel { get; set; } = 0;

	public bool IsReplaceable { get; protected set; } = false;
	public bool IsSolid { get; protected set; } = true;
	public bool IsBuildable { get; protected set; } = true;
	public bool IsTransparent { get; protected set; } = false;
	public bool IsFlammable { get; protected set; } = false;
	public bool IsBlockingSkylight { get; protected set; } = true;
	public bool Edu { get; protected set; } = false;

	public byte BlockLight { get; set; }
	public byte SkyLight { get; set; }
	public byte BiomeId { get; set; }

	protected Block() { }

	public virtual Item GetItem(Level world, bool blockItem = false)
	{
		return ItemFactory.GetItem(Id);
	}

	public bool CanPlace(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face)
	{
		return CanPlace(world, player, Coordinates, targetCoordinates, face);
	}

	protected virtual bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		var playerBox = player.GetBoundingBox() - 0.01f;
		var blockBox = GetBoundingBox();
		if (playerBox.Intersects(blockBox))
		{
			Log.Debug($"Player bbox={playerBox}, block bbox={blockBox}, intersects={playerBox.Intersects(blockBox)}");
			Log.Debug("Can't build where you are standing");
			return false;
		}

		return world.GetBlock(blockCoordinates).IsReplaceable;
	}

	public virtual void BreakBlock(Level world, BlockFace face, bool silent = false)
	{
		world.SetAir(Coordinates);

		if (!silent)
		{
			var particle = new DestroyBlockParticle(world, this);
			particle.Spawn();
		}

		UpdateBlocks(world);
	}

	protected void UpdateBlocks(Level world)
	{
		world.GetBlock(Coordinates.BlockUp()).BlockUpdate(world, Coordinates);
		world.GetBlock(Coordinates.BlockDown()).BlockUpdate(world, Coordinates);
		world.GetBlock(Coordinates.BlockWest()).BlockUpdate(world, Coordinates);
		world.GetBlock(Coordinates.BlockEast()).BlockUpdate(world, Coordinates);
		world.GetBlock(Coordinates.BlockSouth()).BlockUpdate(world, Coordinates);
		world.GetBlock(Coordinates.BlockNorth()).BlockUpdate(world, Coordinates);
	}

	public virtual bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
	{
		return false; // No default placement
	}

	public virtual void BlockAdded(Level level) { }

	public virtual bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		return false; // No default interaction
	}

	public virtual void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face) { }

	public virtual void OnTick(Level level, bool isRandom) { }

	public virtual void BlockUpdate(Level level, BlockCoordinates blockCoordinates) { }

	public float GetHardness()
	{
		return Hardness / 5.0f;
	}

	protected BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
	{
		return face switch
		{
			BlockFace.Down => target + Level.Down,
			BlockFace.Up => target + Level.Up,
			BlockFace.North => target + Level.North,
			BlockFace.South => target + Level.South,
			BlockFace.West => target + Level.West,
			BlockFace.East => target + Level.East,
			_ => target
		};
	}

	public virtual Item[] GetDrops(Level world, Item tool)
	{
		var item = GetItem(world);
		return item != null ? new[] { item } : Array.Empty<Item>();
	}

	public virtual Item GetSmelt(string block)
	{
		return null;
	}

	public virtual float GetExperiencePoints()
	{
		return 0;
	}

	public virtual void DoPhysics(Level level) { }

	public virtual BoundingBox GetBoundingBox()
	{
		return new BoundingBox(Coordinates, Coordinates + 1);
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	protected virtual bool Equals(Block other)
	{
		return RuntimeId == other.RuntimeId;
	}

	public override bool Equals(object obj)
	{
		return obj is Block other && Equals(other);
	}

	public override int GetHashCode()
	{
		return RuntimeId.GetHashCode();
	}

	public override string ToString()
	{
		return $"{GetType().Name} (RuntimeId={RuntimeId}, Coordinates={Coordinates})";
	}
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class StateAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class StateBitAttribute : StateAttribute { }

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class StateRangeAttribute : StateAttribute
{
	public int Minimum { get; }
	public int Maximum { get; }

	public StateRangeAttribute(int minimum, int maximum)
	{
		Minimum = minimum;
		Maximum = maximum;
	}
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class StateEnumAttribute : StateAttribute
{
	public StateEnumAttribute(params string[] validValues) { }
}