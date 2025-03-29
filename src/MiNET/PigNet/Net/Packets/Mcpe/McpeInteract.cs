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
using PigNet.Net.EnumerationsTable;

namespace PigNet.Net.Packets.Mcpe;

public class McpeInteract : Packet<McpeInteract>
{
	public enum Actions
	{
		RightClick = 1,
		LeftClick = 2,
		LeaveVehicle = 3,
		MouseOver = 4,
		OpenNpc = 5,
		OpenInventory = 6
	}

	public InteractPacketAction actionId;
	public long targetRuntimeActorId;
	public Vector3 position;

	public McpeInteract()
	{
		Id = 0x21;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte) actionId);
		WriteUnsignedVarLong(targetRuntimeActorId);
		if (actionId is InteractPacketAction.InteractUpdate or InteractPacketAction.StopRiding)
			// TODO: Something useful with this value
			Write(position);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		actionId = (InteractPacketAction) ReadByte();
		targetRuntimeActorId = ReadUnsignedVarLong();
		if (actionId is InteractPacketAction.InteractUpdate or InteractPacketAction.StopRiding)
			// TODO: Something useful with this value
			position = ReadVector3();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		actionId = default;
		targetRuntimeActorId = default;
		position = default;
	}
}