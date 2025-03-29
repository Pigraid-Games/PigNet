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

public class McpePlaySound : Packet<McpePlaySound>
{
	public float pitch;
	public string soundName;
	public float volume;
	public float x;
	public float y;
	public float z;

	public McpePlaySound()
	{
		Id = 0x56;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(soundName);
		Write(new BlockCoordinates((int) x * 8, (int) y * 8, (int) z * 8));
		Write(volume);
		Write(pitch);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		soundName = ReadString();
		BlockCoordinates blockCoordinates = ReadBlockCoordinates();
		x = blockCoordinates.X / 8;
		y = blockCoordinates.Y / 8;
		z = blockCoordinates.Z / 8;
		volume = ReadFloat();
		pitch = ReadFloat();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		soundName = default;
		x = default;
		y = default;
		z = default;
		volume = default;
		pitch = default;
	}
}