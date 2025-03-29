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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

using PigNet.Items;
using PigNet.Utils;
using PigNet.Utils.Metadata;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAddPlayer : Packet<McpeAddPlayer>
{
	public byte commandPermissions;
	public string deviceId;
	public int deviceOs;
	public long entityIdSelf;
	public uint gameType;
	public float headYaw;
	public Item item;
	public AbilityLayers layers;
	public EntityLinks links;
	public MetadataDictionary metadata;
	public float pitch;
	public string platformChatId;
	public byte playerPermissions;
	public long runtimeEntityId;
	public float speedX;
	public float speedY;
	public float speedZ;
	public PropertySyncData syncdata;
	public string username;

	public UUID uuid;
	public float x;
	public float y;
	public float yaw;
	public float z;

	public McpeAddPlayer()
	{
		Id = 0x0c;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(uuid);
		Write(username);
		WriteUnsignedVarLong(runtimeEntityId);
		Write(platformChatId);
		Write(x);
		Write(y);
		Write(z);
		Write(speedX);
		Write(speedY);
		Write(speedZ);
		Write(pitch);
		Write(yaw);
		Write(headYaw);
		Write(item);
		WriteUnsignedVarInt(gameType);
		Write(metadata);
		Write(syncdata);
		Write((ulong) entityIdSelf);
		Write(playerPermissions);
		Write(commandPermissions);
		Write(layers);
		Write(links);
		Write(deviceId);
		Write(deviceOs);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		uuid = ReadUUID();
		username = ReadString();
		runtimeEntityId = ReadUnsignedVarLong();
		platformChatId = ReadString();
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		speedX = ReadFloat();
		speedY = ReadFloat();
		speedZ = ReadFloat();
		pitch = ReadFloat();
		yaw = ReadFloat();
		headYaw = ReadFloat();
		item = ReadItem();
		gameType = ReadUnsignedVarInt();
		metadata = ReadMetadataDictionary();
		syncdata = ReadPropertySyncData();
		entityIdSelf = ReadSignedVarLong();
		playerPermissions = ReadByte();
		commandPermissions = ReadByte();
		layers = ReadAbilityLayers();
		links = ReadEntityLinks();
		deviceId = ReadString();
		deviceOs = ReadInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		uuid = default;
		username = default;
		runtimeEntityId = default;
		platformChatId = default;
		x = default;
		y = default;
		z = default;
		speedX = default;
		speedY = default;
		speedZ = default;
		pitch = default;
		yaw = default;
		headYaw = default;
		item = default;
		gameType = default;
		metadata = default;
		syncdata = default;
		entityIdSelf = default;
		playerPermissions = default;
		commandPermissions = default;
		layers = default;
		links = default;
		deviceId = default;
		deviceOs = default;
	}
}