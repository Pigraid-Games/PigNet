using System;
using System.Collections.Generic;
using PigNet;
using PigNet.Net;
using PigNet.Utils;
using PigNet.Utils.Skins;
using PigNet.Utils.Vectors;

namespace PigNet.Utils;

public class Records : List<BlockCoordinates>
	{
		public Records()
		{
		}
		public Records(IEnumerable<BlockCoordinates> coordinates) : base(coordinates)
		{
		}
	}

	public class PlayerRecord
	{
		public UUID ClientUuid { get; set; }

		public long EntityId { get; set; }

		public string Username { get; set; }

		public string Xuid { get; set; }

		public string PlatformChatId { get; set; }

		public int DeviceOS { get; set; }

		public Skin Skin { get; set; }

		public bool IsTeacher { get; set; } = false;

		public bool IsHost { get; set; } = false;

		public bool IsSubClient { get; set; } = false;
	}

	public abstract class PlayerRecords : List<PlayerRecord>, IPacketDataObject
	{
		public abstract PlayerRecordType Type { get; }

		public PlayerRecords()
		{

		}

		public PlayerRecords(IEnumerable<PlayerRecord> records) : base(records)
		{

		}

		public void Write(Packet packet)
		{
			packet.Write((byte) Type);
			packet.WriteLength(Count);

			WriteData(packet);
		}

		protected virtual void WriteData(Packet packet) { }

		public static PlayerRecords Read(Packet packet)
		{
			var type = (PlayerRecordType) packet.ReadByte();

			return type switch
			{
				PlayerRecordType.Add => PlayerAddRecords.ReadData(packet),
				PlayerRecordType.Remove => PlayerRemoveRecords.ReadData(packet),
				_ => throw new Exception($"Unknown player record type [{type}]")
			};
		}
	}

	public class PlayerAddRecords : PlayerRecords
	{
		public override PlayerRecordType Type => PlayerRecordType.Add;

		public PlayerAddRecords()
		{

		}

		public PlayerAddRecords(Player player) : this(new Player[] { player })
		{

		}

		public PlayerAddRecords(IEnumerable<PlayerRecord> records) : base(records)
		{

		}

		public PlayerAddRecords(IEnumerable<Player> players)
		{
			foreach (var player in players)
			{
				Add(new PlayerRecord()
				{
					ClientUuid = player.ClientUuid,
					EntityId = player.EntityId,
					Username = player.DisplayName ?? player.Username,
					Xuid = player.CertificateData?.ExtraData?.Xuid ?? null,
					PlatformChatId = player.PlayerInfo.PlatformChatId,
					DeviceOS = player.PlayerInfo.DeviceOS,
					Skin = player.Skin,
					IsTeacher = false,
					IsHost = false,
					IsSubClient = false
				});
			}
		}

		protected override void WriteData(Packet packet)
		{
			foreach (var record in this)
			{
				packet.Write(record.ClientUuid);
				packet.WriteEntityId(record.EntityId);
				packet.Write(record.Username);
				packet.Write(record.Xuid);
				packet.Write(record.PlatformChatId);
				packet.Write(record.DeviceOS);
				packet.Write(record.Skin);
				packet.Write(record.IsTeacher);
				packet.Write(record.IsHost);
				packet.Write(record.IsSubClient);
			}

			foreach (var record in this)
			{
				packet.Write(record.Skin.IsVerified);
			}
		}

		internal static PlayerRecords ReadData(Packet packet)
		{
			var records = new PlayerAddRecords();

			var count = packet.ReadLength();
			for (var i = 0; i < count; i++)
			{
				records.Add(ReadRecord(packet));
			}

			foreach (var record in records)
			{
				record.Skin.IsVerified = packet.ReadBool();
			}

			return records;
		}

		private static PlayerRecord ReadRecord(Packet packet)
		{
			return new PlayerRecord()
			{
				ClientUuid = packet.ReadUUID(),
				EntityId = packet.ReadEntityId(),
				Username = packet.ReadString(),
				Xuid = packet.ReadString(),
				PlatformChatId = packet.ReadString(),
				DeviceOS = packet.ReadInt(),
				Skin = packet.ReadSkin(),
				IsTeacher = packet.ReadBool(),
				IsHost = packet.ReadBool(),
				IsSubClient = packet.ReadBool()
			};
		}
	}

	public class PlayerRemoveRecords : PlayerRecords
	{
		public override PlayerRecordType Type => PlayerRecordType.Remove;

		public PlayerRemoveRecords()
		{

		}

		public PlayerRemoveRecords(IEnumerable<PlayerRecord> records) : base(records)
		{

		}

		public PlayerRemoveRecords(Player player) : this(new Player[] { player })
		{

		}

		public PlayerRemoveRecords(IEnumerable<Player> players)
		{
			foreach (var player in players)
			{
				Add(new PlayerRecord()
				{
					ClientUuid = player.ClientUuid
				});
			}
		}

		protected override void WriteData(Packet packet)
		{
			foreach (var record in this)
			{
				packet.Write(record.ClientUuid);
			}
		}

		internal static PlayerRecords ReadData(Packet packet)
		{
			var records = new PlayerRemoveRecords();

			var count = packet.ReadLength();
			for (var i = 0; i < count; i++)
			{
				records.Add(ReadRecord(packet));
			}

			return records;
		}

		private static PlayerRecord ReadRecord(Packet packet)
		{
			return new PlayerRecord()
			{
				ClientUuid = packet.ReadUUID()
			};
		}
	}