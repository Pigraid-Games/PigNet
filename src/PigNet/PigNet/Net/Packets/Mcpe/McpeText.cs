
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