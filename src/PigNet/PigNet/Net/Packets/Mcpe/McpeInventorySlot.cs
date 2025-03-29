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

using PigNet.Items;
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeInventorySlot : Packet<McpeInventorySlot>
{
	public uint containerId;
	public uint slot;
	public FullContainerName fullContainerName = new();
	public Item storageItem;
	public Item item;

	public McpeInventorySlot()
	{
		Id = 0x32;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt(containerId);
		WriteUnsignedVarInt(slot);
		Write(fullContainerName);
		Write(storageItem);
		Write(item);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		containerId = ReadUnsignedVarInt();
		slot = ReadUnsignedVarInt();
		fullContainerName = readFullContainerName();
		storageItem = ReadItem();
		item = ReadItem();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		containerId = default;
		slot = default;
		fullContainerName = default;
		storageItem = default;
		item = default;
	}
}