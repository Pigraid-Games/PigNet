﻿#region LICENSE

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

using System;
using System.Numerics;
using System.Text;
using JetBrains.Annotations;
using log4net;
using PigNet.Items;
using PigNet.Net;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;
using PigNet.Utils.Metadata;
using PigNet.Utils.Skins;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Entities;

public class PlayerMob : Mob
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerMob));

	public UUID ClientUuid { get; private set; }
	public Skin Skin { get; set; }

	public Item ItemInHand { get; set; }

	public PlayerMob(string name, Level level) : base(EntityType.Player, level)
	{
		ClientUuid = new UUID(Guid.NewGuid().ToByteArray());

		Width = 0.6;
		Length = 0.6;
		Height = 1.80;

		IsSpawned = false;

		NameTag = name;


		var resourcePatch = new SkinResourcePatch() { Geometry = new GeometryIdentifier() {Default = "geometry.humanoid.customSlim" } };
		Skin = new Skin
		{
			SkinId = $"{Guid.NewGuid().ToString()}.CustomSlim",
			SkinResourcePatch = resourcePatch,
			Slim = true,
			Height = 32,
			Width = 64,
			Data = Encoding.Default.GetBytes(new string('Z', 8192)),
		};

		ItemInHand = new ItemAir();

		HideNameTag = false;
		IsAlwaysShowName = true;

		IsInWater = true;
		NoAi = true;
		HealthManager.IsOnFire = false;
		Velocity = Vector3.Zero;
		PositionOffset = 1.62f;
		if (EntityId == -1)
		{
			EntityId = DateTime.UtcNow.Ticks;
		}
	}

	[Wired]
	public void SetPosition(PlayerLocation position, bool teleport = true)
	{
		KnownPosition = position;
		LastUpdatedTime = DateTime.UtcNow;

		var package = McpeMovePlayer.CreateObject();
		package.playerRuntimeId = EntityId;
		package.x = position.X;
		package.y = position.Y + 1.62f;
		package.z = position.Z;
		package.yaw = position.HeadYaw;
		package.headYaw = position.Yaw;
		package.pitch = position.Pitch;
		package.mode = (PositionMode) (teleport ? 1 : 0);

		Level.RelayBroadcast(package);
	}

	public override MetadataDictionary GetMetadata()
	{
		var metadata = base.GetMetadata();

		return metadata;
	}

	private AbilityLayers GetAbilities()
	{
		var layers = new AbilityLayers();

		var baseLayer = new AbilityLayer()
		{
			Type = AbilityLayerType.Base,
			Abilities = PlayerAbility.All,
			Values = 0,
			FlySpeed = 0,
			WalkSpeed = 0
		};

		layers.Add(baseLayer);

		return layers;
	}

	public virtual void SendSkin([CanBeNull] Player[] players = null)
	{
		McpePlayerSkin playerSkin = McpePlayerSkin.CreateObject();
		playerSkin.uuid = ClientUuid;
		playerSkin.skin = Skin;
		playerSkin.oldSkinName = "";
		playerSkin.skinName = "";
		playerSkin.isVerified = true;
			
		if(players != null && players.Length != 0) Level.RelayBroadcast(players, playerSkin);
		else Level.RelayBroadcast(playerSkin);
	}

	public override void SpawnToPlayers(Player[] players)
	{
		var message = McpeAddPlayer.CreateObject();
		message.uuid = ClientUuid;
		message.username = NameTag;
		message.entityIdSelf = EntityId;
		message.runtimeEntityId = EntityId;
		message.x = KnownPosition.X;
		message.y = KnownPosition.Y;
		message.z = KnownPosition.Z;
		message.yaw = KnownPosition.Yaw;
		message.headYaw = KnownPosition.HeadYaw;
		message.pitch = KnownPosition.Pitch;
		message.metadata = GetMetadata();
		message.layers = GetAbilities();
		Level.RelayBroadcast(players, message);

		var mobEquipment = McpeMobEquipment.CreateObject();
		mobEquipment.runtimeActorId = EntityId;
		mobEquipment.item = ItemInHand;
		mobEquipment.slot = 0;
		Level.RelayBroadcast(players, mobEquipment);

		var armorEquipment = McpeMobArmorEquipment.CreateObject();
		armorEquipment.runtimeActorId = EntityId;
		armorEquipment.helmet = Helmet;
		armorEquipment.chestplate = Chest;
		armorEquipment.leggings = Leggings;
		armorEquipment.boots = Boots;
		Level.RelayBroadcast(players, armorEquipment);

		var setEntityData = McpeSetActorData.CreateObject();
		setEntityData.runtimeActorId = EntityId;
		setEntityData.metadata = GetMetadata();
		Level?.RelayBroadcast(players, setEntityData);
	}

	public void RemoveFromPlayerList()
	{
		var fake = new Player(null, null)
		{
			ClientUuid = ClientUuid,
			EntityId = EntityId,
			NameTag = NameTag,
			Skin = Skin
		};

		var players = Level.GetSpawnedPlayers();

		var playerList = McpePlayerList.CreateObject();
		playerList.records = new PlayerRemoveRecords {fake};
		Level.RelayBroadcast(players, Level.CreateMcpeBatch(playerList.Encode()));
		playerList.records = null;
		playerList.PutPool();
	}

	public void AddToPlayerList()
	{
		Player fake = new Player(null, null)
		{
			ClientUuid = ClientUuid,
			EntityId = EntityId,
			NameTag = NameTag,
			Skin = Skin,
			PlayerInfo = new PlayerInfo()
		};

		var players = Level.GetSpawnedPlayers();

		McpePlayerList playerList = McpePlayerList.CreateObject();
		playerList.records = new PlayerAddRecords {fake};
		Level.RelayBroadcast(players, Level.CreateMcpeBatch(playerList.Encode()));
		playerList.records = null;
		playerList.PutPool();
	}

	public override void DespawnFromPlayers(Player[] players)
	{
		{
			var fake = new Player(null, null)
			{
				ClientUuid = ClientUuid,
				EntityId = EntityId,
				NameTag = NameTag,
				Skin = Skin
			};

			McpePlayerList playerList = McpePlayerList.CreateObject();
			playerList.records = new PlayerRemoveRecords {fake};
			Level.RelayBroadcast(players, Level.CreateMcpeBatch(playerList.Encode()));
			playerList.records = null;
			playerList.PutPool();
		}

		McpeRemoveActor mcpeRemovePlayer = McpeRemoveActor.CreateObject();
		mcpeRemovePlayer.entityIdSelf = EntityId;
		Level.RelayBroadcast(players, mcpeRemovePlayer);
	}

	public override void OnTick(Entity[] entities)
	{
		OnTicking(new PlayerEventArgs(null));

		// Do nothing of the mob stuff

		OnTicked(new PlayerEventArgs(null));
	}

	public event EventHandler<PlayerEventArgs> Ticking;

	protected virtual void OnTicking(PlayerEventArgs e)
	{
		Ticking?.Invoke(this, e);
	}

	public event EventHandler<PlayerEventArgs> Ticked;

	protected virtual void OnTicked(PlayerEventArgs e)
	{
		Ticked?.Invoke(this, e);
	}


	public virtual void SendEquipment(Player[] players = null)
	{
		McpeMobEquipment mobEquipment = McpeMobEquipment.CreateObject();
		mobEquipment.runtimeActorId = EntityId;
		mobEquipment.item = ItemInHand;
		mobEquipment.slot = 0;
		if(players == null) Level.RelayBroadcast(mobEquipment);
		else Level.RelayBroadcast(players, mobEquipment);
	}

	public virtual void SendArmor(Player[] players = null)
	{
		McpeMobArmorEquipment armorEquipment = McpeMobArmorEquipment.CreateObject();
		armorEquipment.runtimeActorId = EntityId;
		armorEquipment.helmet = Helmet;
		armorEquipment.chestplate = Chest;
		armorEquipment.leggings = Leggings;
		armorEquipment.boots = Boots;
		if(players == null) Level.RelayBroadcast(armorEquipment);
		else Level.RelayBroadcast(players, armorEquipment);
	}
}