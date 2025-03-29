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

using System.Numerics;

namespace PigNet.Net.Packets.Mcpe;

public class McpeLevelSoundEvent : Packet<McpeLevelSoundEvent>
{
	public uint soundId;
	public Vector3 position;
	public int extraData = -1;
	public string entityType = ":";
	public bool isBabyMob;
	public bool disableRelativeVolume;
	public long actorUniqueId = -1;


	public McpeLevelSoundEvent()
	{
		Id = 0x7b;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(soundId);
		Write(position);
		WriteSignedVarInt(extraData);
		Write(":");
		Write(false);
		Write(disableRelativeVolume);
		Write(actorUniqueId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		soundId = ReadUnsignedVarInt();
		position = ReadVector3();
		extraData = ReadSignedVarInt();
		entityType = ReadString();
		isBabyMob = ReadBool();
		disableRelativeVolume = ReadBool();
		actorUniqueId = ReadInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		soundId = default;
		position = default;
		extraData = default;
		entityType = default;
		isBabyMob = default;
		disableRelativeVolume = default;
		actorUniqueId = default;
	}
}