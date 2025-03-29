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

using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeUpdateBlockSynced : Packet<McpeUpdateBlockSynced>
{
	public BlockCoordinates blockPosition;
	public uint blockRuntimeId;
	public uint flags;
	public uint dataLayerId;
	public long runtimeEntityId;
	public long runtimeEntitySyncMessageId;

	public McpeUpdateBlockSynced()
	{
		Id = 0x6e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(blockPosition);
		WriteUnsignedVarInt(blockRuntimeId);
		WriteUnsignedVarInt(flags);
		WriteUnsignedVarInt(dataLayerId);
		WriteUnsignedVarLong(runtimeEntityId);
		WriteUnsignedVarLong(runtimeEntitySyncMessageId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		blockPosition = ReadBlockCoordinates();
		blockRuntimeId = ReadUnsignedVarInt();
		flags = ReadUnsignedVarInt();
		dataLayerId = ReadUnsignedVarInt();
		runtimeEntityId = ReadUnsignedVarLong();
		runtimeEntitySyncMessageId = ReadUnsignedVarLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		blockPosition = default;
		blockRuntimeId = default;
		flags = default;
		dataLayerId = default;
		runtimeEntityId = default;
		runtimeEntitySyncMessageId = default;
	}
}