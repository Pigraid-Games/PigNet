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

namespace MiNET.Net.EnumerationsTable;

public enum TextProcessingEventOrigin
{
	Unknown = -1,
	ServerChatPublic = 0,
	ServerChatWhisper = 1,
	SignText = 2,
	AnvilText = 3,
	BookAndQuillText = 4,
	CommandBlockText = 5,
	BlockActorDataText = 6,
	JoinEventText = 7,
	LeaveEventText = 8,
	SlashCommandChat = 9,
	CartographyText = 10,
	KickCommand = 11,
	TitleCommand = 12,
	SummonCommand = 13,
	ServerForm = 14,
	Count = 15
}