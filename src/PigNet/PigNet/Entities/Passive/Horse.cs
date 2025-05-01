using System;
using System.Collections;
using System.Numerics;
using fNbt;
using log4net;
using PigNet.Blocks;
using PigNet.Entities.Behaviors;
using PigNet.Inventories;
using PigNet.Items;
using PigNet.Items.Extensions;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;
using PigNet.Utils.Metadata;
using PigNet.Utils.Nbt;
using PigNet.Worlds;
using ItemGoldenApple = PigNet.Items.ItemGoldenApple;

namespace PigNet.Entities.Passive;

public class Horse : PassiveMob, IRideable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Horse));

	public Horse(Level level, bool isDonkey = false, Random rnd = null) : base(isDonkey ? EntityType.Donkey : EntityType.Horse, level)
	{
		Width = Length = 1.4;
		Height = 1.6;

		Random random = rnd ?? new Random();
		Variant = random.Next(7);
		Markings = random.Next(5);
		Speed = (0.45 + (random.NextDouble() * 0.3D) + (random.NextDouble() * 0.3D) + (random.NextDouble() * 0.3)) * 0.25D;
		JumpStrength = 0.4 + (random.NextDouble() * 0.2) + (random.NextDouble() * 0.2) + (random.NextDouble() * 0.2);

		IsAffectedByGravity = true;
		//IsBreathing = true; // ??
		HasCollision = true;

		Behaviors.Add(new HorseRiddenBehavior(this));
		Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.2));
		Behaviors.Add(new HorseEatBlockBehavior(this, 100));
		Behaviors.Add(new WanderBehavior(this, 0.7));
		Behaviors.Add(new LookAtPlayerBehavior(this));
		Behaviors.Add(new RandomLookaroundBehavior(this));

		Inventory = new HorseInventory(this);
	}

	public int Markings { get; set; }
	public bool IsEating { get; set; }
	public double JumpStrength { get; set; }
	public int Temper { get; set; }
	public HorseInventory Inventory { get; set; }
	public Entity Rider { get; set; }

	public override MetadataDictionary GetMetadata()
	{
		Scale = IsBaby ? 0.5582917f : 1.0;
		MetadataDictionary metadata = base.GetMetadata();
		metadata[(int) MetadataFlags.Variant] = new MetadataInt(Variant);
		metadata[(int) MetadataFlags.MarkVariant] = new MetadataInt(Markings);
		if (IsTamed)
		{
			metadata[(int) MetadataFlags.ContainerSize] = new MetadataByte(12);
			metadata[(int) MetadataFlags.ContainerStrengthModifier] = new MetadataInt(2);
		}

		Log.Debug($"Horse: {metadata}");
		return metadata;
	}

	protected override BitArray GetFlags()
	{
		BitArray bitArray = base.GetFlags();

		bitArray[(int) DataFlags.Eating] = IsEating;

		return bitArray;
	}

	public override EntityAttributes GetEntityAttributes()
	{
		EntityAttributes attributes = base.GetEntityAttributes();
		attributes["minecraft:horse.jump_strength"] = new EntityAttribute
		{
			Name = "minecraft:horse.jump_strength",
			MinValue = 0,
			MaxValue = float.MaxValue,
			Value = (float) JumpStrength
		};

		return attributes;
	}

	public override void DoInteraction(int actionId, Player player)
	{
		if (player.IsSneaking)
		{
			Inventory.Open(player);
			return;
		}

		Item inHand = player.Inventory.GetItemInHand();
		if (inHand is ItemSugar
			|| inHand is ItemWheat
			|| inHand is ItemApple
			|| inHand is ItemGoldenCarrot
			|| inHand is ItemGoldenApple
			|| inHand.IsItemBlockOf<HayBlock>())
		{
			// Feeding

			// Increase temper
			if (inHand is ItemSugar)
			{
				Temper += 3;
				HealthManager.Regen();
			}
			else if (inHand is ItemWheat)
			{
				Temper += 3;
				HealthManager.Regen(2);
			}
			else if (inHand is ItemApple)
			{
				Temper += 3;
				HealthManager.Regen(3);
			}
			else if (inHand is ItemGoldenCarrot)
			{
				Temper += 5;
				HealthManager.Regen(4);
			}
			else if (inHand is ItemGoldenApple)
			{
				Temper += 10;
				HealthManager.Regen(10);
			}
			else if (inHand.IsItemBlockOf<HayBlock>())
				//Temper += 3;
				HealthManager.Regen(20);
		}
		else if (IsTamed && !IsSaddled && inHand is ItemSaddle)
		{
			// Saddle horse

			if (!IsSaddled)
			{
				Inventory.SetSlot(player, 0, inHand);
				player.Inventory.RemoveItems(inHand.Id, 1); // Wrong. Should really be item in hand
			}
		}
		else
		{
			// Riding

			if (!IsTamed)
			{
				var random = new Random();
				if (random.Next(100) < Temper || player.GameMode == GameMode.Creative)
				{
					// Tamed
					Temper = 100;
					IsTamed = true;
					BroadcastSetEntityData();

					McpeActorEvent actorEvent = McpeActorEvent.CreateObject();
					actorEvent.runtimeEntityId = EntityId;
					actorEvent.eventId = ActorEvent.TamingSucceeded;
					actorEvent.data = 0;
					Level.RelayBroadcast(actorEvent);
				}
				else
					Temper += 5;
			}

			Mount(player);
		}
	}

	public void SaddleHorse(bool saddle)
	{
		IsSaddled = saddle;
		IsWasdControlled = saddle;
		CanPowerJump = saddle;

		BroadcastSetEntityData();
	}

	public override void Mount(Entity rider)
	{
		if (rider is Player player)
		{
			Rider = player;
			IsRidden = true;

			player.Vehicle = EntityId;

			McpeSetActorLink link = McpeSetActorLink.CreateObject();
			link.linkType = ActorLinkType.Riding;
			link.riderId = player.EntityId;
			link.riddenId = EntityId;
			Level.RelayBroadcast(link);

			SendSetEntityData(player);
		}
	}

	public override void Unmount(Entity rider)
	{
		if (rider is Player player)
		{
			// Unmount ridden entity
			IsRiding = false;

			McpeSetActorLink link = McpeSetActorLink.CreateObject();
			link.linkType = ActorLinkType.None;
			link.riderId = player.EntityId;
			link.riddenId = EntityId;
			Level.RelayBroadcast(link);

			IsRidden = false;
			IsRearing = false;
			BroadcastSetEntityData();

			player.Vehicle = 0;
			Rider = null;

			player.BroadcastSetEntityData();
		}
	}

	public void SendSetEntityData(Player player)
	{
		player.IsRiding = true;
		player.RiderSeatPosition = new Vector3(0, 2.32001f, -0.2f);
		player.RiderRotationLocked = false;
		player.RiderMaxRotation = 181;
		player.RiderMinRotation = 0;
		player.BroadcastSetEntityData();
	}
}

