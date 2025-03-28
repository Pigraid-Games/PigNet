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

using System;
using JetBrains.Annotations;
using MiNET.Items;
using MiNET.Net;
using MiNET.Net.EnumerationsTable;
using MiNET.Net.Packets.Mcpe;
using MiNET.Worlds;

namespace MiNET.Entities;

public class BossBar : Entity
{
	public bool IsVisible { get; set; } = true;
	public bool Animate { get; set; } = false;
	public int Progress { get; set; } = 100;
	public int MaxProgress { get; set; } = 100;
	public string FilteredName { get; set; }
	public ushort DarkenScreen { get; set; }
	public uint Color { get; set; }
	public uint Overlay { get; set; }
	
	public class NoDamageHealthManager(Entity entity) : HealthManager(entity)
	{
		public override void TakeHit(Entity source, Item tool, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
		}

		public override void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
		}

		public override void OnTick()
		{
		}
	}

	public BossBar(Level level, [CanBeNull] string filteredName) : base(EntityType.Slime, level)
	{
		Width = 0;
		Length = 0;
		Height = 0;

		HideNameTag = true;
		IsAlwaysShowName = false;
		IsInvisible = true;
		IsSilent = true;
		HealthManager = new NoDamageHealthManager(this);
		if(filteredName == null) FilteredName = NameTag;
			
		KnownPosition = level.SpawnPoint;
	}

	[Wired]
	public virtual void SetNameTag(string nameTag, [CanBeNull] string filterName = null)
	{
		NameTag = nameTag;
		filterName ??= nameTag;
		McpeBossEvent bossEvent = McpeBossEvent.CreateObject();
		bossEvent.targetActorId = EntityId;
		bossEvent.eventData = new BossEventTypeUpdateName { Name = nameTag, FilteredName = filterName };
		bossEvent.eventType = BossEventUpdateType.UpdateName;
		Level?.RelayBroadcast(bossEvent);
	}

	[Wired]
	public virtual void SetProgress(int progress = Int32.MinValue, int maxProgress = Int32.MinValue)
	{
		if (progress != Int32.MinValue) Progress = progress;
		if (maxProgress != Int32.MinValue) MaxProgress = maxProgress;

		McpeBossEvent bossEvent = McpeBossEvent.CreateObject();
		bossEvent.targetActorId = EntityId;
		bossEvent.eventData = new BossEventTypeUpdatePercent { HealthPercent = (float) Progress / MaxProgress };
		bossEvent.eventType = BossEventUpdateType.UpdatePercent;
		Level?.RelayBroadcast(bossEvent);
	}

	public override void SpawnToPlayers(Player[] players)
	{
		base.SpawnToPlayers(players);

		McpeBossEvent bossEvent = McpeBossEvent.CreateObject();
		bossEvent.targetActorId = EntityId;

		if (IsVisible)
		{
			bossEvent.eventType = BossEventUpdateType.Add;
			bossEvent.eventData = new BossEventTypeAdd
			{
				Name = NameTag,
				FilteredName = FilteredName,
				HealthPercent = (float) Progress / MaxProgress,
				DarkenScreen = DarkenScreen,
				Color = Color,
				Overlay = Overlay
			};
		}
		else
		{
			bossEvent.eventType = BossEventUpdateType.Remove;
			bossEvent.eventData = new BossEventTypePlayerRemove();
		}
		Level?.RelayBroadcast(players, bossEvent);
	}

	public override void DespawnFromPlayers(Player[] players)
	{
		base.DespawnFromPlayers(players);

		McpeBossEvent bossEvent = McpeBossEvent.CreateObject();
		bossEvent.targetActorId = EntityId;
		bossEvent.eventType = BossEventUpdateType.Remove;
		bossEvent.eventData = new BossEventTypePlayerRemove();
		Level?.RelayBroadcast(players, bossEvent);
	}

	public override void OnTick(Entity[] entities)
	{
		base.OnTick(entities);

		if (!Animate) return;

		if (Level.TickTime % 2 != 0) return;
		if (Progress > MaxProgress) Progress = 0;
		SetProgress();
		Progress++;
	}
}