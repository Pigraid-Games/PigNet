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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2024 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items.Tools;

public class Bundle(string name, short id, short metadata = 0, int count = 1)
	: Item(name, id, metadata, count)
{
	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		Console.WriteLine("The Bundle has been dropped");
	}
}

public class ItemBundle() : Bundle("minecraft:bundle", 780);

public class ItemWhiteBundle() : Bundle("minecraft:white_bundle", 781);

public class ItemOrangeBundle() : Bundle("minecraft:orange_bundle", 782);

public class ItemMagentaBundle() : Bundle("minecraft:magenta_bundle", 783);

public class ItemLightBlueBundle() : Bundle("minecraft:light_blue_bundle", 784);

public class ItemYellowBundle() : Bundle("minecraft:yellow_bundle", 785);

public class ItemLimeBundle() : Bundle("minecraft:lime_bundle", 786);

public class ItemPinkBundle() : Bundle("minecraft:pink_bundle", 787);

public class ItemGrayBundle() : Bundle("minecraft:gray_bundle", 788);

public class ItemLightGrayBundle() : Bundle("minecraft:light_gray_bundle", 789);

public class ItemCyanBundle() : Bundle("minecraft:cyan_bundle", 790);

public class ItemPurpleBundle() : Bundle("minecraft:purple_bundle", 791);

public class ItemBlueBundle() : Bundle("minecraft:blue_bundle", 792);

public class ItemBrownBundle() : Bundle("minecraft:brown_bundle", 793);

public class ItemGreenBundle() : Bundle("minecraft:green_bundle", 794);

public class ItemRedBundle() : Bundle("minecraft:red_bundle", 795);

public class ItemBlackBundle() : Bundle("minecraft:black_bundle", 796);