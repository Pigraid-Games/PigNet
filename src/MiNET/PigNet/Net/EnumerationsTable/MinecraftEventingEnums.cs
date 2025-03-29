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

namespace PigNet.Net.EnumerationsTable;

public enum MinecraftEventingInteractionType
{
	Breeding = 1,
	Taming = 2,
	Curing = 3,
	Crafted = 4,
	Shearing = 5,
	Milking = 6,
	Trading = 7,
	Feeding = 8,
	Igniting = 9,
	Coloring = 10,
	Naming = 11,
	Leashing = 12,
	Unleashing = 13,
	PetSleep = 14,
	Trusting = 15,
	Commanding = 16
}

public enum MinecraftEventingPOIBlockInteractionType
{
	None = 0,
	Extend = 1,
	Clone = 2,
	Lock = 3,
	Create = 4,
	CreateLocator = 5,
	Rename = 6,
	ItemPlaced = 7,
	ItemRemoved = 8,
	Cooking = 9,
	Dousing = 10,
	Lighting = 11,
	Haystack = 12,
	Filled = 13,
	Emptied = 14,
	AddDye = 15,
	DyeItem = 16,
	ClearItem = 17,
	EnchantArrow = 18,
	CompostItemPlaced = 19,
	RecoveredBonemeal = 20,
	BookPlaced = 21,
	BookOpened = 22,
	Disenchant = 23,
	Repair = 24,
	DisenchantAndRepair = 25
}

public enum MinecraftEventingTeleportationCause
{
	Unknown = 0,
	Projectile = 1,
	ChorusFruit = 2,
	Command = 3,
	Behavior = 4,
	TeleportationCause_Count = 5
}