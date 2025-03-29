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

namespace PigNet.Net.Packets.Mcpe;

public class McpeSetTitle : Packet<McpeSetTitle>
{
	public int fadeInTime;
	public int fadeOutTime;
	public string filteredString;
	public string platformOnlineId;
	public int stayTime;
	public string text;

	public int type;
	public string xuid;

	public McpeSetTitle()
	{
		Id = 0x58;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(type);
		Write(text);
		WriteSignedVarInt(fadeInTime);
		WriteSignedVarInt(stayTime);
		WriteSignedVarInt(fadeOutTime);
		Write(xuid);
		Write(platformOnlineId);
		Write(filteredString);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		type = ReadSignedVarInt();
		text = ReadString();
		fadeInTime = ReadSignedVarInt();
		stayTime = ReadSignedVarInt();
		fadeOutTime = ReadSignedVarInt();
		xuid = ReadString();
		platformOnlineId = ReadString();
		filteredString = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		type = default;
		text = default;
		fadeInTime = default;
		stayTime = default;
		fadeOutTime = default;
		xuid = default;
		platformOnlineId = default;
	}
}