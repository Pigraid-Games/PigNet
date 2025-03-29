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

using System.Net;

namespace PigNet.Net.Packets.RakNet;

public class ConnectionRequestAccepted : Packet<ConnectionRequestAccepted>
{
	public long incomingTimestamp;
	public long serverTimestamp;
	public IPEndPoint systemAddress;
	public IPEndPoint[] systemAddresses;
	public short systemIndex;

	public ConnectionRequestAccepted()
	{
		Id = 0x10;
		IsMcpe = false;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(systemAddress);
		WriteBe(systemIndex);
		Write(systemAddresses);
		Write(incomingTimestamp);
		Write(serverTimestamp);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		systemAddress = ReadIPEndPoint();
		systemIndex = ReadShortBe();
		systemAddresses = ReadIPEndPoints(20);
		incomingTimestamp = ReadLong();
		serverTimestamp = ReadLong();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		systemAddress = default;
		systemIndex = default;
		systemAddresses = default;
		incomingTimestamp = default;
		serverTimestamp = default;
	}
}