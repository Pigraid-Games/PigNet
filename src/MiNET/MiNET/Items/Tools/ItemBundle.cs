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

public class Bundle(string name, short metadata = 0, int count = 1)
	: Item(name, metadata, count)
{
	public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		// TODO: Implemen the inventory system
	}
}

public class ItemBundle() : Bundle("minecraft:bundle");
public class ItemWhiteBundle() : Bundle("minecraft:white_bundle");
public class ItemOrangeBundle() : Bundle("minecraft:orange_bundle");
public class ItemMagentaBundle() : Bundle("minecraft:magenta_bundle");
public class ItemLightBlueBundle() : Bundle("minecraft:light_blue_bundle");
public class ItemYellowBundle() : Bundle("minecraft:yellow_bundle");
public class ItemLimeBundle() : Bundle("minecraft:lime_bundle");
public class ItemPinkBundle() : Bundle("minecraft:pink_bundle");
public class ItemGrayBundle() : Bundle("minecraft:gray_bundle");
public class ItemLightGrayBundle() : Bundle("minecraft:light_gray_bundle");
public class ItemCyanBundle() : Bundle("minecraft:cyan_bundle");
public class ItemPurpleBundle() : Bundle("minecraft:purple_bundle");
public class ItemBlueBundle() : Bundle("minecraft:blue_bundle");
public class ItemBrownBundle() : Bundle("minecraft:brown_bundle");
public class ItemGreenBundle() : Bundle("minecraft:green_bundle");
public class ItemRedBundle() : Bundle("minecraft:red_bundle");
public class ItemBlackBundle() : Bundle("minecraft:black_bundle");