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

using System;
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeText : Packet<McpeText>
{
	public TextPacketType type;
	public string filteredMessage;
	public string message;
	public bool needsTranslation;
	public string[] parameters;
	public string platformChatId;
	public string source;
	public string xuid;

	public McpeText()
	{
		Id = 0x09;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte) type);
		Write(needsTranslation);
		var chatType = type;
		switch (chatType)
		{
			case TextPacketType.Chat:
			case TextPacketType.Whisper:
			case TextPacketType.Announcement:
				Write(source);
				goto case TextPacketType.Raw;
			case TextPacketType.Raw:
			case TextPacketType.Tip:
			case TextPacketType.SystemMessage:
			case TextPacketType.TextObject:
			case TextPacketType.TextObjectWhisper:
			case TextPacketType.TextObjectAnnouncement:
				Write(message);
				break;
			case TextPacketType.Popup:
			case TextPacketType.Translate:
			case TextPacketType.JukeboxPopup:
				Write(message);
				if (parameters == null)
					WriteUnsignedVarInt(0);
				else
				{
					WriteUnsignedVarInt((uint) parameters.Length);
					foreach (string parameter in parameters) Write(parameter);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		Write(xuid);
		Write(platformChatId);
		Write(filteredMessage);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		type = (TextPacketType) ReadByte();

		needsTranslation = ReadBool();

		TextPacketType chatType = type;
		switch (chatType)
		{
			case TextPacketType.Chat:
			case TextPacketType.Whisper:
			case TextPacketType.Announcement:
				source = ReadString();
				message = ReadString();
				break;
			case TextPacketType.Raw:
			case TextPacketType.Tip:
			case TextPacketType.SystemMessage:
			case TextPacketType.TextObjectWhisper:
			case TextPacketType.TextObject:
			case TextPacketType.TextObjectAnnouncement:
				message = ReadString();
				break;

			case TextPacketType.Popup:
			case TextPacketType.Translate:
			case TextPacketType.JukeboxPopup:
				message = ReadString();
				parameters = new string[ReadUnsignedVarInt()];
				for (int i = 0; i < parameters.Length; ++i) parameters[i] = ReadString();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		xuid = ReadString();
		platformChatId = ReadString();
		filteredMessage = ReadString();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		type = default;
		source = null;
		message = null;
	}
}