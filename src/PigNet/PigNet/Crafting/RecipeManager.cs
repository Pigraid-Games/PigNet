using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using PigNet.Inventories;
using PigNet.Items;
using PigNet.Net.Crafting;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;
using PigNet.Worlds;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMethodReturnValue.Local

namespace PigNet.Crafting;

public class RecipeManager
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(RecipeManager));

	private static int RecipeUniqueIdCounter = 1;

	public static Dictionary<int, Recipe> NetworkIdRecipeMap { get; } = new();
	public static Dictionary<UUID, Recipe> IdRecipeMap { get; } = new();
	public static Dictionary<int, SmeltingRecipeBase> SmeltingRecipes { get; } = new();
	public static Recipes Recipes { get; private set; }

	private static McpeWrapper CraftingData;

	public static McpeWrapper GetCraftingData()
	{
		if (CraftingData != null) return CraftingData;
		McpeCraftingData craftingData = McpeCraftingData.CreateObject();
		craftingData.craftingEntries = Recipes;
		//craftingData.isClean = true;
		McpeWrapper packet = Level.CreateMcpeBatch(craftingData.Encode());
		craftingData.PutPool();
		packet.MarkPermanent();
		CraftingData = packet;

		return CraftingData;
	}

	static RecipeManager()
	{
		Recipes = new Recipes();

		LoadShapedRecipes();
		//LoadShapedChemistryRecipes(); // Edu only
		LoadShapelessRecipes();
		LoadShapelessShapelessUserDataRecipes();
		//LoadShapelessChemistryRecipes(); // Edu only
		LoadSmeltingRecipes();

		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("442d85ed-8272-4543-a6f1-418f90ded05d"),
			UniqueId = RecipeUniqueIdCounter++
		}); // 442d85ed-8272-4543-a6f1-418f90ded05d
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("8b36268c-1829-483c-a0f1-993b7156a8f2"),
			UniqueId = RecipeUniqueIdCounter++
		}); // 8b36268c-1829-483c-a0f1-993b7156a8f2
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("602234e4-cac1-4353-8bb7-b1ebff70024b"),
			UniqueId = RecipeUniqueIdCounter++
		}); // 602234e4-cac1-4353-8bb7-b1ebff70024b
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("98c84b38-1085-46bd-b1ce-dd38c159e6cc"),
			UniqueId = RecipeUniqueIdCounter++
		}); // 98c84b38-1085-46bd-b1ce-dd38c159e6cc
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("d81aaeaf-e172-4440-9225-868df030d27b"),
			UniqueId = RecipeUniqueIdCounter++
		}); // d81aaeaf-e172-4440-9225-868df030d27b
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("b5c5d105-75a2-4076-af2b-923ea2bf4bf0"),
			UniqueId = RecipeUniqueIdCounter++
		}); // b5c5d105-75a2-4076-af2b-923ea2bf4bf0
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("00000000-0000-0000-0000-000000000002"),
			UniqueId = RecipeUniqueIdCounter++
		}); // 00000000-0000-0000-0000-000000000002
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d"),
			UniqueId = RecipeUniqueIdCounter++
		}); // d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("85939755-ba10-4d9d-a4cc-efb7a8e943c4"),
			UniqueId = RecipeUniqueIdCounter++
		}); // 85939755-ba10-4d9d-a4cc-efb7a8e943c4
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("d392b075-4ba1-40ae-8789-af868d56f6ce"),
			UniqueId = RecipeUniqueIdCounter++
		}); // d392b075-4ba1-40ae-8789-af868d56f6ce
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("00000000-0000-0000-0000-000000000001"),
			UniqueId = RecipeUniqueIdCounter++
		}); // 00000000-0000-0000-0000-000000000001
		Recipes.Add(new MultiRecipe()
		{
			Id = new UUID("aecd2294-4b94-434b-8667-4499bb2c9327"),
			UniqueId = RecipeUniqueIdCounter++
		}); // aecd2294-4b94-434b-8667-4499bb2c9327
	}

	public static bool ValidateRecipe(Recipe recipe, List<Item> input, int times, out List<Item> resultItems, out Item[] consumeItems)
	{
		resultItems = null;
		consumeItems = null;

		return recipe switch
		{
			ShapedRecipe shapedRecipe => ValidateRecipe(shapedRecipe, input, times, out resultItems, out consumeItems),
			ShapelessRecipeBase shapedRecipe => ValidateRecipe(shapedRecipe, input, times, out resultItems, out consumeItems),

			_ => false
		};
	}

	public static bool TryGetSmeltingResult(Item input, string block, out Item output)
	{
		if (input is ItemAir)
		{
			output = null;
			return false;
		}

		int hash1 = HashCode.Combine(input, block);
		int hash2 = HashCode.Combine(input.RuntimeId, block);

		output = (SmeltingRecipes.GetValueOrDefault(hash1) ?? SmeltingRecipes.GetValueOrDefault(hash2))?.Output;

		return output != null;
	}

	private static bool ValidateRecipe(ShapedRecipe recipe, List<Item> input, int times, out List<Item> resultItems, out Item[] consumeItems)
	{
		consumeItems = new Item[input.Count];
		resultItems = [];

		var inputClone = input.ToList();
		foreach (RecipeIngredient ingredient in recipe.Input)
		{
			if (ingredient is RecipeAirIngredient) continue;
			int count = ingredient.Count * times;

			Item item = null;
			int index = 0;
			for (; index < inputClone.Count; index++)
			{
				item = inputClone[index];
				if (item == null || item is ItemAir) continue;
				if (ingredient.ValidateItem(item)) break;
			}

			if (index >= inputClone.Count) return false;
			if (item != null && item.Count < count) return false;

			item = item?.Clone() as Item;
			if (item != null)
			{
				item.Count = (byte) count;
				consumeItems[index] = item;
			}
			inputClone[index] = null;
		}

		foreach (Item item in recipe.Output)
		{
			if (item.Clone() is not Item resultItem) continue;
			resultItem.Count = (byte) (resultItem.Count * times);

			resultItems.Add(resultItem);
		}

		return true;
	}

	private static bool ValidateRecipe(ShapelessRecipeBase recipe, List<Item> input, int times, out List<Item> resultItems, out Item[] consumeItems)
	{
		consumeItems = new Item[input.Count];
		resultItems = [];

		var inputClone = input.ToList();

		foreach (RecipeIngredient ingredient in recipe.Input)
		{
			int count = ingredient.Count * times;
			int index = inputClone.FindIndex(ingredient.ValidateItem);
			if (index < 0) return false;

			Item item = inputClone[index];
			if (item == null) return false;
			if (item.Count < count) return false;

			item = item.Clone() as Item;
			if (item != null)
			{
				item.Count = (byte) count;
				consumeItems[index] = item;
			}
			inputClone[index] = new ItemAir();
		}

		foreach (Item item in recipe.Output)
		{
			if (item.Clone() is not Item resultItem) continue;
			resultItem.Count = (byte) (resultItem.Count * times);
			resultItems.Add(resultItem);
		}
		return true;
	}

	private static void LoadShapelessChemistryRecipes()
	{
		LoadShapelessRecipesBase("shapeless_chemistry.json", recipe =>
			new ShapelessChemistryRecipe(recipe.Output, recipe.Input, recipe.Block)
			{
				Priority = recipe.Priority,
				UniqueId = recipe.UniqueId
			});
	}

	private static void LoadShapelessShapelessUserDataRecipes()
	{
		LoadShapelessRecipesBase("shapeless_shulker_box.json", recipe =>
			new ShapelessUserDataRecipe(recipe.Output, recipe.Input, recipe.Block)
			{
				Priority = recipe.Priority,
				UniqueId = recipe.UniqueId,
				UnlockingRequirement = recipe.UnlockingRequirement,
			});
	}

	private static void LoadShapelessRecipes()
	{
		LoadShapelessRecipesBase("shapeless_crafting.json", recipe => recipe);
	}

	private static void LoadShapelessRecipesBase(string source, Func<ShapelessRecipeBase, ShapelessRecipeBase> getRecipe)
	{
		List<ShapelessRecipeData> shapelessCrafting = ResourceUtil.ReadResource<List<ShapelessRecipeData>>(source, typeof(RecipeManager), "Data");

		foreach (ShapelessRecipeData recipeData in shapelessCrafting)
		{
			var input = recipeData.Input.Select(data =>
			{
				TryGetRecipeIngredientFromExternalData(data, out RecipeIngredient ingredient);

				return ingredient;
			}).ToList();

			if (input.Any(val => val == null))
			{
				Log.Debug($"Missing shapeless recipe Inputs: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			var output = recipeData.Output.Select(data =>
			{
				InventoryUtils.TryGetItemFromExternalData(data, out Item item);

				return item;
			}).ToList();

			if (output.Any(val => val == null))
			{
				Log.Debug($"Missing shapeless recipe Outputs: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			RecipeIngredient[] requirements = recipeData.UnlockingIngredients?.Select(data =>
			{
				TryGetRecipeIngredientFromExternalData(data, out RecipeIngredient ingredient);

				return ingredient;
			}).ToArray() ?? [];

			if (requirements.Any(val => val == null))
			{
				Log.Debug($"Missing shapeless recipe UnlockingIngredients: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			ShapelessRecipeBase recipe = new ShapelessRecipe(output, input, recipeData.Block)
			{
				Priority = recipeData.Priority,
				UniqueId = RecipeUniqueIdCounter++
			};

			recipe.UnlockingRequirement.UnlockingIngredients = requirements;

			recipe = getRecipe(recipe);
			NetworkIdRecipeMap.Add(recipe.UniqueId, recipe);
			IdRecipeMap.Add(recipe.Id, recipe);
			Recipes.Add(recipe);
		}
	}

	private static void LoadShapedChemistryRecipes()
	{
		LoadShapedRecipesBase("shaped_chemistry_asymmetric.json", recipe =>
			new ShapedChemistryRecipe(recipe.Width, recipe.Height, recipe.Output, recipe.Input, recipe.Block)
			{
				Priority = recipe.Priority,
				UniqueId = recipe.UniqueId
			});
	}


	private static void LoadShapedRecipes()
	{
		LoadShapedRecipesBase("shaped_crafting.json", recipe => recipe);
	}

	private static void LoadShapedRecipesBase(string source, Func<ShapedRecipeBase, ShapedRecipeBase> getRecipe)
	{
		List<ShapedRecipeData> shapedCrafting = ResourceUtil.ReadResource<List<ShapedRecipeData>>(source, typeof(RecipeManager), "Data");

		foreach (ShapedRecipeData recipeData in shapedCrafting)
		{
			var input = recipeData.Input.ToDictionary(pair => pair.Key, pair =>
			{
				TryGetRecipeIngredientFromExternalData(pair.Value, out RecipeIngredient ingredient);

				return ingredient;
			});

			if (input.Values.Any(val => val == null))
			{
				Log.Debug($"Missing shaped recipe Inputs: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			var output = recipeData.Output.Select(data =>
			{
				InventoryUtils.TryGetItemFromExternalData(data, out Item item);

				return item;
			}).ToList();

			if (output.Any(val => val == null))
			{
				Log.Debug($"Missing shaped recipe Outputs: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			RecipeIngredient[] requirements = recipeData.UnlockingIngredients?.Select(data =>
			{
				TryGetRecipeIngredientFromExternalData(data, out RecipeIngredient ingredient);

				return ingredient;
			}).ToArray() ?? [];

			if (requirements.Any(val => val == null))
			{
				Log.Debug($"Missing shapeless recipe UnlockingIngredients: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			int height = recipeData.Shape.Length;
			int width = recipeData.Shape.First().Length;


			var inputShape = new List<RecipeIngredient>();
			for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++) 
				inputShape.Add(input.TryGetValue(recipeData.Shape[i][j].ToString(), out RecipeIngredient ingredient) ? ingredient : new RecipeAirIngredient());

			ShapedRecipeBase recipe = new ShapedRecipe(width, height, output, inputShape.ToArray(), recipeData.Block)
			{
				Priority = recipeData.Priority,
				UniqueId = RecipeUniqueIdCounter++
			};

			recipe.UnlockingRequirement.UnlockingIngredients = requirements;

			recipe = getRecipe(recipe);
			NetworkIdRecipeMap.Add(recipe.UniqueId, recipe);
			IdRecipeMap.Add(recipe.Id, recipe);
			Recipes.Add(recipe);
		}
	}

	private static void LoadSmeltingRecipes()
	{
		List<SmeltingRecipeData> shapedCrafting = ResourceUtil.ReadResource<List<SmeltingRecipeData>>("smelting.json", typeof(RecipeManager), "Data");

		foreach (SmeltingRecipeData recipeData in shapedCrafting)
		{
			if (!InventoryUtils.TryGetItemFromExternalData(recipeData.Input, out Item input))
			{
				Log.Debug($"Missing smelting recipe Input: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			if (!InventoryUtils.TryGetItemFromExternalData(recipeData.Output, out Item output))
			{
				Log.Debug($"Missing smelting recipe Output: {JsonConvert.SerializeObject(recipeData)}");

				continue;
			}

			SmeltingRecipeBase recipe = recipeData.Input.Metadata == short.MaxValue ? new SmeltingRecipe() : new SmeltingDataRecipe();

			recipe.Block = recipeData.Block;
			recipe.Input = input;
			recipe.Output = output;

			if (!SmeltingRecipes.TryAdd(recipe.GetHashCode(), recipe)) continue;
			IdRecipeMap.Add(recipe.Id, recipe);
			Recipes.Add(recipe);
		}
	}

	private static bool TryGetRecipeIngredientFromExternalData(ExternalDataItem itemData, out RecipeIngredient recipeIngredient)
	{
		recipeIngredient = null;
		if (!string.IsNullOrEmpty(itemData.Tag))
		{
			if (!ItemFactory.ItemTags.ContainsKey(itemData.Tag)) return false;

			recipeIngredient = new RecipeTagIngredient(itemData.Tag);
			return true;
		}
		if (string.IsNullOrEmpty(itemData.Id)) return false;
		if (!InventoryUtils.TryGetItemFromExternalData(itemData, out Item item)) return false;
		recipeIngredient = new RecipeItemIngredient(item);
		return true;
	}
}