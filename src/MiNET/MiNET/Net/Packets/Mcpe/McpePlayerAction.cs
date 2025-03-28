﻿#region LICENSE
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

using MiNET.Net.EnumerationsTable;
using MiNET.Utils.Vectors;

namespace MiNET.Net.Packets.Mcpe;

public class McpePlayerAction : Packet<McpePlayerAction>
{
	public PlayerActionType actionId;
	public BlockCoordinates coordinates;
	public int face;
	public BlockCoordinates resultCoordinates;

	public long runtimeActorId;

	public McpePlayerAction()
	{
		Id = 0x24;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(runtimeActorId);
		WriteSignedVarInt((int) actionId);
		Write(coordinates);
		Write(resultCoordinates);
		WriteSignedVarInt(face);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		runtimeActorId = ReadUnsignedVarLong();
		actionId = (PlayerActionType)ReadSignedVarInt();
		coordinates = ReadBlockCoordinates();
		resultCoordinates = ReadBlockCoordinates();
		face = ReadSignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		runtimeActorId = default;
		actionId = default;
		coordinates = default;
		resultCoordinates = default;
		face = default;
	}
}