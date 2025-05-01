using System;
using System.Collections.Generic;

namespace PigNet.Net;

public class AbilityLayers : List<AbilityLayer>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.Write((byte) Count);

		foreach (AbilityLayer layer in this) layer.Write(packet);
	}

	public static AbilityLayers Read(Packet packet)
	{
		var layers = new AbilityLayers();

		byte count = packet.ReadByte();
		for (int i = 0; i < count; i++) layers.Add(AbilityLayer.Read(packet));

		return layers;
	}
}

public class AbilityLayer : IPacketDataObject
{
	public const int AbilityCount = 20;

	public AbilityLayerType Type { get; set; }

	public Dictionary<PlayerAbility, bool> Abilities { get; set; }

	public float FlySpeed { get; set; }

	public float VerticalFlySpeed { get; set; }

	public float WalkSpeed { get; set; }

	public void Write(Packet packet)
	{
		packet.Write((ushort) Type);

		PlayerAbility values = PlayerAbility.None;
		PlayerAbility abilities = PlayerAbility.None;

		foreach (KeyValuePair<PlayerAbility, bool> ability in Abilities)
		{
			abilities |= ability.Key;
			values |= ability.Value ? ability.Key : 0;
		}

		if (FlySpeed > 0) abilities |= PlayerAbility.FlySpeed;
		if (VerticalFlySpeed > 0) abilities |= PlayerAbility.AbilityVerticalFlySpeed;
		if (WalkSpeed > 0) abilities |= PlayerAbility.WalkSpeed;

		packet.Write((uint) abilities);
		packet.Write((uint) values);
		packet.Write(FlySpeed);
		packet.Write(VerticalFlySpeed);
		packet.Write(WalkSpeed);
	}

	public static AbilityLayer Read(Packet packet)
	{
		var type = (AbilityLayerType) packet.ReadUshort();
		var abilities = (PlayerAbility) packet.ReadUint();
		var values = (PlayerAbility) packet.ReadUint();

		var abilityValues = new Dictionary<PlayerAbility, bool>(AbilityCount);
		for (int i = 0; i < AbilityCount; i++)
		{
			var ability = (PlayerAbility) (1 << i);
			if (ability == PlayerAbility.FlySpeed
				|| ability == PlayerAbility.AbilityVerticalFlySpeed
				|| ability == PlayerAbility.WalkSpeed)
				continue;

			if (abilities.HasFlag(ability)) abilityValues.Add(ability, values.HasFlag(ability));
		}

		return new AbilityLayer
		{
			Type = type,
			Abilities = abilityValues,
			FlySpeed = packet.ReadFloat(),
			VerticalFlySpeed = packet.ReadFloat(),
			WalkSpeed = packet.ReadFloat()
		};
	}
}

public enum AbilityLayerType
{
	CustomCache = 0,
	Base = 1,
	Spectator = 2,
	Commands = 3,
	Editor = 4,
	LoadingScreen = 5
}

[Flags]
public enum PlayerAbility : uint
{
	None = 0,

	Build = 1 << 0,
	Mine = 1 << 1,
	DoorsAndSwitches = 1 << 2,
	OpenContainers = 1 << 3,
	AttackPlayers = 1 << 4,
	AttackMobs = 1 << 5,
	OperatorCommands = 1 << 6,
	Teleport = 1 << 7,
	Invulnerable = 1 << 8,
	Flying = 1 << 9,
	MayFly = 1 << 10,
	InstantBuild = 1 << 11,
	Lightning = 1 << 12,
	FlySpeed = 1 << 13,
	WalkSpeed = 1 << 14,
	Muted = 1 << 15,
	WorldBuilder = 1 << 16,
	NoClip = 1 << 17,
	PrivilegedBuilder = 1 << 18,
	AbilityVerticalFlySpeed = 1 << 19
}