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

using System;

namespace PigNet.Net.EnumerationsTable;

public enum InventoryLayout
{
	None = 0,
	Survival = 1,
	RecipeBook = 2,
	Creative = 3,
	Count = 4
}

public enum InventoryLeftTabIndex
{
	None = 0,
	RecipeConstruction = 1,
	RecipeEquipment = 2,
	RecipeItems = 3,
	RecipeNature = 4,
	RecipeSearch = 5,
	Survival = 6,
	Count = 7
}

public enum InventoryRightTabIndex
{
	None = 0,
	FullScreen = 1,
	Crafting = 2,
	Armor = 3,
	Count = 4
}

[Flags]
public enum InventorySourceFlags
{
	NoFlag = 0,
	WorldInteraction_Random = 1
}

public enum InventorySourceType : uint
{
	InvalidInventory = uint.MaxValue,
	ContainerInventory = 0,
	GlobalInventory = 1,
	WorldInteraction = 2,
	CreativeInventory = 3,
	NonImplementedFeatureTODO = 99999
}