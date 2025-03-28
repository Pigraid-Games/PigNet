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

using System.Net;

namespace MiNET.Net.Packets.RakNet;

public class OpenConnectionReply2 : Packet<OpenConnectionReply2>
{
	public readonly byte[] offlineMessageDataId = [0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78];
	public IPEndPoint clientEndpoint; 
	public byte[] doSecurityAndHandshake;
	public short mtuSize;
	public long serverGuid;

	public OpenConnectionReply2()
	{
		Id = 0x08;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(offlineMessageDataId);
		Write(serverGuid);
		Write(clientEndpoint);
		WriteBe(mtuSize);
		Write(doSecurityAndHandshake);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		ReadBytes(offlineMessageDataId.Length);
		serverGuid = ReadLong();
		clientEndpoint = ReadIPEndPoint();
		mtuSize = ReadShortBe();
		doSecurityAndHandshake = ReadBytes(0, true);
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		serverGuid = default;
		clientEndpoint = default;
		mtuSize = default;
		doSecurityAndHandshake = default;
	}
}