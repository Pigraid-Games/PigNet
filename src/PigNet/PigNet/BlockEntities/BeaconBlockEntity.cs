using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using fNbt;
using fNbt.Serialization;
using PigNet.Blocks;
using PigNet.Effects;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.BlockEntities;

	public class BeaconBlockEntity : BlockEntity
	{
		[NbtProperty("primary")]
		public EffectType Primary { get; set; } = EffectType.Speed;
		[NbtProperty("secondary")]
		public EffectType Secondary { get; set; } = EffectType.Regeneration;

		public BeaconBlockEntity() : base(BlockEntityIds.Beacon)
		{
			UpdatesOnTick = true;
		}

		public override void SetCompound(NbtCompound compound)
		{
			base.SetCompound(compound);
			_nextUpdate = 0;
		}

		private long _nextUpdate;

		public override void OnTick(Level level)
		{
			if (_nextUpdate > level.TickTime) return;

			_nextUpdate = level.TickTime + 80;

			if (!HaveSkyLight(level)) return;

			int pyramidLevels = GetPyramidLevels(level);

			int duration = 180 + pyramidLevels * 40;
			int range = 10 + pyramidLevels * 10;

			EffectType prim = Primary;
			EffectType sec = Secondary;

			Effect effectPrim = GetEffect(prim);

			if (effectPrim == null || pyramidLevels <= 0) return;
			effectPrim.Level = pyramidLevels == 4 && prim == sec ? 1 : 0;
			effectPrim.Duration = duration;
			effectPrim.Particles = true;

			IEnumerable<KeyValuePair<long, Player>> players = level.Players.Where(player => player.Value.IsSpawned && Vector3.Distance(Coordinates, player.Value.KnownPosition) <= range);
			foreach (KeyValuePair<long, Player> player in players)
			{
				player.Value.SetEffect(effectPrim, true);

				if (pyramidLevels != 4 || prim == sec) continue;
				var regen = new Regeneration
				{
					Level = 0,
					Duration = duration,
					Particles = true
				};

				player.Value.SetEffect(regen);
			}
		}

		private bool HaveSkyLight(Level level)
		{
			int height = level.GetHeight(Coordinates);

			if (height == Coordinates.Y + 1) return true;

			for (int y = 1; y < height - Coordinates.Y; y++)
			{
				if (level.IsTransparent(Coordinates + (BlockCoordinates.Up * y))) continue;
				if (level.IsBlock<Bedrock>(Coordinates + (BlockCoordinates.Up * y))) continue;

				return false;
			}

			return true;
		}

		private static Effect GetEffect(EffectType prim)
		{
			Effect eff = null;
			switch (prim)
			{
				case EffectType.Speed:
					eff = new Speed();
					break;
				case EffectType.Slowness:
					eff = new Slowness();
					break;
				case EffectType.Haste:
					eff = new Haste();
					break;
				case EffectType.MiningFatigue:
					eff = new MiningFatigue();
					break;
				case EffectType.Strength:
					eff = new Strength();
					break;
				case EffectType.InstantHealth:
					eff = new InstantHealth();
					break;
				case EffectType.InstantDamage:
					eff = new InstantDamage();
					break;
				case EffectType.JumpBoost:
					eff = new JumpBoost();
					break;
				case EffectType.Nausea:
					eff = new Nausea();
					break;
				case EffectType.Regeneration:
					eff = new Regeneration();
					break;
				case EffectType.Resistance:
					eff = new Resistance();
					break;
				case EffectType.FireResistance:
					eff = new FireResistance();
					break;
				case EffectType.WaterBreathing:
					eff = new WaterBreathing();
					break;
				case EffectType.Invisibility:
					eff = new Invisibility();
					break;
				case EffectType.Blindness:
					eff = new Blindness();
					break;
				case EffectType.NightVision:
					eff = new NightVision();
					break;
				case EffectType.Hunger:
					eff = new Hunger();
					break;
				case EffectType.Weakness:
					eff = new Weakness();
					break;
				case EffectType.Poison:
					eff = new Poison();
					break;
				case EffectType.Wither:
					eff = new Wither();
					break;
				case EffectType.HealthBoost:
					eff = new HealthBoost();
					break;
				case EffectType.Absorption:
					eff = new Absorption();
					break;
				case EffectType.Saturation:
					eff = new Saturation();
					break;
			}
			return eff;
		}

		private int GetPyramidLevels(Level level)
		{
			for (int i = 1; i < 5; i++)
			{
				for (int x = -i; x < i + 1; x++)
				{
					for (int z = -i; z < i + 1; z++)
					{
						Block block = level.GetBlock(Coordinates + new BlockCoordinates(x, -i, z));
						if (block is GoldBlock || block is IronBlock || block is EmeraldBlock || block is DiamondBlock || block is CopperBlock) continue;

						return i - 1;
					}
				}
			}

			return 4;
		}
	}