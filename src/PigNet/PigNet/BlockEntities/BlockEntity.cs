using System;
using System.Collections.Generic;
using fNbt;
using fNbt.Serialization;
using log4net;
using PigNet.Items;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils.Nbt;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.BlockEntities;

[NbtObject]
public abstract class BlockEntity(string id) : ICloneable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(BlockEntity));

	[NbtProperty("id")] public string Id { get; } = id;

	public string CustomName { get; set; }

	[NbtProperty("isMovable")] public bool IsMovable { get; set; }

	[NbtFlatProperty(typeof(NbtLowerCaseNamingStrategy))]
	public BlockCoordinates Coordinates { get; set; }

	[NbtIgnore] public bool UpdatesOnTick { get; set; }

	public virtual object Clone()
	{
		// Slow, but common solution. Recommended to implement real clone.

		Type type = GetType();
		object clone = Activator.CreateInstance(type);

		var settings = new NbtSerializerSettings { Flavor = NbtFlavor.BedrockNoVarInt };
		return NbtConvert.FromNbt(clone, NbtConvert.ToNbt(this, settings), settings);
	}

	public virtual NbtCompound GetCompound()
	{
		return NbtConvert.ToNbt<NbtCompound>(this);
	}

	public virtual void SetCompound(NbtCompound compound)
	{
		NbtConvert.FromNbt(this, compound);
	}

	public virtual void OnTick(Level level)
	{
	}

	public virtual void SendData(Player player)
	{
		NbtCompound tag = GetCompound();
		var nbt = new Nbt { NbtFile = new NbtFile(tag) { Flavor = NbtFlavor.Bedrock } };

		if (Log.IsDebugEnabled) Log.Debug($"Nbt: {nbt.NbtFile.RootTag}");

		McpeBlockActorData entityData = McpeBlockActorData.CreateObject();
		entityData.actorDataTags = nbt;
		entityData.blockPosition = Coordinates;
		player.SendPacket(entityData);
	}

	public virtual void SendData(Level level)
	{
		NbtCompound tag = GetCompound();
		var nbt = new Nbt { NbtFile = new NbtFile(tag) { Flavor = NbtFlavor.Bedrock } };

		if (Log.IsDebugEnabled) Log.Debug($"Nbt: {nbt.NbtFile.RootTag}");

		McpeBlockActorData entityData = McpeBlockActorData.CreateObject();
		entityData.actorDataTags = nbt;
		entityData.blockPosition = Coordinates;
		level.RelayBroadcast(entityData);
	}

	public virtual void RemoveBlockEntity(Level level)
	{
	}

	public virtual List<Item> GetDrops()
	{
		return [];
	}
}