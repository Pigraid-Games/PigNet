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

using System.Numerics;

namespace MiNET.Net.Packets.Mcpe;

public class McpeChangeDimension : Packet<McpeChangeDimension>
{
	public int dimensionId;
	public Vector3 position;
	public bool respawn;
	// Leave empty if there is no loading screen expected on the client. This id needs to be unique and not conflict with any other active loading screens.
	// This is implemented with an unsigned integer incrementing forever, and that is expected to not have collisions when
	// it wraps around back to 0 if that could be a possibility.
	public uint loadingScreenId;

	public McpeChangeDimension()
	{
		Id = 0x3d;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarInt(dimensionId);
		Write(position);
		Write(respawn);
		WriteUnsignedVarInt(loadingScreenId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		dimensionId = ReadSignedVarInt();
		position = ReadVector3();
		respawn = ReadBool();
		loadingScreenId = ReadUnsignedVarInt();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		dimensionId = default;
		position = default;
		respawn = default;
		loadingScreenId = default;
	}
}