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

namespace MiNET.Net.Packets.Mcpe;

public class McpeClientCacheBlobStatus : Packet<McpeClientCacheBlobStatus>
{
	public ulong[] hashMisses;
	public ulong[] hashHits;
	
	public McpeClientCacheBlobStatus()
	{
		Id = 0x87;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarInt((uint) hashMisses.Length);
		WriteUnsignedVarInt((uint) hashHits.Length);
		WriteSpecial(hashMisses);
		WriteSpecial(hashHits);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		var lenMisses = ReadUnsignedVarInt();
		var lenHits = ReadUnsignedVarInt();

		hashMisses = ReadUlongsSpecial(lenMisses);
		hashHits = ReadUlongsSpecial(lenHits);
	}

	public void WriteSpecial(ulong[] values)
	{
		if (values == null) return;

		if (values.Length == 0) return;
		for (int i = 0; i < values.Length; i++)
		{
			ulong val = values[i];
			Write(val);
		}
	}

	public ulong[] ReadUlongsSpecial(uint len)
	{
		var values = new ulong[len];
		for (int i = 0; i < values.Length; i++)
		{
			values[i] = ReadUlong();
		}
		return values;
	}
}