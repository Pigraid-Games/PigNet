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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using MiNET.Net.EnumerationsTable;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MiNET.Net.Packets.Mcpe;

public class BossEventTypeAdd
{
	public string Name { get; set; }
	public string FilteredName { get; set; }
	public float HealthPercent { get; set; }
	public ushort DarkenScreen { get; set; }
	public uint Color { get; set; }
	public uint Overlay { get; set; }
}

public class BossEventTypePlayerAdd
{
	public long PlayerId { get; set; }
}

public class BossEventTypePlayerRemoved
{
	public long PlayerId { get; set; }
}

public class BossEventTypeUpdatePercent
{
	public float HealthPercent { get; set; }
}

public class BossEventTypeUpdateName
{
	public string Name { get; set; }
	public string FilteredName { get; set; }
}

public class BossEventTypeUpdateProperties
{
	public ushort DarkenScreen { get; set; }
	public uint Color { get; set; }
	public uint Overlay { get; set; }
}

public class BossEventTypeUpdateStyle
{
	public uint Color { get; set; }
	public uint Overlay { get; set; }
}

public class BossEventTypeQuery
{
	public long PlayerId { get; set; }
}

public class BossEventTypePlayerRemove;

public class McpeBossEvent : Packet<McpeBossEvent>
{

	public long targetActorId;
	public BossEventUpdateType eventType;
	public object eventData;
	
	public McpeBossEvent()
	{
		Id = 0x4a;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(targetActorId);
		WriteUnsignedVarInt((uint) eventType);

		switch (eventType)
		{
			case BossEventUpdateType.Add:
				if (eventData is BossEventTypeAdd bossEventTypeAdd)
				{
					Write(bossEventTypeAdd.Name);
					Write(bossEventTypeAdd.FilteredName);
					Write(bossEventTypeAdd.HealthPercent);
					Write(bossEventTypeAdd.DarkenScreen);
					WriteUnsignedVarInt(bossEventTypeAdd.Color);
					WriteUnsignedVarInt(bossEventTypeAdd.Overlay);
				}
				break;
			case BossEventUpdateType.PlayerAdded:
				if (eventData is BossEventTypePlayerAdd bossEventTypePlayerAdd) WriteSignedVarLong(bossEventTypePlayerAdd.PlayerId);
				break;
			case BossEventUpdateType.Remove:
				if (eventData is BossEventTypePlayerRemove)
				{
					// NOOP
				}
				break;
			case BossEventUpdateType.PlayerRemoved:
				if (eventData is BossEventTypePlayerRemoved bossEventTypePlayerRemoved) WriteSignedVarLong(bossEventTypePlayerRemoved.PlayerId);
				break;
			case BossEventUpdateType.UpdatePercent:
				if (eventData is BossEventTypeUpdatePercent bossEventTypeUpdatePercent) Write(bossEventTypeUpdatePercent.HealthPercent);
				break;
			case BossEventUpdateType.UpdateName:
				if (eventData is BossEventTypeUpdateName bossEventTypeUpdateName)
				{
					Write(bossEventTypeUpdateName.Name);
					Write(bossEventTypeUpdateName.FilteredName);
				}
				break;
			case BossEventUpdateType.UpdateProperties:
				if (eventData is BossEventTypeUpdateProperties bossEventTypeUpdateProperties)
				{
					Write(bossEventTypeUpdateProperties.DarkenScreen);
					WriteUnsignedVarInt(bossEventTypeUpdateProperties.Color);
					WriteUnsignedVarInt(bossEventTypeUpdateProperties.Overlay);
				}
				break;
			case BossEventUpdateType.UpdateStyle:
				if (eventData is BossEventTypeUpdateStyle bossEventTypeUpdateStyle)
				{
					WriteUnsignedVarInt(bossEventTypeUpdateStyle.Color);
					WriteUnsignedVarInt(bossEventTypeUpdateStyle.Overlay);
				}
				break;
			case BossEventUpdateType.Query:
				if (eventData is BossEventTypeQuery bossEventTypeQuery) WriteSignedVarLong(bossEventTypeQuery.PlayerId);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		targetActorId = ReadSignedVarLong();
		eventType = (BossEventUpdateType) ReadUnsignedVarInt();

		switch (eventType)
		{
			case BossEventUpdateType.Add:
				eventData = new BossEventTypeAdd
				{
					Name = ReadString(),
					FilteredName = ReadString(),
					HealthPercent = ReadFloat(),
					DarkenScreen = ReadUshort(),
					Color = ReadUnsignedVarInt(),
					Overlay = ReadUnsignedVarInt()
				};
				break;
			case BossEventUpdateType.PlayerAdded:
				eventData = new BossEventTypePlayerAdd { PlayerId = ReadSignedVarLong() };
				break;
			case BossEventUpdateType.Remove:
				break;
			case BossEventUpdateType.PlayerRemoved:
				eventData = new BossEventTypePlayerRemoved { PlayerId = ReadSignedVarLong() };
				break;
			case BossEventUpdateType.UpdatePercent:
				eventData = new BossEventTypeUpdatePercent { HealthPercent = ReadFloat() };
				break;
			case BossEventUpdateType.UpdateName:
				eventData = new BossEventTypeUpdateName
				{
					Name = ReadString(),
					FilteredName = ReadString()
				};
				break;
			case BossEventUpdateType.UpdateProperties:
				eventData = new BossEventTypeUpdateProperties
				{
					DarkenScreen = ReadUshort(),
					Color = ReadUnsignedVarInt(),
					Overlay = ReadUnsignedVarInt()
				};
				break;
			case BossEventUpdateType.UpdateStyle:
				eventData = new BossEventTypeUpdateStyle
				{
					Color = ReadUnsignedVarInt(),
					Overlay = ReadUnsignedVarInt()
				};
				break;
			case BossEventUpdateType.Query:
				eventData = new BossEventTypeQuery { PlayerId = ReadSignedVarLong() };
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		targetActorId = default;
		eventType = default;
	}
}