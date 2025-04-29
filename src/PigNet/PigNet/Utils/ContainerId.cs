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

using System;

namespace PigNet.Utils;

public enum ContainerId
	{
		Unknown = -1,

		AnvilInput = 0,
		AnvilMaterial = 1,
		AnvilResultPreview = 2,
		SmithingTableInput = 3,
		SmithingTableMaterial = 4,
		SmithingTableResultPreview = 5,
		Armor = 6,
		LevelEntity = 7,
		BeaconPayment = 8,
		BrewingStandInput = 9,
		BrewingStandResult = 10,
		BrewingStandFuel = 11,
		CombinedHotbarAndInventory = 12,
		CraftingInput = 13,
		CraftingOutputPreview = 14,
		RecipeConstruction = 15,
		RecipeNature = 16,
		RecipeItems = 17,
		RecipeSearch = 18,
		RecipeSearchBar = 19,
		RecipeEquipment = 20,
		RecipeBook = 21,
		EnchantingInput = 22,
		EnchantingMaterial = 23,
		FurnaceFuel = 24,
		FurnaceIngredient = 25,
		FurnaceResult = 26,
		HorseEquip = 27,
		Hotbar = 28,
		Inventory = 29,
		ShulkerBox = 30,
		TradeIngredient1 = 31,
		TradeIngredient2 = 32,
		TradeResultPreview = 33,
		Offhand = 34,
		CompoundCreatorInput = 35,
		CompoundCreatorOutputPreview = 36,
		ElementConstructorOutputPreview = 37,
		MaterialReducerInput = 38,
		MaterialReducerOutput = 39,
		LabTableInput = 40,
		LoomInput = 41,
		LoomDye = 42,
		LoomMaterial = 43,
		LoomResultPreview = 44,
		BlastFurnaceIngredient = 45,
		SmokerIngredient = 46,
		Trade2Ingredient1 = 47,
		Trade2Ingredient2 = 48,
		Trade2ResultPreview = 49,
		GrindstoneInput = 50,
		GrindstoneAdditional = 51,
		GrindstoneResultPreview = 52,
		StonecutterInput = 53,
		StonecutterResultPreview = 54,
		CartographyInput = 55,
		CartographyAdditional = 56,
		CartographyResultPreview = 57,
		Barrel = 58,
		Cursor = 59,
		CreatedOutput = 60,
		SmithingTableTemplate = 61,
		Crafter = 62,
		Dynamic = 63
	}

	public static class WindowIdExtensions
	{
		public static WindowId ToWindowId(this ContainerId containerId)
		{
			return ToWindowId(containerId, WindowId.None);
		}

		public static WindowId ToWindowId(this ContainerId containerId, WindowId currentWindowId)
		{
			switch (containerId)
			{
				case ContainerId.Armor:
					return WindowId.Armor;

				case ContainerId.Hotbar:
				case ContainerId.Inventory:
				case ContainerId.CombinedHotbarAndInventory:
					return WindowId.Inventory;

				case ContainerId.Offhand:
					return WindowId.Offhand;

				case ContainerId.AnvilInput:
				case ContainerId.AnvilMaterial:
				case ContainerId.BeaconPayment:
				case ContainerId.CartographyAdditional:
				case ContainerId.CartographyInput:
				case ContainerId.CompoundCreatorInput:
				case ContainerId.CraftingInput:
				case ContainerId.CreatedOutput:
				case ContainerId.Cursor:
				case ContainerId.EnchantingInput:
				case ContainerId.EnchantingMaterial:
				case ContainerId.GrindstoneAdditional:
				case ContainerId.GrindstoneInput:
				case ContainerId.LabTableInput:
				case ContainerId.LoomDye:
				case ContainerId.LoomInput:
				case ContainerId.LoomMaterial:
				case ContainerId.MaterialReducerInput:
				case ContainerId.MaterialReducerOutput:
				case ContainerId.SmithingTableInput:
				case ContainerId.SmithingTableMaterial:
				case ContainerId.SmithingTableTemplate:
				case ContainerId.StonecutterInput:
				case ContainerId.Trade2Ingredient1:
				case ContainerId.Trade2Ingredient2:
				case ContainerId.TradeIngredient1:
				case ContainerId.TradeIngredient2:
					return WindowId.UI;

				case ContainerId.Barrel:
				case ContainerId.BlastFurnaceIngredient:
				case ContainerId.BrewingStandFuel:
				case ContainerId.BrewingStandInput:
				case ContainerId.BrewingStandResult:
				case ContainerId.FurnaceFuel:
				case ContainerId.FurnaceIngredient:
				case ContainerId.FurnaceResult:
				case ContainerId.HorseEquip:
				case ContainerId.LevelEntity: //chest
				case ContainerId.ShulkerBox:
				case ContainerId.SmokerIngredient:
					return currentWindowId;

				default:
					throw new Exception($"Unexpected container ID {containerId}");
			};
		}
	}