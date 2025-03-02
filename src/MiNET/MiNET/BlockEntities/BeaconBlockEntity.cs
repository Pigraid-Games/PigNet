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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using fNbt;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BlockEntities;

public class BeaconBlockEntity : BlockEntity
{
	private NbtCompound Compound { get; set; }

	public int Primary { get; set; } = 1;
	public int Secondary { get; set; } = 10;

	public BeaconBlockEntity() : base("Beacon")
	{
		UpdatesOnTick = true;

		Compound = new NbtCompound(string.Empty)
		{
			new NbtString("id", Id),
			new NbtInt("primary", Primary),
			new NbtInt("secondary", Secondary),
			new NbtInt("x", Coordinates.X),
			new NbtInt("y", Coordinates.Y),
			new NbtInt("z", Coordinates.Z),
		};
	}

	public override NbtCompound GetCompound()
	{
		Compound["x"] = new NbtInt("x", Coordinates.X);
		Compound["y"] = new NbtInt("y", Coordinates.Y);
		Compound["z"] = new NbtInt("z", Coordinates.Z);
		Compound["primary"] = new NbtInt("primary", Primary);
		Compound["secondary"] = new NbtInt("secondary", Secondary);

		return Compound;
	}

	public override void SetCompound(NbtCompound compound)
	{
		Compound = compound;

		if (compound.TryGet("primary", out NbtInt primary)) Primary = primary.Value;
		if (compound.TryGet("secondary", out NbtInt secondary)) Secondary = secondary.Value;

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

		var prim = (EffectType) Primary;
		var sec = (EffectType) Secondary;

		Effect effectPrim = GetEffect(prim);

		if (effectPrim == null || pyramidLevels <= 0) return;
		effectPrim.Level = pyramidLevels == 4 && prim == sec ? 1 : 0;
		effectPrim.Duration = duration;
		effectPrim.Particles = true;

		IEnumerable<KeyValuePair<long, Player>> players = level.Players.Where(player => 
			player.Value.IsSpawned && Vector3.Distance(Coordinates, player.Value.KnownPosition) <= range);
		
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
			if (level.IsBlock(Coordinates + (BlockCoordinates.Up * y), 7)) continue;

			return false;
		}

		return true;
	}

	private static Effect GetEffect(EffectType prim)
	{
		Effect eff = prim switch
		{
			EffectType.Speed => new Speed(),
			EffectType.Slowness => new Slowness(),
			EffectType.Haste => new Haste(),
			EffectType.MiningFatigue => new MiningFatigue(),
			EffectType.Strength => new Strength(),
			EffectType.InstantHealth => new InstantHealth(),
			EffectType.InstantDamage => new InstantDamage(),
			EffectType.JumpBoost => new JumpBoost(),
			EffectType.Nausea => new Nausea(),
			EffectType.Regeneration => new Regeneration(),
			EffectType.Resistance => new Resistance(),
			EffectType.FireResistance => new FireResistance(),
			EffectType.WaterBreathing => new WaterBreathing(),
			EffectType.Invisibility => new Invisibility(),
			EffectType.Blindness => new Blindness(),
			EffectType.NightVision => new NightVision(),
			EffectType.Hunger => new Hunger(),
			EffectType.Weakness => new Weakness(),
			EffectType.Poison => new Poison(),
			EffectType.Wither => new Wither(),
			EffectType.HealthBoost => new HealthBoost(),
			EffectType.Absorption => new Absorption(),
			EffectType.Saturation => new Saturation(),
			_ => null
		};
		return eff;
	}

	private int GetPyramidLevels(Level level)
	{
		for (int i = 1; i < 5; i++)
		for (int x = -i; x < i + 1; x++)
		for (int z = -i; z < i + 1; z++)
		{
			Block block = level.GetBlock(Coordinates + new BlockCoordinates(x, -i, z));
			if (block.Id is 42 or 41 or 57 or 133) continue;

			return i - 1;
		}

		return 4;
	}
}