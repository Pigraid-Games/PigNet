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

using System;
using System.Numerics;
using MiNET.Items;
using MiNET.Particles;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks;

public partial class Sapling : Block
{
	public Sapling() : base(6)
	{
		FuelEfficiency = 5;
		BlastResistance = 0;
		IsTransparent = true;
		IsFlammable = true;
	}

	protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
	{
		if (!base.CanPlace(world, player, blockCoordinates, targetCoordinates, face)) return false;
		Block under = world.GetBlock(Coordinates.BlockDown());
		return under is Dirt or Podzol or Grass;
	}

	public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
	{
		Item itemInHand = player.Inventory.GetItemInHand();
		SaplingType = itemInHand.Metadata switch
		{
			0 => "oak",
			1 => "spruce",
			2 => "birch",
			3 => "jungle",
			4 => "acacia",
			5 => "dark_oak",
			_ => throw new ArgumentOutOfRangeException()
		};
		return false;
	}

	public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
	{
		if (player.Inventory.GetItemInHand() is not ItemDye { Metadata: 15 }) return false;
		var random = new Random();
		for (int i = 0; i < 3; i++)
		{
			var particle = new LegacyParticle((int) ParticleType.VillagerHappy, level)
			{
				Position = (Vector3) Coordinates
							+ (new Vector3(0.5f, 0.5f, 0.5f)
								+ new Vector3((float) (random.NextDouble() - 0.5D), (float) (random.NextDouble() - 0.5D), (float) (random.NextDouble() - 0.5D)))
			};
			particle.Spawn();
		}

		if (!(random.NextDouble() < 0.45)) return false;
		OnTick(level, true);
		return true;
	}

	//[StateBit] public bool AgeBit { get; set; } = false;
	//[StateEnum("oak", "spruce", "birch", "jungle", "acacia", "dark_oak")]
	//public string SaplingType { get; set; } = "oak";

	public override void OnTick(Level level, bool isRandom)
	{
		if (!isRandom) return;

		int lightLevel = level.GetSubtractedLight(Coordinates);
		if (lightLevel < 9 || new Random().Next(7) != 0) return;
		SmallTreeGenerator generator = null;
		Block log;
		Block leaves;
		switch (SaplingType)
		{
			case "oak":
				log = new Log { OldLogType = SaplingType };
				leaves = new Leaves { OldLeafType = SaplingType };
				generator = new SmallTreeGenerator(log, leaves, 4);
				break;
			case "spruce":
				log = new Log { OldLogType = SaplingType };
				leaves = new Leaves { OldLeafType = SaplingType };
				generator = new SmallTreeGenerator(log, leaves, 4);
				break;
			case "birch":
				log = new Log { OldLogType = SaplingType };
				leaves = new Leaves { OldLeafType = SaplingType };
				generator = new SmallTreeGenerator(log, leaves, 5);
				break;
			case "jungle":
				log = new Log { OldLogType = SaplingType };
				leaves = new Leaves { OldLeafType = SaplingType };
				generator = new SmallTreeGenerator(log, leaves, 4 + new Random().Next(7));
				break;
			case "acacia":
				log = new Log { OldLogType = SaplingType };
				leaves = new Leaves { OldLeafType = SaplingType };
				generator = new SmallTreeGenerator(log, leaves, 5);
				break;
			case "dark_oak":
				log = new Log { OldLogType = SaplingType };
				leaves = new Leaves { OldLeafType = SaplingType };
				generator = new SmallTreeGenerator(log, leaves, 5);
				break;
		}

		if (generator == null) return;

		level.SetAir(Coordinates);

		if (!generator.Generate(level, Coordinates)) level.SetBlock(this);
	}
}

public abstract class TreeGeneratorBase
{
	protected static bool CanGrowInto(Block material)
	{
		return material is Air or Leaves or Leaves2 or Grass or Dirt or Log or Log2 or Sapling or Vine;
	}
}

public class SmallTreeGenerator(Block log, Block leave, int minTreeHeight) : TreeGeneratorBase
{
	public bool Generate(Level level, BlockCoordinates position)
	{
		var rand = new Random();
		int height = rand.Next(3) + minTreeHeight;

		bool canGrow = true;

		if (position.Y >= 1 && position.Y + height + 1 <= 256)
		{
			for (int y = position.Y; y <= position.Y + 1 + height; ++y)
			{
				int k = 1;

				if (y == position.Y) k = 0;

				if (y >= position.Y + 1 + height - 2) k = 2;

				for (int x = position.X - k; x <= position.X + k && canGrow; ++x)
				for (int z = position.Z - k; z <= position.Z + k && canGrow; ++z)
					if (y is >= 0 and < 256)
					{
						if (!CanGrowInto(level.GetBlock(x, y, z))) canGrow = false;
					}
					else
						canGrow = false;
			}

			if (!canGrow)
			{
			}
			else
			{
				Block block = level.GetBlock(position.BlockDown());

				if (block is not (Grass or Dirt or Farmland) || position.Y >= 256 - height - 1) return false;
				level.SetBlock(new Dirt { Coordinates = position.BlockDown() });

				for (int y = position.Y - 3 + height; y <= position.Y + height; ++y)
				{
					int yd = y - (position.Y + height);
					int ydHalf = 1 - (yd / 2);

					for (int x = position.X - ydHalf; x <= position.X + ydHalf; ++x)
					{
						int xd = x - position.X;

						for (int z = position.Z - ydHalf; z <= position.Z + ydHalf; ++z)
						{
							int zd = z - position.Z;

							if (Math.Abs(xd) == ydHalf && Math.Abs(zd) == ydHalf && (rand.Next(2) == 0 || yd == 0)) continue;
							var blockpos = new BlockCoordinates(x, y, z);
							Block material = level.GetBlock(blockpos);

							if (material is not Air && material is not Leaves && material is not Leaves2) continue;
							leave.Coordinates = blockpos;
							level.SetBlock(leave);
						}
					}
				}

				for (int y = 0; y < height; ++y)
				{
					BlockCoordinates blockpos = position + (BlockCoordinates.Up * y);
					Block material = level.GetBlock(blockpos);

					if (material is not Air && material is not Leaves && material is not Leaves2) continue;
					log.Coordinates = blockpos;
					level.SetBlock(log);
				}

				return true;
			}
			return false;
		}
		return false;
	}
}