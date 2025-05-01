#region LICENSE

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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using PigNet.Net;
using PigNet.Net.Packets.Mcpe;

namespace PigNet.Utils;

public class ScoreEntries : List<ScoreEntry>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.Write((byte) (this.FirstOrDefault() is ScoreEntryRemove ? McpeSetScore.Types.Remove : McpeSetScore.Types.Change));
		packet.WriteLength(Count);

		foreach (ScoreEntry entry in this) packet.Write(entry);
	}

	public static ScoreEntries Read(Packet packet)
	{
		var entries = new ScoreEntries();
		byte type = packet.ReadByte();
		int count = packet.ReadLength();

		switch ((McpeSetScore.Types) type)
		{
			case McpeSetScore.Types.Remove:
				for (int i = 0; i < count; i++) entries.Add(ScoreEntryRemove.Read(packet));

				break;
			case McpeSetScore.Types.Change:
				for (int i = 0; i < count; i++)
				{
					var changeType = (McpeSetScore.ChangeTypes) packet.ReadByte();
					entries.Add(changeType switch
					{
						McpeSetScore.ChangeTypes.Player => ScoreEntryChangePlayer.Read(packet),
						McpeSetScore.ChangeTypes.Entity => ScoreEntryChangeEntity.Read(packet),
						McpeSetScore.ChangeTypes.FakePlayer => ScoreEntryChangeFakePlayer.Read(packet),

						_ => throw new Exception($"Unexpected score entry change type = [{changeType}]")
					});
				}

				break;
			default:
				throw new Exception($"Unexpected score entry type = [{type}]");
		}

		return entries;
	}
}

public abstract class ScoreEntry : IPacketDataObject
{
	public long Id { get; set; }

	public string ObjectiveName { get; set; }

	public uint Score { get; set; }

	public void Write(Packet packet)
	{
		packet.WriteSignedVarLong(Id);
		packet.Write(ObjectiveName);
		packet.Write(Score);

		WriteData(packet);
	}

	protected virtual void WriteData(Packet packet) { }

	protected static TEntry ReadData<TEntry>(Packet packet) where TEntry : ScoreEntry, new()
	{
		return new TEntry
		{
			Id = packet.ReadSignedVarLong(),
			ObjectiveName = packet.ReadString(),
			Score = packet.ReadUint()
		};
	}
}

public class ScoreEntryRemove : ScoreEntry
{
	public static ScoreEntry Read(Packet packet)
	{
		return ReadData<ScoreEntryRemove>(packet);
	}
}

public abstract class ScoreEntryChange : ScoreEntry
{
}

public class ScoreEntryChangePlayer : ScoreEntryChange
{
	public long EntityId { get; set; }

	protected override void WriteData(Packet packet)
	{
		packet.Write((byte) McpeSetScore.ChangeTypes.Player);
		packet.WriteEntityId(EntityId);
	}

	public static ScoreEntry Read(Packet packet)
	{
		ScoreEntryChangePlayer entry = ReadData<ScoreEntryChangePlayer>(packet);

		entry.EntityId = packet.ReadEntityId();

		return entry;
	}
}

public class ScoreEntryChangeEntity : ScoreEntryChange
{
	public long EntityId { get; set; }

	protected override void WriteData(Packet packet)
	{
		packet.Write((byte) McpeSetScore.ChangeTypes.Entity);
		packet.WriteEntityId(EntityId);
	}

	public static ScoreEntry Read(Packet packet)
	{
		ScoreEntryChangeEntity entry = ReadData<ScoreEntryChangeEntity>(packet);

		entry.EntityId = packet.ReadEntityId();

		return entry;
	}
}

public class ScoreEntryChangeFakePlayer : ScoreEntryChange
{
	public string CustomName { get; set; }

	protected override void WriteData(Packet packet)
	{
		packet.Write((byte) McpeSetScore.ChangeTypes.FakePlayer);
		packet.Write(CustomName);
	}

	public static ScoreEntry Read(Packet packet)
	{
		ScoreEntryChangeFakePlayer entry = ReadData<ScoreEntryChangeFakePlayer>(packet);

		entry.CustomName = packet.ReadString();

		return entry;
	}
}

public class ScoreboardIdentityEntries : List<ScoreboardIdentityEntry>, IPacketDataObject
{
	public void Write(Packet packet)
	{
		packet.Write((byte) (this.FirstOrDefault() is ScoreboardClearIdentityEntry ? McpeSetScoreboardIdentity.Operations.ClearIdentity : McpeSetScoreboardIdentity.Operations.RegisterIdentity));
		packet.WriteLength(Count);

		foreach (ScoreboardIdentityEntry entry in this) packet.Write(entry);
	}

	public static ScoreboardIdentityEntries Read(Packet packet)
	{
		var entries = new ScoreboardIdentityEntries();
		var type = (McpeSetScoreboardIdentity.Operations) packet.ReadByte();
		int count = packet.ReadLength();

		switch (type)
		{
			case McpeSetScoreboardIdentity.Operations.RegisterIdentity:
				for (int i = 0; i < count; i++) entries.Add(ScoreboardRegisterIdentityEntry.Read(packet));

				break;
			case McpeSetScoreboardIdentity.Operations.ClearIdentity:
				for (int i = 0; i < count; i++) entries.Add(ScoreboardClearIdentityEntry.Read(packet));

				break;
			default:
				throw new Exception($"Unexpected scoreboard identity operation type = [{type}]");
		}

		return entries;
	}
}

public abstract class ScoreboardIdentityEntry : IPacketDataObject
{
	public long Id { get; set; }

	public void Write(Packet packet)
	{
		packet.WriteSignedVarLong(Id);

		WriteData(packet);
	}

	protected virtual void WriteData(Packet packet) { }

	protected static TEntry ReadData<TEntry>(Packet packet) where TEntry : ScoreboardIdentityEntry, new()
	{
		return new TEntry { Id = packet.ReadSignedVarLong() };
	}
}

public class ScoreboardRegisterIdentityEntry : ScoreboardIdentityEntry
{
	public long EntityId { get; set; }

	protected override void WriteData(Packet packet)
	{
		packet.WriteEntityId(EntityId);
	}

	public static ScoreboardIdentityEntry Read(Packet packet)
	{
		ScoreboardRegisterIdentityEntry entry = ReadData<ScoreboardRegisterIdentityEntry>(packet);

		entry.EntityId = packet.ReadEntityId();

		return entry;
	}
}

public class ScoreboardClearIdentityEntry : ScoreboardIdentityEntry
{
	public static ScoreboardIdentityEntry Read(Packet packet)
	{
		return ReadData<ScoreboardClearIdentityEntry>(packet);
	}
}