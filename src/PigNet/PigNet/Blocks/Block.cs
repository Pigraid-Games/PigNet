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

		public float Hardness { get; protected set; }
		public float BlastResistance { get; protected set; }
		public short FuelEfficiency { get; protected set; }
		public float FrictionFactor { get; protected set; } = 0.6f;
		public int LightLevel { get; set; }

		public bool IsReplaceable { get; protected set; }
		public bool IsSolid { get; protected set; } = true;
		public bool IsBuildable { get; protected set; } = true;
		public bool IsTransparent { get; protected set; }
		public bool IsFlammable { get; protected set; }
		public bool IsBlockingSkylight { get; protected set; } = true;

		public bool Edu { get; protected set; }

		public byte BlockLight { get; set; }
		public byte SkyLight { get; set; }

		public byte BiomeId { get; set; }

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
			BoundingBox playerBbox = (player.GetBoundingBox() - 0.01f);
			BoundingBox blockBbox = GetBoundingBox();
			if (!playerBbox.Intersects(blockBbox)) return world.GetBlock(blockCoordinates).IsReplaceable;
			Log.Debug($"Player bbox={playerBbox}, block bbox={blockBbox}, intersects={playerBbox.Intersects(blockBbox)}");
			Log.Debug("Can't build where you are standing");
			return false;

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
			// No default placement. Return unhandled.
			return false;
		}

		public virtual void BlockAdded(Level level)
		{
		}

		public virtual bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			// No default interaction. Return unhandled.
			return false;
		}

		public virtual void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
		}

		public virtual void OnTick(Level level, bool isRandom)
		{
		}

		public virtual void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
		}

		public float GetHardness()
		{
			return Hardness / 5.0F;
		}

		//public double GetMineTime(Item miningTool)
		//{
		//	int multiplier = (int) miningTool.ItemMaterial;
		//	return Hardness*(1.5*multiplier);
		//}

		protected BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
		{
			switch (face)
			{
				case BlockFace.Down:
					return target + Level.Down;
				case BlockFace.Up:
					return target + Level.Up;
				case BlockFace.North:
					return target + Level.North;
				case BlockFace.South:
					return target + Level.South;
				case BlockFace.West:
					return target + Level.West;
				case BlockFace.East:
					return target + Level.East;
				default:
					return target;
			}
		}

		public virtual Item[] GetDrops(Level world, Item tool)
		{
			Item item = GetItem(world);
			return item == null ? [] : [item];
		}

		public virtual Item GetSmelt(string block)
		{
			return null;
		}

		public virtual float GetExperiencePoints()
		{
			return 0;
		}

		public virtual void DoPhysics(Level level)
		{
		}

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
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is Block block && Equals(block);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return $"{base.ToString()}, Coordinates: {Coordinates}";
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateAttribute : Attribute;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateBitAttribute : StateAttribute;


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateRangeAttribute(int minimum, int maximum) : StateAttribute
	{
		public int Minimum { get; } = minimum;
		public int Maximum { get; } = maximum;
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateEnumAttribute : StateAttribute
	{
		// ReSharper disable once UnusedParameter.Local
		public StateEnumAttribute(params string[] validValues)
		{
		}
	}