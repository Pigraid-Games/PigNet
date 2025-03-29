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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using PigNet.Utils.Vectors;

namespace PigNet.Net.Packets.Mcpe;

public class McpeCommandBlockUpdate : Packet<McpeCommandBlockUpdate>
{
	public bool isBlock;
	public string command;
	public uint commandBlockMode;
	public BlockCoordinates coordinates;
	public bool isConditional;
	public bool isRedstoneMode;
	public string lastOutput;
	public long minecartEntityId;
	public string name;
	public bool shouldTrackOutput;
	
	public McpeCommandBlockUpdate()
	{
		Id = 0x4e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		
		Write(isBlock);
		if (isBlock)
		{
			Write(coordinates);
			WriteUnsignedVarInt(commandBlockMode);
			Write(isRedstoneMode);
			Write(isConditional);
		}
		else
			WriteUnsignedVarLong(minecartEntityId);

		Write(command);
		Write(lastOutput);
		Write(name);
		Write(shouldTrackOutput);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		isBlock = ReadBool();
		if (isBlock)
		{
			coordinates = ReadBlockCoordinates();
			commandBlockMode = ReadUnsignedVarInt();
			isRedstoneMode = ReadBool();
			isConditional = ReadBool();
		}
		else
			minecartEntityId = ReadUnsignedVarLong();

		command = ReadString();
		lastOutput = ReadString();
		name = ReadString();
		shouldTrackOutput = ReadBool();
	}

	public override void Reset()
	{
		base.Reset();
		
		isBlock = default;
		coordinates = default;
		commandBlockMode = default;
		isRedstoneMode = default;
		isConditional = default;
		minecartEntityId = default;
		command = default;
		lastOutput = default;
		name = default;
		shouldTrackOutput = default;

	}
}