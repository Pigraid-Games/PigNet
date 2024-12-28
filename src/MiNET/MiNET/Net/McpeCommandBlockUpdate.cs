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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Net;

public partial class McpeCommandBlockUpdate : Packet<McpeCommandBlockUpdate>
{
	public BlockCoordinates Coordinates; // = null;
	public uint CommandBlockMode; // = null;
	public bool IsRedstoneMode; // = null;
	public bool IsConditional; // = null;
	public long MinecartEntityId; // = null;
	public string Command; // = null;
	public string LastOutput; // = null;
	public string Name; // = null;
	public bool ShouldTrackOutput; // = null;

	partial void AfterEncode()
	{
		if (IsBlock)
		{
			Write(Coordinates);
			WriteUnsignedVarInt(CommandBlockMode);
			Write(IsRedstoneMode);
			Write(IsConditional);
		}
		else
			WriteUnsignedVarLong(MinecartEntityId);

		Write(Command);
		Write(LastOutput);
		Write(Name);
		Write(ShouldTrackOutput);
	}

	partial void AfterDecode()
	{
		if (IsBlock)
		{
			Coordinates = ReadBlockCoordinates();
			CommandBlockMode = ReadUnsignedVarInt();
			IsRedstoneMode = ReadBool();
			IsConditional = ReadBool();
		}
		else
			MinecartEntityId = ReadUnsignedVarLong();

		Command = ReadString();
		LastOutput = ReadString();
		Name = ReadString();
		ShouldTrackOutput = ReadBool();
	}

	public override void Reset()
	{
		Coordinates = default;
		CommandBlockMode = default;
		IsRedstoneMode = default;
		IsConditional = default;
		MinecartEntityId = default;
		Command = default;
		LastOutput = default;
		Name = default;
		ShouldTrackOutput = default;

		base.Reset();
	}
}