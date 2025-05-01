using System.Collections.Generic;
using PigNet.Net;

namespace PigNet;

public class AttributeModifiers : Dictionary<string, AttributeModifier>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.WriteLength(Count);
		foreach (AttributeModifier modifier in Values) modifier.Write(packet);
	}

	public static AttributeModifiers Read(Packet packet)
	{
		var modifiers = new AttributeModifiers();
		int count = packet.ReadLength();
		for (int i = 0; i < count; i++)
		{
			var modifier = AttributeModifier.Read(packet);
			modifiers[modifier.Name] = modifier;
		}

		return modifiers;
	}
}

public class PlayerAttributes : Dictionary<string, PlayerAttribute>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.WriteLength(Count);
		foreach (PlayerAttribute attribute in Values) attribute.Write(packet);
	}

	public static PlayerAttributes Read(Packet packet)
	{
		var attributes = new PlayerAttributes();
		int count = packet.ReadLength();
		for (int i = 0; i < count; i++)
		{
			var attribute = PlayerAttribute.Read(packet);
			attributes[attribute.Name] = attribute;
		}

		return attributes;
	}
}

public class EntityAttributes : Dictionary<string, EntityAttribute>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.WriteLength(Count);
		foreach (EntityAttribute attribute in Values) attribute.Write(packet);
	}

	public static EntityAttributes Read(Packet packet)
	{
		var attributes = new EntityAttributes();
		int count = packet.ReadLength();
		for (int i = 0; i < count; i++)
		{
			var attribute = EntityAttribute.Read(packet);
			attributes[attribute.Name] = attribute;
		}

		return attributes;
	}
}

public class EntityLink : IPacketDataObject
{
	public enum EntityLinkType : byte
	{
		Remove = 0,
		Rider = 1,
		Passenger = 2
	}

	public EntityLink(long fromEntityId, long toEntityId, EntityLinkType type, bool immediate, bool causedByRider, float vehicleAngularVelocity)
	{
		FromEntityId = fromEntityId;
		ToEntityId = toEntityId;
		Type = type;
		Immediate = immediate;
		CausedByRider = causedByRider;
		VehicleAngularVelocity = vehicleAngularVelocity;
	}

	public long FromEntityId { get; set; }

	public long ToEntityId { get; set; }

	public EntityLinkType Type { get; set; }

	public bool Immediate { get; set; }

	public bool CausedByRider { get; set; }

	public float VehicleAngularVelocity { get; set; }

	public void Write(Packet packet)
	{
		packet.WriteEntityId(FromEntityId);
		packet.WriteEntityId(ToEntityId);
		packet.Write((byte) Type);
		packet.Write(Immediate);
		packet.Write(CausedByRider);
		packet.Write(VehicleAngularVelocity);
	}

	public static EntityLink Read(Packet packet)
	{
		long fromEntityId = packet.ReadEntityId();
		long toEntityId = packet.ReadEntityId();
		var type = (EntityLinkType) packet.ReadByte();
		bool immediate = packet.ReadBool();
		bool causedByRider = packet.ReadBool();
		float vehicleAngularVelocity = packet.ReadFloat();

		return new EntityLink(fromEntityId, toEntityId, type, immediate, causedByRider, vehicleAngularVelocity);
	}
}

public class EntityLinks : List<EntityLink>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.WriteLength(Count); // LE

		foreach (EntityLink link in this) link.Write(packet);
	}

	public static EntityLinks Read(Packet packet)
	{
		int count = packet.ReadLength();

		var links = new EntityLinks();
		for (int i = 0; i < count; i++) links.Add(EntityLink.Read(packet));

		return links;
	}
}

public class GameRules : HashSet<GameRule>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.WriteVarInt(Count);
		foreach (GameRule rule in this) rule.Write(packet);
	}

	public static GameRules Read(Packet packet)
	{
		var gameRules = new GameRules();

		int count = packet.ReadVarInt();
		for (int i = 0; i < count; i++) gameRules.Add(GameRule.Read(packet));

		return gameRules;
	}
}