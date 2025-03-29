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

using PigNet.Utils.Metadata;

namespace PigNet.Net.Packets.Mcpe;

public class McpeAddActor : Packet<McpeAddActor>
{
	public EntityAttributes attributes;
	public float bodyYaw;

	public long entityIdSelf;
	public string entityType;
	public float headYaw;
	public EntityLinks links;
	public MetadataDictionary metadata;
	public float pitch;
	public long runtimeEntityId;
	public float speedX;
	public float speedY;
	public float speedZ;
	public PropertySyncData syncdata;
	public float x;
	public float y;
	public float yaw;
	public float z;

	public McpeAddActor()
	{
		Id = 0x0d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(entityIdSelf);
		WriteUnsignedVarLong(runtimeEntityId);
		Write(entityType);
		Write(x);
		Write(y);
		Write(z);
		Write(speedX);
		Write(speedY);
		Write(speedZ);
		Write(pitch);
		Write(yaw);
		Write(headYaw);
		Write(bodyYaw);
		Write(attributes);
		Write(metadata);
		Write(syncdata);
		Write(links);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		entityIdSelf = ReadSignedVarLong();
		runtimeEntityId = ReadUnsignedVarLong();
		entityType = ReadString();
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		speedX = ReadFloat();
		speedY = ReadFloat();
		speedZ = ReadFloat();
		pitch = ReadFloat();
		yaw = ReadFloat();
		headYaw = ReadFloat();
		bodyYaw = ReadFloat();
		attributes = ReadEntityAttributes();
		metadata = ReadMetadataDictionary();
		syncdata = ReadPropertySyncData();
		links = ReadEntityLinks();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		entityIdSelf = default;
		runtimeEntityId = default;
		entityType = default;
		x = default;
		y = default;
		z = default;
		speedX = default;
		speedY = default;
		speedZ = default;
		pitch = default;
		yaw = default;
		headYaw = default;
		bodyYaw = default;
		attributes = default;
		metadata = default;
		syncdata = default;
		links = default;
	}
}