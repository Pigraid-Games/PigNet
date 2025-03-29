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

using System.Collections.Generic;
using System.Linq;
using fNbt;
using PigNet.Items;
using PigNet.Items.Armor;
using PigNet.Items.Food;
using PigNet.Items.Tools;
using PigNet.Items.Weapons;

// ReSharper disable InconsistentNaming

namespace PigNet;

public class creativeGroup(int category, string name, Item icon)
{
	public int Category { get; set; } = category;
	public string Name { get; set; } = name;
	public Item Icon { get; set; } = icon;
}

public class CreativeItemEntry(uint groupIndex, Item item)
{
	public uint GroupIndex { get; set; } = groupIndex;
	public Item Item { get; set; } = item;
}

public static class InventoryUtils
{
	public static List<Item> itemList;
	public static List<CreativeItemEntry> GetCreativeMetadataSlots()
	{
		var keys = CreativeGroups.Keys.ToList();
		var slotData = new List<CreativeItemEntry>();
		foreach (KeyValuePair<string, List<Item>> kvp in CreativeInventoryItems)
		{
			string key = kvp.Key;
			List<Item> items = kvp.Value;

			slotData.AddRange(items.Select(item => new CreativeItemEntry((uint) keys.IndexOf(key), item)));
		}

		itemList ??= CreativeInventoryItems.Values.SelectMany(items => items).ToList();
		return slotData;
	}

	public static List<creativeGroup> GetCreativeGroups()
	{
		return CreativeGroups.Select(group => new creativeGroup(group.Value.Category, group.Value.Name, group.Value.Icon)).ToList();
	}

	public static Dictionary<string, creativeGroup> CreativeGroups = new()
	{
		//Generated with PigNet.Client (creativeGroups.txt)
		{"Planks", new creativeGroup(1, "itemGroup.name.planks", new Item(5, 0))},
		{"Walls", new creativeGroup(1, "itemGroup.name.walls", new Item(139, 0))},
		{"Fence", new creativeGroup(1, "itemGroup.name.fence", new Item(85, 0))},
		{"FenceGate", new creativeGroup(1, "itemGroup.name.fenceGate", new Item(107, 0))},
		{"Stairs", new creativeGroup(1, "itemGroup.name.stairs", new Item(67, 0))},
		{"Door", new creativeGroup(1, "itemGroup.name.door", new Item(324, 0))},
		{"Trapdoor", new creativeGroup(1, "itemGroup.name.trapdoor", new Item(96, 0))},
		{"Construction0", new creativeGroup(1, "", new ItemAir())},
		{"Glass", new creativeGroup(1, "itemGroup.name.glass", new Item(20, 0))},
		{"GlassPane", new creativeGroup(1, "itemGroup.name.glassPane", new Item(102, 0))},
		{"Construction1", new creativeGroup(1, "", new ItemAir())},
		{"Slab", new creativeGroup(1, "itemGroup.name.slab", new Item(44, 0))},
		{"StoneBrick", new creativeGroup(1, "itemGroup.name.stoneBrick", new Item(98, 0))},
		{"Construction2", new creativeGroup(1, "", new ItemAir())},
		{"Sandstone", new creativeGroup(1, "itemGroup.name.sandstone", new Item(24, 0))},
		{"Construction3", new creativeGroup(1, "", new ItemAir())},
		{"Copper", new creativeGroup(1, "itemGroup.name.copper", new Item(-340, 0))},
		{"Construction4", new creativeGroup(1, "", new ItemAir())},
		{"Wool", new creativeGroup(1, "itemGroup.name.wool", new Item(35, 0))},
		{"WoolCarpet", new creativeGroup(1, "itemGroup.name.woolCarpet", new Item(171, 0))},
		{"ConcretePowder", new creativeGroup(1, "itemGroup.name.concretePowder", new Item(237, 0))},
		{"Concrete", new creativeGroup(1, "itemGroup.name.concrete", new Item(236, 0))},
		{"StainedClay", new creativeGroup(1, "itemGroup.name.stainedClay", new Item(172, 0))},
		{"GlazedTerracotta", new creativeGroup(1, "itemGroup.name.glazedTerracotta", new Item(220, 0))},
		{"Construction5", new creativeGroup(1, "", new ItemAir())},
		{"Nature0", new creativeGroup(2, "", new ItemAir())},
		{"Ore", new creativeGroup(2, "itemGroup.name.ore", new Item(15, 0))},
		{"Stone", new creativeGroup(2, "itemGroup.name.stone", new Item(1, 0))},
		{"Nature1", new creativeGroup(2, "", new ItemAir())},
		{"Log", new creativeGroup(2, "itemGroup.name.log", new Item(17, 0))},
		{"Wood", new creativeGroup(2, "itemGroup.name.wood", new Item(-212, 0))},
		{"Leaves", new creativeGroup(2, "itemGroup.name.leaves", new Item(18, 0))},
		{"Sapling", new creativeGroup(2, "itemGroup.name.sapling", new Item(6, 0))},
		{"Nature2", new creativeGroup(2, "", new ItemAir())},
		{"Seed", new creativeGroup(2, "itemGroup.name.seed", new Item(295, 0))},
		{"Crop", new creativeGroup(2, "itemGroup.name.crop", new Item(296, 0))},
		{"Nature3", new creativeGroup(2, "", new ItemAir())},
		{"Grass", new creativeGroup(2, "itemGroup.name.grass", new Item(-848, 0))},
		{"Coral_decorations", new creativeGroup(2, "itemGroup.name.coral_decorations", new Item(-131, 3))},
		{"Flower", new creativeGroup(2, "itemGroup.name.flower", new Item(37, 0))},
		{"Dye", new creativeGroup(2, "itemGroup.name.dye", new Item(351, 11))},
		{"Nature4", new creativeGroup(2, "", new ItemAir())},
		{"RawFood", new creativeGroup(2, "itemGroup.name.rawFood", new Item(365, 0))},
		{"Mushroom", new creativeGroup(2, "itemGroup.name.mushroom", new Item(39, 0))},
		{"Nature5", new creativeGroup(2, "", new ItemAir())},
		{"MonsterStoneEgg", new creativeGroup(2, "itemGroup.name.monsterStoneEgg", new Item(97, 0))},
		{"Nature6", new creativeGroup(2, "", new ItemAir())},
		{"MobEgg", new creativeGroup(2, "itemGroup.name.mobEgg", new Item(383, 10))},
		{"Nature7", new creativeGroup(2, "", new ItemAir())},
		{"Coral", new creativeGroup(2, "itemGroup.name.coral", new Item(-132, 0))},
		{"Sculk", new creativeGroup(2, "itemGroup.name.sculk", new Item(-458, 0))},
		{"Nature8", new creativeGroup(2, "", new ItemAir())},
		{"Helmet", new creativeGroup(3, "itemGroup.name.helmet", new Item(298, 0))},
		{"Chestplate", new creativeGroup(3, "itemGroup.name.chestplate", new Item(299, 0))},
		{"Leggings", new creativeGroup(3, "itemGroup.name.leggings", new Item(300, 0))},
		{"Boots", new creativeGroup(3, "itemGroup.name.boots", new Item(301, 0))},
		{"Sword", new creativeGroup(3, "itemGroup.name.sword", new Item(268, 0))},
		{"Axe", new creativeGroup(3, "itemGroup.name.axe", new Item(271, 0))},
		{"Pickaxe", new creativeGroup(3, "itemGroup.name.pickaxe", new Item(270, 0))},
		{"Shovel", new creativeGroup(3, "itemGroup.name.shovel", new Item(269, 0))},
		{"Hoe", new creativeGroup(3, "itemGroup.name.hoe", new Item(290, 0))},
		{"Equipment0", new creativeGroup(3, "", new ItemAir())},
		{"Arrow", new creativeGroup(3, "itemGroup.name.arrow", new Item(262, 0))},
		{"Equipment1", new creativeGroup(3, "", new ItemAir())},
		{"CookedFood", new creativeGroup(3, "itemGroup.name.cookedFood", new Item(366, 0))},
		{"MiscFood", new creativeGroup(3, "itemGroup.name.miscFood", new Item(297, 0))},
		{"Equipment2", new creativeGroup(3, "", new ItemAir())},
		{"GoatHorn", new creativeGroup(3, "itemGroup.name.goatHorn", new ItemGoatHorn())},
		{"Equipment3", new creativeGroup(3, "", new ItemAir())},
		{"Bundles", new creativeGroup(3, "itemGroup.name.bundles", new ItemBundle())},
		{"HorseArmor", new creativeGroup(3, "itemGroup.name.horseArmor", new Item(416, 0))},
		{"Equipment4", new creativeGroup(3, "", new ItemAir())},
		{"Potion", new creativeGroup(3, "itemGroup.name.potion", new Item(373, 0))},
		{"SplashPotion", new creativeGroup(3, "itemGroup.name.splashPotion", new Item(438, 0))},
		{"LingeringPotion", new creativeGroup(3, "itemGroup.name.lingeringPotion", new Item(441, 0))},
		{"OminousBottle", new creativeGroup(3, "itemGroup.name.ominousBottle", new Item(628, 0))},
		{"Equipment5", new creativeGroup(3, "", new ItemAir())},
		{"Items0", new creativeGroup(4, "", new ItemAir())},
		{"Bed", new creativeGroup(4, "itemGroup.name.bed", new Item(355, 0))},
		{"Items1", new creativeGroup(4, "", new ItemAir())},
		{"Candle", new creativeGroup(4, "itemGroup.name.candles", new Item(-412, 0))},
		{"Items2", new creativeGroup(4, "", new ItemAir())},
		{"Anvil", new creativeGroup(4, "itemGroup.name.anvil", new Item(145, 0))},
		{"Items3", new creativeGroup(4, "", new ItemAir())},
		{"Chest", new creativeGroup(4, "itemGroup.name.chest", new Item(54, 0))},
		{"Items4", new creativeGroup(4, "", new ItemAir())},
		{"ShulkerBox", new creativeGroup(4, "itemGroup.name.shulkerBox", new Item(205, 0))},
		{"Items5", new creativeGroup(4, "", new ItemAir())},
		{"Record", new creativeGroup(4, "itemGroup.name.record", new Item(500, 0))},
		{"Items6", new creativeGroup(4, "", new ItemAir())},
		{"Sign", new creativeGroup(4, "itemGroup.name.sign", new Item(323, 0))},
		{"Hanging_sign", new creativeGroup(4, "itemGroup.name.hanging_sign", new Item(-500, 0))},
		{"Items7", new creativeGroup(4, "", new ItemAir())},
		{"Skull", new creativeGroup(4, "itemGroup.name.skull", new Item(-968, 0))},
		{"Items8", new creativeGroup(4, "", new ItemAir())},
		{"EnchantedBook", new creativeGroup(4, "itemGroup.name.enchantedBook", new Item(403, 0))},
		{"Boat", new creativeGroup(4, "itemGroup.name.boat", new Item(333, 0))},
		{"Chestboat", new creativeGroup(4, "itemGroup.name.chestboat", new Item(675, 0))},
		{"Rail", new creativeGroup(4, "itemGroup.name.rail", new Item(66, 0))},
		{"Minecart", new creativeGroup(4, "itemGroup.name.minecart", new Item(328, 0))},
		{"Items9", new creativeGroup(4, "", new ItemAir())},
		{"Buttons", new creativeGroup(4, "itemGroup.name.buttons", new Item(143, 0))},
		{"Items10", new creativeGroup(4, "", new ItemAir())},
		{"PressurePlate", new creativeGroup(4, "itemGroup.name.pressurePlate", new Item(72, 0))},
		{"Items11", new creativeGroup(4, "", new ItemAir())},
		{"Banner", new creativeGroup(4, "itemGroup.name.banner", new Item(446, 0))},
		{"Banner_pattern", new creativeGroup(4, "itemGroup.name.banner_pattern", new Item(434, 0))},
		{"PotterySherds", new creativeGroup(4, "itemGroup.name.potterySherds", new ItemAnglerPotterySherd())},
		{"SmithingTemplates", new creativeGroup(4, "itemGroup.name.smithing_templates", new ItemNetheriteUpgrade())},
		{"Firework", new creativeGroup(4, "itemGroup.name.firework", new Item(401, 0))},
		{"FireworkStars", new creativeGroup(4, "itemGroup.name.fireworkStars", new Item(402, 0))},
		{"Items12", new creativeGroup(4, "", new ItemAir())},
	};
	
