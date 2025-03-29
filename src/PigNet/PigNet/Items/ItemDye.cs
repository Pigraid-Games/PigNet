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

namespace PigNet.Items;

public class ItemDye() : Item("minecraft:dye", 351, canInteract: false)
{
	public static byte ToColorCode(int metadata)
	{
		return metadata switch
		{
			1 => //red
				14,
			2 => //green
				13,
			5 => //purple
				10,
			6 => //cyan
				9,
			7 => //light_gray
				8,
			8 => //gray
				7,
			9 => //pink
				6,
			10 => //lime
				5,
			11 => //yellow
				4,
			12 => //ligh_blue
				3,
			13 => //magenta
				2,
			14 => //orange
				1,
			16 => //black
				15,
			17 => //brown
				12,
			18 => //blue
				11,
			19 => //white
				0,
			_ => 255
		};
	}
}