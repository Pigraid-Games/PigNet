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

using MiNET.Utils;

namespace MiNET.Net.Packets.Mcpe;

public class McpeResourcePackStack : Packet<McpeResourcePackStack>
{
	public ResourcePackIdVersions behaviorpackidversions;
	public Experiments experiments;
	public bool experimentsPreviouslyToggled;
	public string gameVersion;
	public bool hasEditorPacks;

	public bool mustAccept;
	public ResourcePackIdVersions resourcepackidversions;

	public McpeResourcePackStack()
	{
		Id = 0x07;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(mustAccept);
		Write(behaviorpackidversions);
		Write(resourcepackidversions);
		Write(gameVersion);
		Write(experiments);
		Write(experimentsPreviouslyToggled);
		Write(hasEditorPacks);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		mustAccept = ReadBool();
		behaviorpackidversions = ReadResourcePackIdVersions();
		resourcepackidversions = ReadResourcePackIdVersions();
		gameVersion = ReadString();
		experiments = ReadExperiments();
		experimentsPreviouslyToggled = ReadBool();
		hasEditorPacks = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		mustAccept = default;
		behaviorpackidversions = default;
		resourcepackidversions = default;
		gameVersion = default;
		experiments = default;
		experimentsPreviouslyToggled = default;
		hasEditorPacks = default;
	}
}