	public static readonly Dictionary<string, List<Item>> CreativeInventoryItems = new() //group name, item
	{
		{"Planks", [
				new Item(5), // oak_planks
				new Item(5, 1), // spruce_planks
				new Item(5, 2), // birch_planks
				new Item(5, 3), // jungle_planks
				new Item(5, 4), // acacia_planks
				new Item(5, 5)  // dark_oak_planks
		]},
		{"Walls", [
				new Item(139, 1), //mossy_cobblestone_wall
				new Item(139, 2), //granite_wall
				new Item(139, 3), //diorite_wall
				new Item(139, 4), //andesite_wall
				new Item(139, 5), //sandstone_wall
				new Item(139, 6), //red_sandstone_wall
				new Item(139, 7), //stone_brick_wall
				new Item(139, 8), //mossy_stone_brick_wall
				new Item(139, 9), //brick_wall
				new Item(139, 10), //nether_brick_wall
				new Item(139, 11), //red_nether_brick_wall
				new Item(139, 12), //end_stone_wall
				new Item(139, 13), //prismarine_wall
		]},
		{"Fence", [
				new Item(85, 0), //oak_fence
				new Item(85, 1), //spruce_fence
				new Item(85, 2), //birch_fence
				new Item(85, 3), //jungle_fence
				new Item(85, 4), //acacia_fence
				new Item(85, 5), //dark_oak_fence
				new Item(113, 0), //nether_brick_fence
		]},
		{"FenceGate", [
				new Item(107, 0), //oak_fence_gate
				new Item(183, 0), //spruce_fence_gate
				new Item(184, 0), //birch_fence_gate
				new Item(185, 0), //jungle_fence_gate
				new Item(187, 0), //acacia_fence_gate
				new Item(186, 0), //dark_oak_fence_gate
		]},
		{"Stairs", [
				new Item(-180, 0), //stone_stairs
				new Item(67, 0), //cobblestone_stairs
				new Item(-179, 0), //mossy_cobblestone_stairs
				new Item(53, 0), //oak_stairs
				new Item(134, 0), //spruce_stairs
				new Item(135, 0), //birch_stairs
				new Item(136, 0), //jungle_stairs
				new Item(163, 0), //acacia_stairs
				new Item(164, 0), //dark_oak_stairs
				new Item(109, 0), //stone_brick_stairs
				new Item(-175, 0), //mossy_stone_brick_stairs
				new Item(128, 0), //sandstone_stairs
				new Item(-177, 0), //smooth_sandstone_stairs
				new Item(180, 0), //red_sandstone_stairs
				new Item(-176, 0), //smooth_red_sandstone_stairs
				new Item(-169, 0), //granite_stairs
				new Item(-172, 0), //polished_granite_stairs
				new Item(-170, 0), //diorite_stairs
				new Item(-173, 0), //polished_diorite_stairs
				new Item(-171, 0), //andesite_stairs
				new Item(-174, 0), //polished_andesite_stairs
				new Item(108, 0), //brick_stairs
				new Item(114, 0), //nether_brick_stairs
				new Item(-184, 0), //red_nether_brick_stairs
				new Item(-178, 0), //end_stone_brick_stairs
				new Item(156, 0), //quartz_stairs
				new Item(-185, 0), //smooth_quartz_stairs
				new Item(203, 0), //purpur_stairs
				new Item(-2, 0), //prismarine_stairs
				new Item(-3, 0), //dark_prismarine_stairs
				new Item(-4, 0), //prismarine_brick_stairs
		]},
		{"Door", [
				new Item(324, 0), //oak_door
				new Item(427, 0), //spruce_door
				new Item(428, 0), //birch_door
				new Item(429, 0), //jungle_door
				new Item(430, 0), //acacia_door
				new Item(431, 0), //dark_oak_door
				new Item(330, 0), //iron_door
		]},
		{"Trapdoor", [
				new Item(96, 0), //oak_trapdoor
				new Item(-149, 0), //spruce_trapdoor
				new Item(-146, 0), //birch_trapdoor
				new Item(-148, 0), //jungle_trapdoor
				new Item(-145, 0), //acacia_trapdoor
				new Item(-147, 0), //dark_oak_trapdoor
				new Item(167, 0), //iron_trapdoor
		]},
		{"Construction0", [
				new Item(101, 0), //iron_bars
		]},
		{"Glass", [
				new Item(20, 0), //glass 
				new Item(241, 0), //white_stained_glass
				new Item(241, 8), //light_gray_stained_glass
				new Item(241, 7), //gray_stained_glass
				new Item(241, 15), //black_stained_glass
				new Item(241, 12), //brown_stained_glass
				new Item(241, 14), //red_stained_glass
				new Item(241, 1), //orange_stained_glass
				new Item(241, 4), //yellow_stained_glass
				new Item(241, 5), //lime_stained_glass
				new Item(241, 13), //green_stained_glass
				new Item(241, 9), //cyan_stained_glass
				new Item(241, 3), //light_blue_stained_glass
				new Item(241, 11), //blue_stained_glass
				new Item(241, 10), //purple_stained_glass
				new Item(241, 2), //magenta_stained_glass
				new Item(241, 6), //pink_stained_glass
		]},
		{"GlassPane", [
				new Item(102, 0), //glass_pane
				new Item(160, 0), //white_stained_glass_pane
				new Item(160, 8), //light_gray_stained_glass_pane
				new Item(160, 7), //gray_stained_glass_pane
				new Item(160, 15), //black_stained_glass_pane
				new Item(160, 12), //brown_stained_glass_pane
				new Item(160, 14), //red_stained_glass_pane
				new Item(160, 1), //orange_stained_glass_pane
				new Item(160, 4), //yellow_stained_glass_pane
				new Item(160, 5), //lime_stained_glass_pane
				new Item(160, 13), //green_stained_glass_pane
				new Item(160, 9), //cyan_stained_glass_pane
				new Item(160, 3), //light_blue_stained_glass_pane
				new Item(160, 11), //blue_stained_glass_pane
				new Item(160, 10), //purple_stained_glass_pane
				new Item(160, 2), //magenta_stained_glass_pane
				new Item(160, 6), //pink_stained_glass_pane
		]},
		{"Construction1", [
				new Item(65, 0), //ladder
				new Item(-165, 0), //scaffolding
				new Item(45, 0), //brick_block
		]},
		{"Slab", [

				new Item(44, 0), //smooth_stone_slab
				new Item(-166, 2), //stone_slab
				new Item(44, 3), //cobblestone_slab
				new Item(182, 5), //mossy_cobblestone_slab
				new Item(44, 2), //oak_slab
				new Item(158, 1), //spruce_slab
				new Item(158, 2), //birch_slab
				new Item(158, 3), //jungle_slab
				new Item(158, 4), //acacia_slab
				new Item(158, 5), //dark_oak_slab
				new Item(44, 5), //stone_brick_slab
				new Item(-166, 0), //mossy_stone_brick_slab
				new Item(44, 1), //sandstone_slab
				new Item(-166, 3), //cut_sandstone_slab
				new Item(182, 6), //smooth_sandstone_slab
				new Item(182, 0), //red_sandstone_slab
				new Item(-166, 4), //cut_red_sandstone_slab
				new Item(-162, 1), //smooth_red_sandstone_slab
				new Item(-162, 6), //granite_slab
				new Item(-162, 7), //polished_granite_slab
				new Item(-162, 4), //diorite_slab
				new Item(-162, 5), //polished_diorite_slab
				new Item(-162, 3), //andesite_slab
				new Item(-162, 2), //polished_andesite_slab
				new Item(44, 4), //bricks_slab
				new Item(44, 7), //nether_brick_slab
				new Item(182, 7), //red_nether_brick_slab
				new Item(-162, 0), //end_stone_brick_slab
				new Item(44, 6), //quartz_slab
				new Item(-166, 1), //smooth_quartz_slab
				new Item(182, 1), //purpur_slab
				new Item(182, 2), //prismarine_slab
				new Item(182, 3), //dark_prismarine_slab
				new Item(182, 4), //prismarine_bricks_slab
		]},
		{"StoneBrick", [
				new Item(98, 0), //stone_bricks
				new Item(98, 1), //mossy_stone_bricks
				new Item(98, 2), //cracked_stone_bricks
				new Item(98, 3), //chiseled_stone_bricks
				new Item(-183, 0), //smooth_stone
				new Item(206, 0), //end_stone_bricks
				new Item(168, 2), //prismarine_bricks
		]},
		{"Construction2", [
				new Item(4, 0), //cobblestone
				new Item(48, 0), //mossy_cobblestone
		]},
		{"Sandstone", [
				new Item(24, 0), //sandstone
				new Item(24, 1), //chiseled_sandstone
				new Item(24, 2), //cut_sandstone
				new Item(24, 3), //smooth_sandstone
				new Item(179, 0), //red_sandstone
				new Item(179, 1), //chiseled_red_sandstone
				new Item(179, 2), //cut_red_sandstone
				new Item(179, 3), //smooth_red_sandstone
		]},
		{"Construction3", [
				new Item(173, 0), //coal_block
				new Item(-139, 0), //dried_kelp_block
				new Item(41, 0), //gold_block
				new Item(42, 0), //iron_block
				new Item(133, 0), //emerald_block
				new Item(57, 0), //diamond_block
				new Item(22, 0), //lapis_lazuli_block
				new Item(155, 0), //quartz_block
				new Item(155, 2), //pillar_quartz_block
				new Item(155, 1), //chiseled_quartz_block
				new Item(155, 3), //smooth_quartz_block
				new Item(168, 0), //prismarine
				new Item(168, 1), //dark_prismarine
				new Item(165, 0), //slime_block
				new Item(170, 0), //haybale
				new Item(216, 0), //bone_block
				new Item(214, 0), //nether_wart_block
				new Item(112, 0), //nether_brick_block
				new Item(215, 0), //red_nether_brick
		]},
		{"Wool", [
				new Item(35, 0), //white_wool
				new Item(35, 8), //light_gray_wool
				new Item(35, 7), //gray_wool
				new Item(35, 15), //black_wool
				new Item(35, 12), //brown_wool
				new Item(35, 14), //red_wool
				new Item(35, 1), //orange_wool
				new Item(35, 4), //yellow_wool
				new Item(35, 5), //lime_wool
				new Item(35, 13), //green_wool
				new Item(35, 9), //cyan_wool
				new Item(35, 3), //light_blue_wool
				new Item(35, 11), //blue_wool
				new Item(35, 10), //purple_wool
				new Item(35, 2), //magenta_wool
				new Item(35, 6), //pink_wool
		]},
		{"WoolCarpet", [
				new Item(171, 0), //white_carpet_carpet
				new Item(171, 8), //light_gray_carpet
				new Item(171, 7), //gray_carpet
				new Item(171, 15), //black_carpet
				new Item(171, 12), //brown_carpet
				new Item(171, 14), //red_carpet
				new Item(171, 1), //orange_carpet
				new Item(171, 4), //yellow_carpet
				new Item(171, 5), //lime_carpet
				new Item(171, 13), //green_carpet
				new Item(171, 9), //cyan_carpet
				new Item(171, 3), //light_blue_carpet
				new Item(171, 11), //blue_carpet
				new Item(171, 10), //purple_carpet
				new Item(171, 2), //magenta_carpet
				new Item(171, 6), //pink_carpet
		]},    
		{"ConcretePowder", [
				new Item(237, 0), //white_concrete_powder
				new Item(237, 8), //light_gray_concrete_powder
				new Item(237, 7), //gray_concrete_powder
				new Item(237, 15), //black_concrete_powder
				new Item(237, 12), //brown_concrete_powder
				new Item(237, 14), //red_concrete_powder
				new Item(237, 1), //orange_concrete_powder
				new Item(237, 4), //yellow_concrete_powder
				new Item(237, 5), //lime_concrete_powder
				new Item(237, 13), //green_concrete_powder
				new Item(237, 9), //cyan_concrete_powder
				new Item(237, 3), //light_blue_concrete_powder
				new Item(237, 11), //blue_concrete_powder
				new Item(237, 10), //purple_concrete_powder
				new Item(237, 2), //magenta_concrete_powder
				new Item(237, 6), //pink_concrete_powder
		]},
		{"Concrete", [
				new Item(236, 0), //white_concrete
				new Item(236, 8), //light_gray_concrete
				new Item(236, 7), //gray_concrete
				new Item(236, 15), //black_concrete
				new Item(236, 12), //brown_concrete
				new Item(236, 14), //red_concrete
				new Item(236, 1), //orange_concrete
				new Item(236, 4), //yellow_concrete
				new Item(236, 5), //lime_concrete
				new Item(236, 13), //green_concrete
				new Item(236, 9), //cyan_concrete
				new Item(236, 3), //light_blue_concrete
				new Item(236, 11), //blue_concrete
				new Item(236, 10), //purple_concrete
				new Item(236, 2), //magenta_concrete
				new Item(236, 6), //pink_concrete
		]},
		{"StainedClay", [
				new Item(82, 0), //clay_block
				new Item(172, 0), //terracotta
				new Item(159, 0), //white_terracotta
				new Item(159, 8), //light_gray_terracotta
				new Item(159, 7), //gray_terracotta
				new Item(159, 15), //black_terracotta
				new Item(159, 12), //brown_terracotta
				new Item(159, 14), //red_terracotta
				new Item(159, 1), //orange_terracotta
				new Item(159, 4), //yellow_terracotta
				new Item(159, 5), //lime_terracotta
				new Item(159, 13), //green_terracotta
				new Item(159, 9), //cyan_terracotta
				new Item(159, 3), //light_blue_terracotta
				new Item(159, 11), //blue_terracotta
				new Item(159, 10), //purple_terracotta
				new Item(159, 2), //magenta_terracotta
				new Item(159, 6), //pink_terracotta
		]},
		{"GlazedTerracotta", [
				new Item(220, 0), //white_glazed_terracotta
				new Item(228, 0), //silver_glazed_terracotta
				new Item(227, 0), //gray_glazed_terracotta
				new Item(235, 0), //black_glazed_terracotta
				new Item(232, 0), //brown_glazed_terracotta
				new Item(234, 0), //red_glazed_terracotta
				new Item(221, 0), //orange_glazed_terracotta
				new Item(224, 0), //yellow_glazed_terracotta
				new Item(225, 0), //lime_glazed_terracotta 
				new Item(233, 0), //green_glazed_terracotta	
				new Item(229, 0), //cyan_glazed_terracotta
				new Item(223, 0), //light_blue_glazed_terracotta
				new Item(231, 0), //blue_glazed_terracotta
				new Item(219, 0), //purple_glazed_terracotta
				new Item(222, 0), //magenta_glazed_terracotta
				new Item(226, 0), //pink_glazed_terracotta
		]},
		{"Construction5", [
				new Item(201, 0), //purpur_block
				new Item(201, 2), //purpur_pillar
		]},
		{"Nature0", [
				new Item(3, 0), //dirt
				new Item(3, 1), //coarse_dirt
				new Item(2, 0), //grass_block
				new Item(198, 0), //dirt_path
				new Item(243, 0), //podzol
				new Item(110, 0), //mycelium
		]},
		{"Ore", [
				new Item(15, 0), //iron_ore
				new Item(14, 0), //gold_ore
				new Item(56, 0), //diamond_ore
				new Item(21, 0), //lapis_lazuli_ore
				new Item(73, 0), //redstone_ore
				new Item(16, 0), //coal_ore
				new Item(129, 0), //emerald_ore
				new Item(153, 0), //nether_quartz_ore
		]},
		{"Stone", [
				new Item(1, 0), //stone
				new Item(1, 1), //granite
				new Item(1, 3), //diorite
				new Item(1, 5), //andesite
				new Item(1, 2), //polished_granite
				new Item(1, 4), //polished_diorite
				new Item(1, 6), //polished_andesite
		]},
		{"Nature1", [
				new Item(13, 0), //gravel
				new Item(12, 0), //sand
				new Item(12, 1), //red_sand
				new Item(81, 0), //cactus
		]},
		{"Log", [
				new Item(17, 0), //oak_log
				new Item(-10, 0), //stripped_oak_log
				new Item(17, 1), //spruce_log
				new Item(-5, 0), //stripped_spruce_log
				new Item(17, 2), //birch_log
				new Item(-6, 0), //stripped_birch_log
				new Item(17, 3), //jungle_log
				new Item(-7, 0), //stripped_jungle_log
				new Item(162, 0), //acacia_log
				new Item(-8, 0), //stripped_acacia_log
				new Item(162, 1), //dark_oak_log
				new Item(-9, 0), //stripped_dark_oak_log
		]},
		{"Wood", [
				new Item(-212, 7), //oak_wood
				new Item(-212, 15), //stripped_oak_wood
				new Item(-212, 1), //spruce_wood
				new Item(-212, 9), //stripped_spruce_wood
				new Item(-212, 2), //birch_wood
				new Item(-212, 10), //stripped_birch_wood
				new Item(-212, 3), //jungle_wood
				new Item(-212, 11), //stripped_jungle_wood
				new Item(-212, 4), //acacia_wood
				new Item(-212, 12), //stripped_acacia_wood
				new Item(-212, 5), //dark_oak_wood
				new Item(-212, 13), //stripped_dark_oak_wood
		]},
		{"Leaves", [
				new Item(18, 0), //oak_leaves
				new Item(18, 1), //spruce_leaves
				new Item(18, 2), //birch_leaves
				new Item(18, 3), //jungle_leaves
				new Item(161, 0), //acacia_leaves
				new Item(161, 1), //dark_oak_leaves
		]},
		{"Sapling", [
				new Item(6, 0), //oak_sapling
				new Item(6, 1), //spruce_sapling
				new Item(6, 2), //birch_sapling
				new Item(6, 3), //jungle_sapling
				new Item(6, 4), //acacia_sapling
				new Item(6, 5), //*dark_oak_sapling
		]},
		{"Seed", [
				new Item(295, 0), //wheat_seeds
				new Item(361, 0), //pumpkin_seeds
				new Item(362, 0), //melon_seeds
				new Item(458, 0), //beetroot_seeds
		]},
		{"Crop", [
				new Item(296, 0), //wheat
				new Item(457, 0), //beetroot
				new Item(392, 0), //potato
				new Item(394, 0), //poisonous_potato
				new Item(391, 0), //carrot
				new Item(396, 0), //golden_carrot
				new Item(260, 0), //apple
				new Item(322, 0), //golden_apple
				new Item(466, 0), //enchanted_golden_apple
				new Item(103, 0), //melon
				new Item(360, 0), //melon_slice
				new Item(382, 0), //glistering_melon_slice
				new Item(477, 0), //sweet_berries
				new Item(86, 0), //pumpkin
		]},
		{"Nature3", [
				new Item(-155, 0), //carved_pumpkin
				new Item(91, 0), //lit_pumpkin
		]},
		{"Grass", [
				new Item(31, 2), //fern
				new Item(175, 11), //large_fern
				new Item(31, 0), //grass
				new Item(175, 10), //tall_grass
		]},
		{"Coral_decorations", [
				new Item(-131, 3), //fire_coral
				new Item(-131, 1), //brain_coral
				new Item(-131, 2), //bubble_coral
				new Item(-131, 0), //tube_coral
				new Item(-131, 4), //horn_coral
				new Item(-131, 11), //dead_fire_coral
				new Item(-131, 9), //dead_brain_coral
				new Item(-131, 10), //dead_bubble_coral
				new Item(-131, 8), //dead_tube_coral
				new Item(-131, 12), //dead_horn_coral
				new Item(-133, 3), //fire_coral_fan
				new Item(-133, 1), //brain_coral_fan
				new Item(-133, 2), //bubble_coral_fan
				new Item(-133, 7), //tube_coral_fan
				new Item(-133, 4), //horn_coral_fan
				new Item(-134, 3), //dead_fire_coral_fan
				new Item(-134, 1), //dead_brain_coral_fan
				new Item(-134, 2), //dead_bubble_coral_fan
				new Item(-134, 7), //dead_tube_coral_fan
				new Item(-134, 4), //dead_horn_coral_fan
		]},
		{"Flower", [
				new Item(37, 0), //dandelion
				new Item(38, 0), //poppy
				new Item(38, 1), //blue_orchid
				new Item(38, 2), //allium
				new Item(38, 3), //azure_bluet
				new Item(38, 4), //red_tulip
				new Item(38, 5), //orange_tulip
				new Item(38, 6), //white_tulip
				new Item(38, 7), //pink_tulip
				new Item(38, 8), //oxeye_daisy
				new Item(38, 9), //cornflower
				new Item(38, 10), //lily_of_the_valley
				new Item(175, 0), //sunflower
				new Item(175, 1), //lilac
				new Item(175, 4), //rose_bush
				new Item(175, 5), //peony
				new Item(-216, 0), //wither_rose
		]},
		{"Dye", [
				new Item(351, 19), //white_dye
				new Item(351, 7), //light_gray_dye
				new Item(351, 8), //gray_dye
				new Item(351, 16), //black_dye
				new Item(351, 17), //brown_dye
				new Item(351, 1), //red_dye
				new Item(351, 14), //orange_dye
				new Item(351, 11), //yellow_dye
				new Item(351, 10), //lime_dye
				new Item(351, 2), //green_dye
				new Item(351, 6), //cyan_dye
				new Item(351, 12), //light_blue_dye
				new Item(351, 18), //blue_dye
				new Item(351, 5), //purple_dye
				new Item(351, 13), //magenta_dye
				new Item(351, 9), //pink_dye
		]},
		{"Nature4", [
				new Item(335, 0), //kelp
				new Item(-130, 0), //seagrass
				new Item(351, 0), //ink_sac
				new Item(351, 3), //cocoa_beans
				new Item(351, 4), //lapis_lazuli
				new Item(351, 15), //bone_meal
				new Item(106, 0), //vines
				new Item(111, 0), //lilypad
				new Item(32, 0), //dead_bush
				new Item(-163, 0), //bamboo
				new Item(80, 0), //snow
				new Item(79, 0), //ice
				new Item(174, 0), //packed_ice
				new Item(-11, 0), //blue_ice
				new Item(78, 0), //top_snow
		]},
		{"RawFood", [
				new Item(365, 0), //raw_chicken
				new Item(319, 0), //porkchop
				new Item(363, 0), //beef
				new Item(423, 0), //mutton
				new Item(411, 0), //raw_rabbit
				new Item(349, 0), //cod
				new Item(460, 0), //salmon
				new Item(461, 0), //tropical_fish
				new Item(462, 0), //pufferfish
		]},
		{"Mushroom", [
				new Item(39, 0), //brown_mushroom
				new Item(40, 0), //red_mushroom
				new Item(99, 14), //brown_mushroom_block
				new Item(100, 14), //red_mushroom_block
				new Item(99, 15), //mushroom_stem
				new Item(99, 0), //mushroom
		]},
		{"Nature5", [
				new Item(344, 0), //egg
				new Item(338, 0), //sugar_canes
				new Item(353, 0), //sugar
				new Item(367, 0), //rotten_flesh
				new Item(352, 0), //bone
				new Item(30, 0), //cobweb
				new Item(375, 0), //spider_eye
				new Item(52, 0), //monster_spawner
		]},
		{"MonsterStoneEgg", [
				new Item(97, 0), //infested_stone
				new Item(97, 1), //infested_cobblestone
				new Item(97, 2), //infested_stone_brick
				new Item(97, 3), //infested_mossy_stone_brick
				new Item(97, 4), //infested_cracked_stone_brick
				new Item(97, 5), //infested_chiseled_stone_brick
		]},
		{"Nature6", [
				new Item(122, 0), //dragon_egg
			]},
		{"MobEgg", [
				new Item(-159, 0), //turtle_spawn_egg
				new Item(383, 10), //chicken_spawn_egg
				new Item(383, 11), //cow_spawn_egg
				new Item(383, 12), //pig_spawn_egg
				new Item(383, 13), //sheep_spawn_egg
				new Item(383, 14), //wolf_spawn_egg
				new Item(383, 28), //polar_bear_spawn_egg
				new Item(383, 22), //ocelot_spawn_egg
				new Item(383, 75), //cat_spawn_egg
				new Item(383, 16), //mooshroom_spawn_egg
				new Item(383, 19), //bat_spawn_egg
				new Item(383, 30), //parrot_spawn_egg
				new Item(383, 18), //rabbit_spawn_egg
				new Item(383, 29), //llama_spawn_egg
				new Item(383, 23), //horse_spawn_egg
				new Item(383, 24), //donkey_spawn_egg
				new Item(383, 25), //mule_spawn_egg
				new Item(383, 26), //skeleton_horse_spawn_egg
				new Item(383, 27), //zombie_horse_spawn_egg
				new Item(383, 111), //tropical_fish_spawn_egg
				new Item(383, 112), //cod_spawn_egg
				new Item(383, 108), //pufferfish_spawn_egg
				new Item(383, 109), //salmon_spawn_egg
				new Item(383, 31), //dolphin_spawn_egg
				new Item(383, 74), //turtle_spawn_egg
				new Item(383, 113), //panda_spawn_egg
				new Item(383, 121), //fox_spawn_egg
				new Item(383, 33), //creeper_spawn_egg
				new Item(383, 38), //enderman_spawn_egg
				new Item(383, 39), //silverfish_spawn_egg
				new Item(383, 34), //skeleton_spawn_egg
				new Item(383, 48), //wither_skeleton_spawn_egg
				new Item(383, 46), //stray_spawn_egg
				new Item(383, 37), //slime_spawn_egg
				new Item(383, 35), //spider_spawn_egg
				new Item(383, 32), //zombie_spawn_egg
				new Item(383, 36), //zombie_pigman_spawn_egg
				new Item(383, 47), //husk_spawn_egg
				new Item(383, 110), //drowned_spawn_egg
				new Item(383, 17), //squid_spawn_egg
				new Item(383, 40), //cave_spider_spawn_egg
				new Item(383, 45), //witch_spawn_egg
				new Item(383, 49), //guardian_spawn_egg
				new Item(383, 50), //elder_guardian_spawn_egg
				new Item(383, 55), //endermite_spawn_egg
				new Item(383, 42), //magma_cube_spawn_egg
				new Item(383, 41), //ghast_spawn_egg
				new Item(383, 43), //blaze_spawn_egg
				new Item(383, 54), //shulker_spawn_egg
				new Item(383, 57), //vindicator_spawn_egg
				new Item(383, 104), //evoker_spawn_egg
				new Item(383, 105), //vex_spawn_egg
				new Item(383, 115), //villager_spawn_egg
				new Item(383, 118), //wandering_trader_spawn_egg
				new Item(383, 116), //zombie_villager_spawn_egg
				new Item(383, 58), //phantom_spawn_egg
				new Item(383, 114), //pillager_spawn_egg
				new Item(383, 59), //ravager_spawn_egg
					]},
		{"Nature7", [
				new Item(49, 0), //obsidian
				new Item(7, 0), //bedrock
				new Item(88, 0), //soul_sand
				new Item(87, 0), //netherrack
				new Item(213, 0), //magma_block
				new Item(372, 0), //nether_wart
				new Item(121, 0), //end_stone
				new Item(200, 0), //chorus_flower
				new Item(240, 0), //chorus_plant
				new Item(432, 0), //chorus_fruit
				new Item(433, 0), //popped_chorus_fruit
				new Item(19, 0), //sponge
				new Item(19, 1), //wet_sponge
					]},
		{"Coral", [
				new Item(-132, 7), //tube_coral_block
				new Item(-132, 1), //brain_coral_block
				new Item(-132, 2), //bubble_coral_block
				new Item(-132, 3), //fire_coral_block
				new Item(-132, 4), //horn_coral_block*
				new Item(-132, 15), //dead_tube_coral_block
				new Item(-132, 9), //dead_brain_coral_block
				new Item(-132, 10), //dead_bubble_coral_block
				new Item(-132, 11), //dead_brain_coral_block
				new Item(-132, 12), //dead_horn_coral_block
					]},
		#region Equipment Items
		{"Helmet", [
				new Item(298, 0), //leather_cap
				new Item(302, 0), //chainmail_helmet
				new Item(306, 0), //iron_helmet
				new Item(314, 0), //golden_helmet
				new Item(310, 0), //diamond_helmet
				new Item(748), //netherite_helmet
					]},
		{"Chestplate", [
				new Item(299, 0), //leather_tunic
				new Item(303, 0), //chainmail_chestplate
				new Item(307, 0), //iron_chestplate
				new Item(315, 0), //golden_chestplate
				new Item(311, 0), //diamond_chestplate
				new Item(749) //netherite_chestplate
					]},
		{"Leggings", [
				new Item(300, 0), //leather_pants
				new Item(304, 0), //chainmail_leggings
				new Item(308, 0), //iron_leggings
				new Item(316, 0), //golden_leggings
				new Item(312, 0), //diamond_leggings
				new Item(750), //netherite_leggings
					]},
		{"Boots", [
				new Item(301, 0), //leather_boots
				new Item(305, 0), //chainmail_boots
				new Item(309, 0), //iron_boots
				new Item(317, 0), //golden_boots
				new Item(313, 0), //diamond_boots
				new Item(751), //netherite_boots
					]},
		{"Sword", [
				new Item(268, 0), //wooden_sword
				new Item(272, 0), //stone_sword
				new Item(267, 0), //iron_sword
				new Item(283, 0), //golden_sword
				new Item(276, 0), //diamond_sword
			    new Item(743), //netherite_sword
					]},
		{"Axe", [
				new Item(271, 0), //wooden_axe
				new Item(275, 0), //stone_axe
				new Item(258, 0), //iron_axe
				new Item(286, 0), //golden_axe
				new Item(279, 0), //diamond_axe
				new Item(746), //netherite_axe
					]},
		{"Pickaxe", [
				new Item(270, 0), //wooden_pickaxe
				new Item(274, 0), //stone_pickaxe
				new Item(257, 0), //iron_pickaxe
				new Item(285, 0), //golden_pickaxe
				new Item(278, 0), //diamond_pickaxe
				new Item(745), //netherite_pickaxe
					]},
		{"Shovel", [
				new Item(269, 0), //wooden_shovel
				new Item(273, 0), //stone_shovel
				new Item(256, 0), //iron_shovel
				new Item(284, 0), //golden_shovel
				new Item(277, 0), //diamond_shovel
				new Item(744), //netherite_shovel
					]},
		{"Hoe", [
				new Item(290, 0), //wooden_hoe
				new Item(291, 0), //stone_hoe
				new Item(292, 0), //iron_hoe
				new Item(294, 0), //golden_hoe
				new Item(293, 0), //diamond_hoe
				new Item(747), //netherite_hoe
		]},
		{"Equipment0", [
				new Item(261, 0), //bow
				new Item(471, 0), //crossbow
				new Item(1047), //mace
					]},
		{"Arrow", [
				new ItemArrow(), //base
				//new ItemArrow(1), //arrow_splashing
				//new ItemArrow(2), //arrow_mundane
				//new ItemArrow(3), //arrow_mundane
				//new ItemArrow(4), //arrow_thick
				//new ItemArrow(5), //arrow_awkward
				new ItemArrow(6), //arrow_nightVision
				new ItemArrow(7), //arrow_nightVision
				new ItemArrow(8), //arrow_invisibility
				new ItemArrow(9), //arrow_invisibility
				new ItemArrow(10), //arrow_jump
				new ItemArrow(11), //arrow_jump
				new ItemArrow(12), //arrow_jump
				new ItemArrow(13), //arrow_fireResistance
				new ItemArrow(14), //arrow_fireResistance
				new ItemArrow(15), //arrow_moveSpeed
				new ItemArrow(16), //arrow_moveSpeed
				new ItemArrow(17), //arrow_moveSpeed
				new ItemArrow(18), //arrow_moveSlowdown
				new ItemArrow(19), //arrow_moveSlowdown
				new ItemArrow(20), //arrow_waterBreathing
				new ItemArrow(21), //arrow_waterBreathing
				new ItemArrow(22), //arrow_heal
				new ItemArrow(23), //arrow_heal
				new ItemArrow(24), //arrow_harm
				new ItemArrow(25), //arrow_harm
				new ItemArrow(26), //arrow_poison
				new ItemArrow(27), //arrow_poison
				new ItemArrow(28), //arrow_poison
				new ItemArrow(29), //arrow_regeneration
				new ItemArrow(30), //arrow_regeneration
				new ItemArrow(31), //arrow_regeneration
				new ItemArrow(32), //arrow_damageBoost
				new ItemArrow(33), //arrow_damageBoost
				new ItemArrow(34), //arrow_damageBoost
				new ItemArrow(35), //arrow_weakness
				new ItemArrow(36), //arrow_weakness
				new ItemArrow(37), //arrow_wither
				new ItemArrow(38), //arrow_turtleMaster
				new ItemArrow(39), //arrow_turtleMaster
				new ItemArrow(40), //arrow_turtleMaster
				new ItemArrow(41), //arrow_slowFalling
				new ItemArrow(42), //arrow_slowFalling
				new ItemArrow(43), //arrow_windCharging
				new ItemArrow(44), //arrow_weaving
				new ItemArrow(45), //arrow_oozing
				new ItemArrow(46), //arrow_infestation
				new ItemArrow(47), // arrow_?
					]},
		{"Equipment1", [
				new Item(513), //shield
		]},

		{"CookedFood", [
				new ItemCookedChicken(),
				new ItemCookedPorkchop(),
				new ItemCookedBeef(),
				new ItemCookedMutton(),
				new ItemCookedRabbit(),
				new ItemCookedCod(),
				new ItemCookedSalmon(),
		]},

		{"MiscFood", [
				new ItemBread(),
				new ItemMushroomStew(),
				new ItemBeetrootSoup(),
				new ItemRabbitStew(),
				new ItemBakedPotato(),
				new ItemCookie(),
				new ItemPumpkinPie(),
				new ItemCake(),
				new ItemDriedKelp()
		]},

		{"Equipment2", [
				new ItemFishingRod(),
				new ItemCarrotOnAStick(),
				new ItemWarpedFungusOnAStick(),
				new ItemSnowball(),
				new ItemWindCharge(),
				new Item(359, 0), //shears
				new Item(259, 0), //flint_and_steel
				new ItemLead(),
				new ItemClock(),
				new ItemCompass(),
				new ItemRecoveryCompass()
		] },
		{"GoatHorn", [
			new ItemGoatHorn(),
			new ItemGoatHorn(ItemGoatHorn.GoatHornType.Sing),
			new ItemGoatHorn(ItemGoatHorn.GoatHornType.Seek),
			new ItemGoatHorn(ItemGoatHorn.GoatHornType.Feel),
			new ItemGoatHorn(ItemGoatHorn.GoatHornType.Admire),
			new ItemGoatHorn(ItemGoatHorn.GoatHornType.Call),
			new ItemGoatHorn(ItemGoatHorn.GoatHornType.Yearn),
			new ItemGoatHorn(ItemGoatHorn.GoatHornType.Dream)
		]},
		{"Equipment3", [
			new ItemEmptyMap(),
			new ItemEmptyMap(2),
			new ItemSaddle()
		]},
		{"Bundles", [
			new ItemBundle(),
			new ItemWhiteBundle(),
			new ItemLightGrayBundle(),
			new ItemGrayBundle(),
			new ItemBlackBundle(),
			new ItemBrownBundle(),
			new ItemRedBundle(),
			new ItemOrangeBundle(),
			new ItemYellowBundle(),
			new ItemLimeBundle(),
			new ItemGreenBundle(),
			new ItemCyanBundle(),
			new ItemBlueBundle(),
			new ItemPurpleBundle(),
			new ItemMagentaBundle(),
			new ItemPinkBundle()
		]},
		{"HorseArmor", [
				new ItemLeatherHorseArmor(),
				new ItemIronHorseArmor(),
				new ItemGoldenHorseArmor(),
				new ItemDiamondHorseArmor()
		] },
		{"Equipment4", [
				new ItemWolfArmor(),
				new ItemTrident(),
				new (469), //turtle_shell
				new ItemElytra(),
				new ItemTotemOfUndying(),
				new ItemGlassBottle(),
				new ItemExperienceBottle()
		] },
		{"Potion", [
				new ItemPotion(0), //potion_emptyPotion
				new ItemPotion(1), //potion_mundane
				new ItemPotion(2), //potion_mundane
				new ItemPotion(3), //potion_thick
				new ItemPotion(4), //potion_awkward
				new ItemPotion(5), //potion_nightVision
				new ItemPotion(6), //potion_nightVision
				new ItemPotion(7), //potion_invisibility
				new ItemPotion(8), //potion_invisibility
				new ItemPotion(9), //potion_jump
				new ItemPotion(10), //potion_jump
				new ItemPotion(11), //potion_jump
				new ItemPotion(12), //potion_fireResistance
				new ItemPotion(13), //potion_fireResistance
				new ItemPotion(14), //potion_moveSpeed
				new ItemPotion(15), //potion_moveSpeed
				new ItemPotion(16), //potion_moveSpeed
				new ItemPotion(17), //potion_moveSlowdown
				new ItemPotion(18), //potion_moveSlowdown
				new ItemPotion(19), //potion_waterBreathing
				new ItemPotion(20), //potion_waterBreathing
				new ItemPotion(21), //potion_heal
				new ItemPotion(22), //potion_heal
				new ItemPotion(23), //potion_harm
				new ItemPotion(24), //potion_harm
				new ItemPotion(25), //potion_poison
				new ItemPotion(26), //potion_poison
				new ItemPotion(27), //potion_poison
				new ItemPotion(28), //potion_regeneration
				new ItemPotion(29), //potion_regeneration
				new ItemPotion(30), //potion_regeneration
				new ItemPotion(31), //potion_damageBoost
				new ItemPotion(32), //potion_damageBoost
				new ItemPotion(33), //potion_damageBoost
				new ItemPotion(34), //potion_weakness
				new ItemPotion(35), //potion_weakness
				new ItemPotion(36), //potion_wither
				new ItemPotion(37), //potion_turtleMaster
				new ItemPotion(38), //potion_turtleMaster
				new ItemPotion(39), //potion_turtleMaster
				new ItemPotion(40), //potion_slowFalling
				new ItemPotion(41), //potion_slowFalling
		] },
		{"SplashPotion", [
				new ItemSplashPotion(), //splash_potion_emptyPotion
				new ItemSplashPotion(1), //splash_potion_mundane
				new ItemSplashPotion(2), //splash_potion_mundane
				new ItemSplashPotion(3), //splash_potion_thick
				new ItemSplashPotion(4), //splash_potion_awkward
				new ItemSplashPotion(5), //splash_potion_nightVision
				new ItemSplashPotion(6), //splash_potion_nightVision
				new ItemSplashPotion(7), //splash_potion_invisibility
				new ItemSplashPotion(8), //splash_potion_invisibility
				new ItemSplashPotion(9), //splash_potion_jump
				new ItemSplashPotion(10), //splash_potion_jump
				new ItemSplashPotion(11), //splash_potion_jump
				new ItemSplashPotion(12), //splash_potion_fireResistance
				new ItemSplashPotion(13), //splash_potion_fireResistance
				new ItemSplashPotion(14), //splash_potion_moveSpeed
				new ItemSplashPotion(15), //splash_potion_moveSpeed
				new ItemSplashPotion(16), //splash_potion_moveSpeed
				new ItemSplashPotion(17), //splash_potion_moveSlowdown
				new ItemSplashPotion(18), //splash_potion_moveSlowdown
				new ItemSplashPotion(19), //splash_potion_waterBreathing
				new ItemSplashPotion(20), //splash_potion_waterBreathing
				new ItemSplashPotion(21), //splash_potion_heal
				new ItemSplashPotion(22), //splash_potion_heal
				new ItemSplashPotion(23), //splash_potion_harm
				new ItemSplashPotion(24), //splash_potion_harm
				new ItemSplashPotion(25), //splash_potion_poison
				new ItemSplashPotion(26), //splash_potion_poison
				new ItemSplashPotion(27), //splash_potion_poison
				new ItemSplashPotion(28), //splash_potion_regeneration
				new ItemSplashPotion(29), //splash_potion_regeneration
				new ItemSplashPotion(30), //splash_potion_regeneration
				new ItemSplashPotion(31), //splash_potion_damageBoost
				new ItemSplashPotion(32), //splash_potion_damageBoost
				new ItemSplashPotion(33), //splash_potion_damageBoost
				new ItemSplashPotion(34), //splash_potion_weakness
				new ItemSplashPotion(35), //splash_potion_weakness
				new ItemSplashPotion(36), //splash_potion_wither
				new ItemSplashPotion(37), //splash_potion_turtleMaster
				new ItemSplashPotion(38), //splash_potion_turtleMaster
				new ItemSplashPotion(39), //splash_potion_turtleMaster
				new ItemSplashPotion(40), //splash_potion_slowFalling
				new ItemSplashPotion(41) //splash_potion_slowFalling
		] },
		{"LingeringPotion", [
				new ItemLingeringPotion(0), //lingering_potion_emptyPotion
				new ItemLingeringPotion(1), //lingering_potion_mundane
				new ItemLingeringPotion(2), //lingering_potion_mundane
				new ItemLingeringPotion(3), //lingering_potion_thick
				new ItemLingeringPotion(4), //lingering_potion_awkward
				new ItemLingeringPotion(5), //lingering_potion_nightVision
				new ItemLingeringPotion(6), //lingering_potion_nightVision
				new ItemLingeringPotion(7), //lingering_potion_invisibility
				new ItemLingeringPotion(8), //lingering_potion_invisibility
				new ItemLingeringPotion(9), //lingering_potion_jump
				new ItemLingeringPotion(10), //lingering_potion_jump
				new ItemLingeringPotion(11), //lingering_potion_jump
				new ItemLingeringPotion(12), //lingering_potion_fireResistance
				new ItemLingeringPotion(13), //lingering_potion_fireResistance
				new ItemLingeringPotion(14), //lingering_potion_moveSpeed
				new ItemLingeringPotion(15), //lingering_potion_moveSpeed
				new ItemLingeringPotion(16), //lingering_potion_moveSpeed
				new ItemLingeringPotion(17), //lingering_potion_moveSlowdown
				new ItemLingeringPotion(18), //lingering_potion_moveSlowdown
				new ItemLingeringPotion(19), //lingering_potion_waterBreathing
				new ItemLingeringPotion(20), //lingering_potion_waterBreathing
				new ItemLingeringPotion(21), //lingering_potion_heal
				new ItemLingeringPotion(22), //lingering_potion_heal
				new ItemLingeringPotion(23), //lingering_potion_harm
				new ItemLingeringPotion(24), //lingering_potion_harm
				new ItemLingeringPotion(25), //lingering_potion_poison
				new ItemLingeringPotion(26), //lingering_potion_poison
				new ItemLingeringPotion(27), //lingering_potion_poison
				new ItemLingeringPotion(28), //lingering_potion_regeneration
				new ItemLingeringPotion(29), //lingering_potion_regeneration
				new ItemLingeringPotion(30), //lingering_potion_regeneration
				new ItemLingeringPotion(31), //lingering_potion_damageBoost
				new ItemLingeringPotion(32), //lingering_potion_damageBoost
				new ItemLingeringPotion(33), //lingering_potion_damageBoost
				new ItemLingeringPotion(34), //lingering_potion_weakness
				new ItemLingeringPotion(35), //lingering_potion_weakness
				new ItemLingeringPotion(36), //lingering_potion_wither
				new ItemLingeringPotion(37), //lingering_potion_turtleMaster
				new ItemLingeringPotion(38), //lingering_potion_turtleMaster
				new ItemLingeringPotion(39), //lingering_potion_turtleMaster
				new ItemLingeringPotion(40), //lingering_potion_slowFalling
				new ItemLingeringPotion(41) //lingering_potion_slowFalling
		] },
		{"OminousBottles", [
			new ItemOminousBottle(), //ominous_bottle lvl 1
			new ItemOminousBottle(1), //ominous_bottle lvl 2
			new ItemOminousBottle(2), //ominous_bottle lvl 3
			new ItemOminousBottle(3), //ominous_bottle lvl 4
			new ItemOminousBottle(4), //ominous_bottle lvl 5
		]},
		{"Equipment5", [
			new ItemSpyglass(),
			new ItemBrush(),
		]},
		#endregion Equipment Items
		#region Items
		{"Items0", [ new ItemStick() ] }, // Stick
		{"Bed", [
			new ItemBed(), //bed_white
			new ItemBed(8), //bed_light_gray
			new ItemBed(7), //bed_gray
			new ItemBed(15), //bed_black
			new ItemBed(12), //bed_brown
			new ItemBed(14), //bed_red
			new ItemBed(1), //bed_oraange
			new ItemBed(4), //bed_yellow
			new ItemBed(5), //bed_lime
			new ItemBed(13), //bed_green
			new ItemBed(9), //bed_cyan
			new ItemBed(3), //bed_light_blue
			new ItemBed(11), //bed_blue
			new ItemBed(10), //bed_purple
			new ItemBed(2), //bed_magenta
			new ItemBed(6), //bed_pink
		] },
		{"Items1", [
			new ItemTorch(),
			new ItemSoulTorch(),
			new ItemSeaPickle(),
			new ItemLantern(),
			new ItemSoulLantern(),
		]},
		{"Candle", [
			new ItemCandle(),
			new ItemWhiteCandle(),
			new ItemOrangeCandle(),
			new ItemPinkCandle(),
			new ItemLightBlueCandle(),
			new ItemYellowCandle(),
			new ItemLimeCandle(),
			new ItemPinkCandle(),
			new ItemGrayCandle(),
			new ItemLightGrayCandle(),
			new ItemCyanCandle(),
			new ItemPurpleCandle(),
			new ItemBrownCandle(),
			new ItemGreenCandle(),
			new ItemRedCandle(),
			new ItemBlackCandle(),
		]},
		{"Items2", [
			new ItemCraftingTable(),
			new ItemCartographyTable(),
			new ItemFletchingTable(),
			new ItemSmitchingTable(),
			new ItemCampfire(),
			new ItemSoulCampfire(),
			new ItemFurnace(),
			new ItemBlastFurnace(),
			new ItemSmoker(),
			new ItemRespawnAnchor(),
			new ItemBrewingStand(),
		]},
		{"Anvil", [
			new ItemAnvil(),
			new ItemChippedAnvil(),
			new ItemDamagedAnvil(),
		]},

		{"Items3", [
			new ItemGrindstone(),
			new ItemEnchantingTable(),
			new ItemBookshelf(),
			new ItemChiseledBookshelf(),
			new ItemLectern(),
			new ItemCauldron(),
			new ItemComposter(),
		]},

		{"Chest", [
			new ItemChest(),
			new ItemTrappedChest(),
			new ItemEnderChest(),
		]},

		{"Items4", [
			new ItemBarrel(),
		]},

		{"ShulkerBox", [
			new ItemShulkerBox(),
			new ItemWhiteShulkerBox(),
			new ItemLightGrayShulkerBox(),
			new ItemGrayShulkerBox(),
			new ItemBlackShulkerBox(),
			new ItemBrownShulkerBox(),
			new ItemRedShulkerBox(),
			new ItemOrangeShulkerBox(),
			new ItemYellowShulkerBox(),
			new ItemLimeShulkerBox(),
			new ItemGreenShulkerBox(),
			new ItemCyanShulkerBox(),
			new ItemLightBlueShulkerBox(),
			new ItemBlueShulkerBox(),
			new ItemPurpleShulkerBox(),
			new ItemMagentaShulkerBox(),
			new ItemPinkShulkerBox(),
		]},

		{"Items5", [
			new ItemArmorStand(),
			new ItemNoteBlock(),
			new ItemJukebox(),
		]},

		{"Record", [
			new ItemMusicDisc13(),
			new ItemMusicDiscCat(),
			new ItemMusicDiscBlocks(),
			new ItemMusicDiscChirp(),
			new ItemMusicDiscFar(),
			new ItemMusicDiscMall(),
			new ItemMusicDiscMellohi(),
			new ItemMusicDiscStal(),
			new ItemMusicDiscStrad(),
			new ItemMusicDiscWard(),
			new ItemMusicDisc11(),
			new ItemMusicDiscWait(),
			new ItemMusicDiscOtherside(),
			new ItemMusicDisc5(),
			new ItemMusicDiscPigstep(),
			new ItemMusicDiscRelic(),
			new ItemMusicDiscCreator(),
			new ItemMusicDiscCreatorMusicBox(),
			new ItemMusicDiscPrecipice(),
		]},
		{"Items6", [
			new ItemMusicDiscFragment5(),
			new ItemGlowstoneDust(),
			new ItemGlowstone(),
			new ItemRedstoneLamp(),
			new ItemSeaLantern(),
		]},

		{"Sign", [
			new ItemSign(),
			new ItemSpruceSign(),
			new ItemBirchSign(),
			new ItemJungleSign(),
			new ItemAcaciaSign(),
			new ItemDarkOakSign(),
			new ItemMangroveSign(),
			new ItemCherrySign(),
			new ItemPaleOakSign(),
			new ItemBambooSign(),
			new ItemCrimsonSign(),
			new ItemWarpedSign(),
		]},

		{"Items7", [
			new ItemPainting(),
			new ItemFrame(),
			new ItemHoneyBottle(),
			new ItemFlowerPot(),
			new ItemBowl(),
			new ItemBucket(0), //bucket
			new ItemBucket(1), //milk
			new ItemBucket(8), //water
			new ItemBucket(10), //lava
			new ItemBucket(2), //cod
			new ItemBucket(3), //salmon
			new ItemBucket(4), //tropical_fish
			new ItemBucket(5), // pufferfish,
			new ItemBucket(11), //powder snow
			new ItemBucket(12), //axolotl
			new ItemBucket(13), //tadpole
		]},

		{"Skull", [
			new ItemPlayerHead(),
			new ItemZombieHead(),
			new ItemCreeperHead(),
			new ItemDragonHead(),
			new ItemSkeletonHead(),
			new ItemWitherSkeletonSkull(),
			new ItemPiglinHead(),
		]},

		{"Items8", [
			new ItemBeacon(),
			new ItemBell(),
			new ItemConduit(),
			new ItemStonecutter(),
			new ItemCoal(),
			new ItemCharcoal(),
			new ItemDiamond(),
			new ItemIronNugget(),
			new ItemRawIron(),
			new ItemRawGold(),
			new ItemRawCopper(),
			new ItemCopperIngot(),
			new ItemIronIngot(),
			new ItemNetheriteScrap(),
			new ItemNetheriteIngot(),
			new ItemGoldNugget(),
			new ItemGoldIngot(),
			new ItemEmerald(),
			new ItemNetherQuartz(),
			new ItemClayBall(),
			new ItemBrick(),
			new ItemNetherbrick(),
			new ItemResinBrick(),
			new ItemPrismarineShard(),
			new ItemAmethystShard(),
			new ItemPrismarineCrystals(),
			new ItemNautilusShell(),
			new ItemHeartOfTheSea(),
			new ItemTurtleScute(),
			new ItemArmadilloScute(),
			new ItemPhantomMembrane(),
			new ItemString(),
			new ItemFeather(),
			new ItemFlint(),
			new ItemGunpowder(),
			new ItemLeather(),
			new ItemRabbitHide(),
			new ItemRabbitFoot(),
			new ItemFireCharge(),
			new ItemBlazeRod(),
			new ItemBreezeRod(),
			new ItemHeavyCore(),
			new ItemBlazePowder(),
			new ItemMagmaCream(),
			new ItemFermentedSpiderEye(),
			new ItemEchoShard(),
			new ItemDragonBreath(),
			new ItemShulkerShell(),
			new ItemGhastTear(),
			new ItemSlimeBall(),
			new ItemEnderPearl(),
			new ItemEnderEye(),
			new ItemNetherstar(),
			new ItemEndRod(),
			new ItemLightningRod(),
			new ItemEndCrystal(),
			new ItemPaper(),
			new ItemBook(),
			new ItemWritableBook(),
		]},

		{"EnchantedBook", [
				new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 1) } } } }, //enchanted_book_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 2) } } } }, //enchanted_book_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 3) } } } }, //enchanted_book_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 4) } } } }, //enchanted_book_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 1) } } } }, //enchanted_book_fire_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 2) } } } }, //enchanted_book_fire_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 3) } } } }, //enchanted_book_fire_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 4) } } } }, //enchanted_book_fire_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 1) } } } }, //enchanted_book_feather_falling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 2) } } } }, //enchanted_book_feather_falling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 3) } } } }, //enchanted_book_feather_falling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 4) } } } }, //enchanted_book_feather_falling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 1) } } } }, //enchanted_book_blast_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 2) } } } }, //enchanted_book_blast_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 3) } } } }, //enchanted_book_blast_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 4) } } } }, //enchanted_book_blast_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 1) } } } }, //enchanted_book_projectile_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 2) } } } }, //enchanted_book_projectile_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 3) } } } }, //enchanted_book_projectile_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 4) } } } }, //enchanted_book_projectile_protection
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 5), new NbtShort("lvl", 1) } } } }, //enchanted_book_thorns
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 5), new NbtShort("lvl", 2) } } } }, //enchanted_book_thorns
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 5), new NbtShort("lvl", 3) } } } }, //enchanted_book_thorns
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 6), new NbtShort("lvl", 1) } } } }, //enchanted_book_respiration
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 6), new NbtShort("lvl", 2) } } } }, //enchanted_book_respiration
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 6), new NbtShort("lvl", 3) } } } }, //enchanted_book_respiration
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 7), new NbtShort("lvl", 1) } } } }, //enchanted_book_depth_strider
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 7), new NbtShort("lvl", 2) } } } }, //enchanted_book_depth_strider
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 7), new NbtShort("lvl", 3) } } } }, //enchanted_book_depth_strider
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 8), new NbtShort("lvl", 1) } } } }, //enchanted_book_aqua_affinity
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 1) } } } }, //enchanted_book_sharpness
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 2) } } } }, //enchanted_book_sharpness
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 3) } } } }, //enchanted_book_sharpness
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 4) } } } }, //enchanted_book_sharpness
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 5) } } } }, //enchanted_book_sharpness
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 1) } } } }, //enchanted_book_smite
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 2) } } } }, //enchanted_book_smite
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 3) } } } }, //enchanted_book_smite
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 4) } } } }, //enchanted_book_smite
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 5) } } } }, //enchanted_book_smite
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 1) } } } }, //enchanted_book_bane_of_arthropods
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 2) } } } }, //enchanted_book_bane_of_arthropods
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 3) } } } }, //enchanted_book_bane_of_arthropods
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 4) } } } }, //enchanted_book_bane_of_arthropods
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 5) } } } }, //enchanted_book_bane_of_arthropods
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 12), new NbtShort("lvl", 1) } } } }, //enchanted_book_knockback
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 12), new NbtShort("lvl", 2) } } } }, //enchanted_book_knockback
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 13), new NbtShort("lvl", 1) } } } }, //enchanted_book_fire_aspect
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 13), new NbtShort("lvl", 2) } } } }, //enchanted_book_fire_aspect
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 14), new NbtShort("lvl", 1) } } } }, //enchanted_book_looting
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 14), new NbtShort("lvl", 2) } } } }, //enchanted_book_looting
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 14), new NbtShort("lvl", 3) } } } }, //enchanted_book_looting
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 1) } } } }, //enchanted_book_efficiency
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 2) } } } }, //enchanted_book_efficiency
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 3) } } } }, //enchanted_book_efficiency
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 4) } } } }, //enchanted_book_efficiency
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 5) } } } }, //enchanted_book_efficiency
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 16), new NbtShort("lvl", 1) } } } }, //enchanted_book_silk_touch
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 17), new NbtShort("lvl", 1) } } } }, //enchanted_book_unbreaking
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 17), new NbtShort("lvl", 2) } } } }, //enchanted_book_unbreaking
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 17), new NbtShort("lvl", 3) } } } }, //enchanted_book_unbreaking
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 18), new NbtShort("lvl", 1) } } } }, //enchanted_book_fortune
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 18), new NbtShort("lvl", 2) } } } }, //enchanted_book_fortune
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 18), new NbtShort("lvl", 3) } } } }, //enchanted_book_fortune
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 1) } } } }, //enchanted_book_power
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 2) } } } }, //enchanted_book_power
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 3) } } } }, //enchanted_book_power
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 4) } } } }, //enchanted_book_power
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 5) } } } }, //enchanted_book_power
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 20), new NbtShort("lvl", 1) } } } }, //enchanted_book_punch
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 20), new NbtShort("lvl", 2) } } } }, //enchanted_book_punch
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 21), new NbtShort("lvl", 1) } } } }, //enchanted_book_flame
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 22), new NbtShort("lvl", 1) } } } }, //enchanted_book_infinity
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 23), new NbtShort("lvl", 1) } } } }, //enchanted_book_luck_of_the_sea
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 23), new NbtShort("lvl", 2) } } } }, //enchanted_book_luck_of_the_sea
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 23), new NbtShort("lvl", 3) } } } }, //enchanted_book_luck_of_the_sea
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 24), new NbtShort("lvl", 1) } } } }, //enchanted_book_lure
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 24), new NbtShort("lvl", 2) } } } }, //enchanted_book_lure
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 24), new NbtShort("lvl", 3) } } } }, //enchanted_book_lure
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 25), new NbtShort("lvl", 1) } } } }, //enchanted_book_frost_walker
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 25), new NbtShort("lvl", 2) } } } }, //enchanted_book_frost_walker
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 26), new NbtShort("lvl", 1) } } } }, //enchanted_book_mending
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 1) } } } }, //enchanted_book_impaling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 2) } } } }, //enchanted_book_impaling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 3) } } } }, //enchanted_book_impaling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 4) } } } }, //enchanted_book_impaling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 5) } } } }, //enchanted_book_impaling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 30), new NbtShort("lvl", 1) } } } }, //enchanted_book_riptide
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 30), new NbtShort("lvl", 2) } } } }, //enchanted_book_riptide
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 30), new NbtShort("lvl", 3) } } } }, //enchanted_book_riptide
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 31), new NbtShort("lvl", 1) } } } }, //enchanted_book_loyalty
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 31), new NbtShort("lvl", 2) } } } }, //enchanted_book_loyalty
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 31), new NbtShort("lvl", 3) } } } }, //enchanted_book_loyalty
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 32), new NbtShort("lvl", 1) } } } }, //enchanted_book_channeling
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 33), new NbtShort("lvl", 1) } } } }, //enchanted_book_multishot
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 1) } } } }, //enchanted_book_piercing
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 2) } } } }, //enchanted_book_piercing
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 3) } } } }, //enchanted_book_piercing
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 4) } } } }, //enchanted_book_piercing
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 35), new NbtShort("lvl", 1) } } } }, //enchanted_book_quick_charge
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 35), new NbtShort("lvl", 2) } } } }, //enchanted_book_quick_charge
		new Item(403, 0){ ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 35), new NbtShort("lvl", 3) } } } }, //enchanted_book_quick_charge
		]},
		{"Boat", [
			new Item(333, 0), //boat_oak
			new Item(333, 1), //boat_spruce
			new Item(333, 2), //boat_birch
			new Item(333, 3), //boat_jungle
			new Item(333, 4), //boat_acacia
			new Item(333, 5), //boat_dark_oak
		]},

		{"Rail", [
			new Item(66, 0), //rail
			new Item(27, 0), //powered_rail
			new Item(28, 0), //detector_rail
			new Item(126, 0), //activator_rail
		]},

		{"Minecart", [
			new Item(328, 0), //minecart
			new Item(342, 0), //chest_minecart
			new Item(408, 0), //hopper_minecart
			new Item(407, 0), //tnt_minecart
		]},

		{"Items9", [
			new Item(331, 0), //redstone
			new Item(152, 0), //redstone_block
			new Item(76, 0), //redstone_torch
			new Item(69, 0), //lever
		]},

		{"Buttons", [
			new Item(143, 0), //oak_button
			new Item(-144, 0), //spruce_button
			new Item(-141, 0), //birch_button
			new Item(-143, 0), //jungle_button
			new Item(-140, 0), //acacia_button
			new Item(-142, 0), //dark_oak_button
			new Item(77, 0), //stone_button
		]},

		{"Items10", [
			new Item(131, 0), //tripwire_hook
		]},

		{"PressurePlate", [
				new Item(72, 0), //oak_pressure_plate
				new Item(-154, 0), //spruce_pressure_plate
				new Item(-151, 0), //birch_pressure_plate
				new Item(-153, 0), //jungle_pressure_plate
				new Item(-150, 0), //acacia_pressure_plate
				new Item(-152, 0), //dark_oak_pressure_plate
				new Item(70, 0), //stone_pressure_plate
				new Item(147, 0), //light_weight_pressure_plate
				new Item(148, 0), //heavy_weight_pressure_plate
		]},

		{"Items11", [
			new Item(251, 0), //observer
			new Item(151, 0), //daylight_sensor
			new Item(356, 0), //repeater
			new Item(404, 0), //comparator
			new Item(410, 0), //hopper
			new Item(125, 0), //dropper
			new Item(23, 0), //dispenser
			new Item(33, 0), //piston
			new Item(29, 0), //sticky_piston
			new Item(46, 0), //tnt
			new Item(421, 0), //name_tag
			new Item(-204, 0), //loom
		]},

		{"Banner", [
			new Item(446, 0), //banner_white
			new Item(446, 8), //banner_light_gray
			new Item(446, 7), //banner_gray
			new Item(446, 15), //banner_black
			new Item(446, 12), //banner_brown
			new Item(446, 14), //banner_red
			new Item(446, 1), //banner_orange
			new Item(446, 4), //banner_yellow
			new Item(446, 5), //banner_lime
			new Item(446, 13), //banner_green
			new Item(446, 9), //banner_cyan
			new Item(446, 3), //banner_light_blue
			new Item(446, 11), //banner_blue
			new Item(446, 10), //banner_purple
			new Item(446, 2), //banner_magenta
			new Item(446, 6), //banner_pink
			new Item(446, 15){ ExtraData = new NbtCompound { new NbtInt("Type", 1) } }, //illager_banner
		]},

		{"Banner_pattern", [
			new Item(434, 0), //creeper_charge_banner_pattern ok
			new Item(434, 1), //skull_charge_banner_pattern ok
			new Item(434, 2), //flower_charge_banner_pattern ok
			new Item(434, 3), //thing_banner_pattern ok
			new Item(434, 4), //field_masoned_banner_pattern ok
			new Item(434, 5), //bordure_intented_banner_pattern ok
			new Item(434, 6), //snout_banner_pattern ok
			new Item(434, 7), //globe_banner_pattern ok
			new Item(1069), //flow_banner_pattern
			new Item(1070), //guster_banner_pattern
		]},
		{"PotterySherds", [
			new ItemAnglerPotterySherd(),
			new ItemArcherPotterySherd(),
			new ItemArmsUpPotterySherd(),
			new ItemBladePotterySherd(),
			new ItemBrewerPotterySherd(),
			new ItemBurnPotterySherd(),
			new ItemDangerPotterySherd(),
			new ItemExplorerPotterySherd(),
			new ItemFlowPotterySherd(),
			new ItemFriendPotterySherd(),
			new ItemGusterPotterySherd(),
			new ItemHeartPotterySherd(),
			new ItemHeartbreakPotterySherd(),
			new ItemHowlPotterySherd(),
			new ItemMinerPotterySherd(),
			new ItemMournerPotterySherd(),
			new ItemPlentyPotterySherd(),
			new ItemPrizePotterySherd(),
			new ItemScrapePotterySherd(),
			new ItemSheafPotterySherd(),
			new ItemShelterPotterySherd(),
			new ItemSkullPotterySherd(),
			new ItemSnortPotteryShert(),
		]},
		{"SmithingTemplates", [
			new ItemNetheriteUpgrade(),
			new ItemSentryArmorTrim(),
			new ItemVexArmorTrim(),
			new ItemWildArmorTrim(),
			new ItemCoastArmorTrim(),
			new ItemDuneArmorDrim(),
			new ItemWayfinderArmorTrim(),
			new ItemShaperArmorTrim(),
			new ItemRaiserArmorTrim(),
			new ItemHostArmorTrim(),
			new ItemWardArmorTrim(),
			new ItemSilenceArmorTrim(),
			new ItemTideArmorTrim(),
			new ItemSnoutArmorTrim(),
			new ItemRibArmorTrim(),
			new ItemEyeArmorTrim(),
			new ItemSpireArmorTrim(),
			new ItemFlowArmorTrim(),
			new ItemBoltArmorTrim(),
		]},
		{"Firework", [
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)0), new NbtByte("Flight") } } }, //firework_rocket
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{0}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_white
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{8}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_light_gray
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{7}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_gray
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{15}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_black
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{12}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_brown
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{14}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_red
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{1}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_orange
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{4}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_yellow
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{5}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_lime
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{13}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_green
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{9}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_cyan
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{3}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_light_blue
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{11}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_blue
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{10}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_purple
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{2}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_magenta
				new Item(401, 0){ ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{6}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, //firework_rocket_pink
           
		]},

		{"FireworkStars", [
				new Item(402, 0){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{0}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -14869215) } }, //firework_star_white
				new Item(402, 8){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{8}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -12103854) } }, //firework_star_light_gray
				new Item(402, 7){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{7}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -6447721) } }, //firework_star_gray
				new Item(402, 15){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{15}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -986896) } }, //firework_star_black
				new Item(402, 12){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{12}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -12930086) } }, //firework_star_brown
				new Item(402, 14){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{14}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -425955) } }, //firework_star_red
				new Item(402, 1){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{1}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -5231066) } }, //firework_star_orange
				new Item(402, 4){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{4}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -12827478) } }, //firework_star_yellow
				new Item(402, 5){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{5}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -7785800) } }, //firework_star_lime
				new Item(402, 13){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{13}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -3715395) } }, //firework_star_green
				new Item(402, 9){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{9}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -816214) } }, //firework_star_cyan
				new Item(402, 3){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{3}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -8170446) } }, //firework_star_light_blue
				new Item(402, 11){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{11}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -75715) } }, //firework_star_blue
				new Item(402, 10){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{10}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -8337633) } }, //firework_star_purple
				new Item(402, 2){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{2}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -10585066) } }, //firework_star_magenta
				new Item(402, 6){ ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{6}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -15295332) } }, //firework_star_pink
		
		]},
		{"Items12", [
			new ItemChain(),
			new ItemTarget(),
			new ItemDecoratedPot(),
			new ItemTrialKey(),
			new ItemOminousTrialKey()
		]}
		#endregion Items
	};
}