public class HorseRiddenBehavior : BehaviorBase
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(HorseRiddenBehavior));

	private readonly Horse _horse;
	private long _rideTime;

	public HorseRiddenBehavior(Horse horse)
	{
		_horse = horse;
	}

	public override bool ShouldStart()
	{
		return _horse.IsRidden;
	}

	public override void OnTick(Entity[] entities)
	{
		if (!_horse.IsTamed)
		{
			//Log.Debug($"Riding untamed horse {_rideTime}");

			if (_rideTime > 100 && !_horse.IsRearing)
			{
				_horse.IsRearing = true;
				_horse.BroadcastSetEntityData();
			}
			else if (_rideTime > 120)
			{
				McpeActorEvent actorEvent = McpeActorEvent.CreateObject();
				actorEvent.runtimeEntityId = _horse.EntityId;
				actorEvent.eventId = ActorEvent.TamingFailed;
				actorEvent.data = 0;
				_horse.Level.RelayBroadcast(actorEvent);
				_horse.Unmount(_horse.Rider);
			}

			_rideTime++;
		}
		else
		{
			if (_horse.IsRearing && _horse.IsOnGround)
			{
				_horse.IsRearing = false;
				_horse.BroadcastSetEntityData();
			}
		}
	}

	public override void OnEnd()
	{
		_rideTime = 0;

		if (_horse.IsRearing)
		{
			_horse.IsRearing = false;
			_horse.BroadcastSetEntityData();
		}
	}
}

public class HorseInventory : ContainerInventory
{
	private readonly Horse _horse;

	public HorseInventory(Horse horse)
		: base(new ItemStacks(2), horse.EntityId)
	{
		_horse = horse;

		Type = WindowType.Horse;
	}

	protected override bool OnInventoryOpen(Player player, bool open)
	{
		var opened = base.OnInventoryOpen(player, open);

		if (opened)
		{
			McpeUpdateEquipment equ = McpeUpdateEquipment.CreateObject();
			equ.actorId = _horse.EntityId;
			equ.containerId = (byte) WindowId;
			equ.containerType = (byte) Type;

			equ.namedtag = new Nbt { NbtFile = new NbtFile(GetNbt()) { Flavor = NbtFlavor.Bedrock } };

			player.SendPacket(equ);
		}

		return opened;
	}

	public override void SetSlot(Player player, byte slot, Item itemStack)
	{
		switch (slot)
		{
			case 0:
				_horse.SaddleHorse(itemStack is ItemSaddle);
				break;
			case 1:
				_horse.Chest = itemStack;
				_horse.BroadcastArmor();
				break;
		}

		base.SetSlot(player, slot, itemStack);
	}

	public NbtCompound GetNbt()
	{
		// TODO - WTF?!

		var root = new NbtCompound("")
		{
			new NbtList("slots")
			{
				new NbtCompound
				{
					new NbtList("acceptedItems")
					{
						new NbtCompound
						{
							new NbtCompound("slotItem")
							{
								new NbtByte("Count", 1),
								new NbtShort("Damage", 0),
								new NbtShort("id", 329)
							}
						}
					},
					new NbtCompound("item")
					{
						new NbtByte("Count", Slots[0].Count),
						new NbtShort("Damage", Slots[0].Metadata),
						new NbtShort("id", Slots[0].LegacyId)
					},
					new NbtInt("slotNumber", 0)
				},
				new NbtCompound
				{
					new NbtList("acceptedItems")
					{
						new NbtCompound
						{
							new NbtCompound("slotItem")
							{
								new NbtByte("Count", 1),
								new NbtShort("Damage", 0),
								new NbtShort("id", 416)
							}
						},
						new NbtCompound
						{
							new NbtCompound("slotItem")
							{
								new NbtByte("Count", 1),
								new NbtShort("Damage", 0),
								new NbtShort("id", 417)
							}
						},
						new NbtCompound
						{
							new NbtCompound("slotItem")
							{
								new NbtByte("Count", 1),
								new NbtShort("Damage", 0),
								new NbtShort("id", 418)
							}
						},
						new NbtCompound
						{
							new NbtCompound("slotItem")
							{
								new NbtByte("Count", 1),
								new NbtShort("Damage", 0),
								new NbtShort("id", 419)
							}
						}
					},
					new NbtCompound("item")
					{
						new NbtByte("Count", Slots[1].Count),
						new NbtShort("Damage", Slots[1].Metadata),
						new NbtShort("id", Slots[1].LegacyId)
					},
					new NbtInt("slotNumber", 1)
				}
			}
		};

		return root;
	}
}