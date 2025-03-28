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

using MiNET.Net.EnumerationsTable;

namespace MiNET.Net.Packets.Mcpe;

public class McpeMovePlayer : Packet<McpeMovePlayer>
{
	public float headYaw;
	public PositionMode mode;
	public bool onGround;
	public long ridingRuntimeId;
	public float pitch;

	public long playerRuntimeId;
	public float x;
	public float y;
	public float yaw;
	public float z;
	
	public long tick;

	public McpeMovePlayer()
	{
		Id = 0x13;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(playerRuntimeId);
		Write(x);
		Write(y);
		Write(z);
		Write(pitch);
		Write(yaw);
		Write(headYaw);
		Write((byte) mode);
		Write(onGround);
		WriteUnsignedVarLong(ridingRuntimeId);
		if (mode == PositionMode.Teleport)
		{
			Write(0);
			Write(0);
		}

		WriteUnsignedVarLong(tick);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		playerRuntimeId = ReadUnsignedVarLong();
		x = ReadFloat();
		y = ReadFloat();
		z = ReadFloat();
		pitch = ReadFloat();
		yaw = ReadFloat();
		headYaw = ReadFloat();
		mode = (PositionMode) ReadByte();
		onGround = ReadBool();
		ridingRuntimeId = ReadUnsignedVarLong();
		if (mode == PositionMode.Teleport)
		{
			ReadInt();
			ReadInt();
		}

		tick = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		playerRuntimeId = default;
		x = default;
		y = default;
		z = default;
		pitch = default;
		yaw = default;
		headYaw = default;
		mode = default;
		onGround = default;
		ridingRuntimeId = default;
	}
}