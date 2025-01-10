using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET.Blocks;
using MiNET.Items.Armor;
using MiNET.Items.Custom;
using MiNET.Items.Food;
using MiNET.Items.Tools;
using MiNET.Items.Weapons;
using MiNET.Net.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items;

public interface ICustomItemFactory
{
	Item GetItem(short id, short metadata, int count);
}

public interface ICustomBlockItemFactory
{
	ItemBlock GetBlockItem(Block block, short metadata, int count);
}

public class ItemFactory
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFactory));

	public static ICustomItemFactory CustomItemFactory { get; set; }
	public static ICustomBlockItemFactory CustomBlockItemFactory { get; set; }

	public static Dictionary<string, short> NameToId { get; private set; }
	public static Itemstates Itemstates { get; internal set; }

	public static ItemTranslator Translator { get; }

	static ItemFactory()
	{
		NameToId = BuildNameToId();

		Itemstates = ResourceUtil.ReadResource<Itemstates>("itemstates.json", typeof(Item));
		Translator = new ItemTranslator(Itemstates);
	}

	private static Dictionary<string, short> BuildNameToId()
	{
		var nameToId = new Dictionary<string, short>();

		for (short idx = -600; idx < 1100; idx++)
			try
			{
				Item item = GetItem(idx);
				if (item == null) continue;

				string name = item.GetType().Name.ToLowerInvariant();

				switch (name)
				{
					case "item": continue;
					case "itemblock":
						if (item is ItemBlock itemBlock)
						{
							Block block = itemBlock.Block;
							name = block?.GetType().Name.ToLowerInvariant();
							if (string.IsNullOrEmpty(name) || name == "block") continue;
						}
						break;
					default:
						if (name.StartsWith("item"))
							name = name[4..];
						break;
				}

				nameToId.TryAdd(name, idx);

				if (string.IsNullOrWhiteSpace(item.Name)) continue;
				string itemName = item.Name.ToLowerInvariant();
				nameToId.TryAdd(itemName, idx);
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to process item ID {idx}: {ex.Message}", ex);
			}
		return nameToId;
	}

	public static short GetItemIdByName(string itemName)
	{
		itemName = itemName.ToLowerInvariant().Replace("_", "").Replace("minecraft:", "");
		if (NameToId.TryGetValue(itemName, out short name)) return name;
		return (short) BlockFactory.GetBlockIdByName(itemName);
	}

	public static Item GetItem(string name, short metadata = 0, int count = 1)
	{
		return GetItem(GetItemIdByName(name), metadata, count);
	}

	public static void RegisterItem(short id, Item item)
	{
		ItemFactories.TryAdd(id, (_, _) => item);
	}

	public static void UnregisterItem(short id)
	{
		ItemFactories.Remove(id);
	}

	public static void UnregisterITem(Item item)
	{
		short result = GetItemIdByName(item.Name);
		UnregisterItem(result);
	}

	private static readonly Dictionary<short, Func<short, int, Item>> ItemFactories = new()
	{
		{ -428, (_, _) => new ItemBlackCandle() },
		{ -427, (_, _) => new ItemRedCandle() },
		{ -426, (_, _) => new ItemGreenCandle() },
		{ -425, (_, _) => new ItemBrownCandle() },
		{ -424, (_, _) => new ItemBlueCandle() },
		{ -423, (_, _) => new ItemPurpleCandle() },
		{ -422, (_, _) => new ItemCyanCandle() },
		{ -421, (_, _) => new ItemLightGrayCandle() },
		{ -420, (_, _) => new ItemGrayCandle() },
		{ -419, (_, _) => new ItemPinkCandle() },
		{ -418, (_, _) => new ItemLimeCandle() },
		{ -417, (_, _) => new ItemYellowCandle() },
		{ -416, (_, _) => new ItemLightBlueCandle() },
		{ -415, (_, _) => new ItemMagentaCandle() },
		{ -414, (_, _) => new ItemOrangeCandle() },
		{ -413, (_, _) => new ItemWhiteCandle() },
		{ -412, (_, _) => new ItemCandle() },
		{ 0, (_, _) => new ItemAir() },
		{ 15, (_, _) => new ItemBoneMeal() },
		{ 256, (_, _) => new ItemIronShovel() },
		{ 257, (_, _) => new ItemIronPickaxe() },
		{ 258, (_, _) => new ItemIronAxe() },
		{ 259, (_, _) => new ItemFlintAndSteel() },
		{ 260, (_, _) => new ItemApple() },
		{ 261, (_, _) => new ItemBow() },
		{ 262, (_, _) => new ItemArrow() },
		{ 263, (_, _) => new ItemCoal() },
		{ 264, (_, _) => new ItemDiamond() },
		{ 265, (_, _) => new ItemIronIngot() },
		{ 266, (_, _) => new ItemGoldIngot() },
		{ 267, (_, _) => new ItemIronSword() },
		{ 268, (_, _) => new ItemWoodenSword() },
		{ 269, (_, _) => new ItemWoodenShovel() },
		{ 270, (_, _) => new ItemWoodenPickaxe() },
		{ 271, (_, _) => new ItemWoodenAxe() },
		{ 272, (_, _) => new ItemStoneSword() },
		{ 273, (_, _) => new ItemStoneShovel() },
		{ 274, (_, _) => new ItemStonePickaxe() },
		{ 275, (_, _) => new ItemStoneAxe() },
		{ 276, (_, _) => new ItemDiamondSword() },
		{ 277, (_, _) => new ItemDiamondShovel() },
		{ 278, (_, _) => new ItemDiamondPickaxe() },
		{ 279, (_, _) => new ItemDiamondAxe() },
		{ 280, (_, _) => new ItemStick() },
		{ 281, (_, _) => new ItemBowl() },
		{ 282, (_, _) => new ItemMushroomStew() },
		{ 283, (_, _) => new ItemGoldenSword() },
		{ 284, (_, _) => new ItemGoldenShovel() },
		{ 285, (_, _) => new ItemGoldenPickaxe() },
		{ 286, (_, _) => new ItemGoldenAxe() },
		{ 287, (_, _) => new ItemString() },
		{ 288, (_, _) => new ItemFeather() },
		{ 289, (_, _) => new ItemGunpowder() },
		{ 290, (_, _) => new ItemWoodenHoe() },
		{ 291, (_, _) => new ItemStoneHoe() },
		{ 292, (_, _) => new ItemIronHoe() },
		{ 293, (_, _) => new ItemDiamondHoe() },
		{ 294, (_, _) => new ItemGoldenHoe() },
		{ 295, (_, _) => new ItemWheatSeeds() },
		{ 296, (_, _) => new ItemWheat() },
		{ 297, (_, _) => new ItemBread() },
		{ 298, (_, _) => new ItemLeatherHelmet() },
		{ 299, (_, _) => new ItemLeatherChestplate() },
		{ 300, (_, _) => new ItemLeatherLeggings() },
		{ 301, (_, _) => new ItemLeatherBoots() },
		{ 302, (_, _) => new ItemChainmailHelmet() },
		{ 303, (_, _) => new ItemChainmailChestplate() },
		{ 304, (_, _) => new ItemChainmailLeggings() },
		{ 305, (_, _) => new ItemChainmailBoots() },
		{ 306, (_, _) => new ItemIronHelmet() },
		{ 307, (_, _) => new ItemIronChestplate() },
		{ 308, (_, _) => new ItemIronLeggings() },
		{ 309, (_, _) => new ItemIronBoots() },
		{ 310, (_, _) => new ItemDiamondHelmet() },
		{ 311, (_, _) => new ItemDiamondChestplate() },
		{ 312, (_, _) => new ItemDiamondLeggings() },
		{ 313, (_, _) => new ItemDiamondBoots() },
		{ 314, (_, _) => new ItemGoldenHelmet() },
		{ 315, (_, _) => new ItemGoldenChestplate() },
		{ 316, (_, _) => new ItemGoldenLeggings() },
		{ 317, (_, _) => new ItemGoldenBoots() },
		{ 318, (_, _) => new ItemFlint() },
		{ 319, (_, _) => new ItemPorkchop() },
		{ 320, (_, _) => new ItemCookedPorkchop() },
		{ 321, (_, _) => new ItemPainting() },
		{ 322, (_, _) => new ItemGoldenApple() },
		{ 323, (_, _) => new ItemSign() },
		{ 324, (_, _) => new ItemWoodenDoor() },
		{ 325, (metadata, _) => new ItemBucket(metadata) },
		{ 328, (_, _) => new ItemMinecart() },
		{ 329, (_, _) => new ItemSaddle() },
		{ 330, (_, _) => new ItemIronDoor() },
		{ 331, (_, _) => new ItemRedstone() },
		{ 332, (_, _) => new ItemSnowball() },
		{ 333, (metadata, _) => new ItemBoat(metadata) },
		{ 334, (_, _) => new ItemLeather() },
		{ 335, (_, _) => new ItemKelp() },
		{ 336, (_, _) => new ItemBrick() },
		{ 337, (_, _) => new ItemClayBall() },
		{ 338, (_, _) => new ItemReeds() },
		{ 339, (_, _) => new ItemPaper() },
		{ 340, (_, _) => new ItemBook() },
		{ 341, (_, _) => new ItemSlimeBall() },
		{ 342, (_, _) => new ItemChestMinecart() },
		{ 344, (_, _) => new ItemEgg() },
		{ 345, (_, _) => new ItemCompass() },
		{ 346, (_, _) => new ItemFishingRod() },
		{ 347, (_, _) => new ItemClock() },
		{ 348, (_, _) => new ItemGlowstoneDust() },
		{ 349, (_, _) => new ItemCod() },
		{ 350, (_, _) => new ItemCookedCod() },
		{ 351, (_, _) => new ItemDye() },
		{ 352, (_, _) => new ItemBone() },
		{ 353, (_, _) => new ItemSugar() },
		{ 354, (_, _) => new ItemCake() },
		{ 355, (_, _) => new ItemBed() },
		{ 356, (_, _) => new ItemRepeater() },
		{ 357, (_, _) => new ItemCookie() },
		{ 358, (_, _) => new ItemMap() },
		{ 359, (_, _) => new ItemShears() },
		{ 360, (_, _) => new ItemMelon() },
		{ 361, (_, _) => new ItemPumpkinSeeds() },
		{ 362, (_, _) => new ItemMelonSeeds() },
		{ 363, (_, _) => new ItemBeef() },
		{ 364, (_, _) => new ItemCookedBeef() },
		{ 365, (_, _) => new ItemChicken() },
		{ 366, (_, _) => new ItemCookedChicken() },
		{ 367, (_, _) => new ItemRottenFlesh() },
		{ 368, (_, _) => new ItemEnderPearl() },
		{ 369, (_, _) => new ItemBlazeRod() },
		{ 370, (_, _) => new ItemGhastTear() },
		{ 371, (_, _) => new ItemGoldNugget() },
		{ 372, (_, _) => new ItemNetherWart() },
		{ 373, (metadata, _) => new ItemPotion(metadata) },
		{ 374, (_, _) => new ItemGlassBottle() },
		{ 375, (_, _) => new ItemSpiderEye() },
		{ 376, (_, _) => new ItemFermentedSpiderEye() },
		{ 377, (_, _) => new ItemBlazePowder() },
		{ 378, (_, _) => new ItemMagmaCream() },
		{ 379, (_, _) => new ItemBrewingStand() },
		{ 380, (_, _) => new ItemCauldron() },
		{ 381, (_, _) => new ItemEnderEye() },
		{ 382, (_, _) => new ItemGlisteningMelonSlice() },
		{ 383, (metadata, _) => new ItemSpawnEgg(metadata) },
		{ 389, (_, _) => new ItemFrame() },
		{ 384, (_, _) => new ItemExperienceBottle() },
		{ 385, (_, _) => new ItemFireCharge() },
		{ 386, (_, _) => new ItemWritableBook() },
		{ 387, (_, _) => new ItemWrittenBook() },
		{ 388, (_, _) => new ItemEmerald() },
		{ 390, (_, _) => new ItemFlowerPot() },
		{ 391, (_, _) => new ItemCarrot() },
		{ 392, (_, _) => new ItemPotato() },
		{ 393, (_, _) => new ItemBakedPotato() },
		{ 394, (_, _) => new ItemPoisonousPotato() },
		{ 395, (_, _) => new ItemEmptyMap() },
		{ 396, (_, _) => new ItemGoldenCarrot() },
		{ 397, (metadata, _) => new ItemSkull(metadata) },
		{ 399, (_, _) => new ItemNetherstar() },
		{ 400, (_, _) => new ItemPumpkinPie() },
		{ 401, (_, _) => new ItemFireworkRocket() },
		{ 402, (_, _) => new ItemFireworkStar() },
		{ 403, (_, _) => new ItemEnchantedBook() },
		{ 404, (_, _) => new ItemComparator() },
		{ 405, (_, _) => new ItemNetherbrick() },
		{ 406, (_, _) => new ItemQuartz() },
		{ 407, (_, _) => new ItemTntMinecart() },
		{ 408, (_, _) => new ItemHopperMinecart() },
		{ 409, (_, _) => new ItemPrismarineShard() },
		{ 410, (_, _) => new ItemHopper() },
		{ 411, (_, _) => new ItemRabbit() },
		{ 412, (_, _) => new ItemCookedRabbit() },
		{ 413, (_, _) => new ItemRabbitStew() },
		{ 414, (_, _) => new ItemRabbitFoot() },
		{ 415, (_, _) => new ItemRabbitHide() },
		{ 416, (_, _) => new ItemLeatherHorseArmor() },
		{ 417, (_, _) => new ItemIronHorseArmor() },
		{ 418, (_, _) => new ItemGoldenHorseArmor() },
		{ 419, (_, _) => new ItemDiamondHorseArmor() },
		{ 420, (_, _) => new ItemLead() },
		{ 421, (_, _) => new ItemNameTag() },
		{ 422, (_, _) => new ItemPrismarineCrystals() },
		{ 423, (_, _) => new ItemMutton() },
		{ 424, (_, _) => new ItemCookedMutton() },
		{ 425, (_, _) => new ItemArmorStand() },
		{ 426, (_, _) => new ItemEndCrystal() },
		{ 427, (_, _) => new ItemSpruceDoor() },
		{ 428, (_, _) => new ItemBirchDoor() },
		{ 429, (_, _) => new ItemJungleDoor() },
		{ 430, (_, _) => new ItemAcaciaDoor() },
		{ 431, (_, _) => new ItemDarkOakDoor() },
		{ 432, (_, _) => new ItemChorusFruit() },
		{ 433, (_, _) => new ItemPoppedChorusFruit() },
		{ 434, (_, _) => new ItemBannerPattern() },
		{ 435, (_, _) => new ItemChickenSpawnEgg() },
		{ 437, (_, _) => new ItemDragonBreath() },
		{ 438, (_, _) => new ItemSplashPotion() },
		{ 441, (_, _) => new ItemLingeringPotion() },
		{ 442, (_, _) => new ItemSparkler() },
		{ 443, (_, _) => new ItemCommandBlockMinecart() },
		{ 444, (_, _) => new ItemElytra() },
		{ 445, (_, _) => new ItemShulkerShell() },
		{ 446, (_, _) => new ItemBanner() },
		{ 447, (_, _) => new ItemMedicine() },
		{ 448, (_, _) => new ItemBalloon() },
		{ 449, (_, _) => new ItemRapidFertilizer() },
		{ 450, (_, _) => new ItemTotemOfUndying() },
		{ 451, (_, _) => new ItemBleach() },
		{ 452, (_, _) => new ItemIronNugget() },
		{ 453, (_, _) => new ItemIceBomb() },
		{ 455, (_, _) => new ItemTrident() },
		{ 457, (_, _) => new ItemBeetroot() },
		{ 458, (_, _) => new ItemBeetrootSeeds() },
		{ 459, (_, _) => new ItemBeetrootSoup() },
		{ 460, (_, _) => new ItemSalmon() },
		{ 461, (_, _) => new ItemTropicalFish() },
		{ 462, (_, _) => new ItemPufferFish() },
		{ 463, (_, _) => new ItemCookedSalmon() },
		{ 464, (_, _) => new ItemDriedKelp() },
		{ 465, (_, _) => new ItemNautilusShell() },
		{ 466, (_, _) => new ItemEnchantedApple() },
		{ 467, (_, _) => new ItemHeartOfTheSea() },
		{ 468, (_, _) => new ItemTurtleShellPiece() },
		{ 469, (_, _) => new ItemTurtleHelmet() },
		{ 470, (_, _) => new ItemPhantomMembrane() },
		{ 471, (_, _) => new ItemCrossbow() },
		{ 472, (_, _) => new ItemSpruceSign() },
		{ 473, (_, _) => new ItemBirchSign() },
		{ 474, (_, _) => new ItemJungleSign() },
		{ 475, (_, _) => new ItemAcaciaSign() },
		{ 476, (_, _) => new ItemDarkoakSign() },
		{ 477, (_, _) => new ItemSweetBerries() },
		{ 498, (metadata, _) => new ItemCamera(metadata) },
		{ 499, (_, _) => new ItemCompound() },
		{ 500, (_, _) => new ItemMusicDisc13() },
		{ 501, (_, _) => new ItemMusicDiscCat() },
		{ 502, (_, _) => new ItemMusicDiscBlocks() },
		{ 503, (_, _) => new ItemMusicDiscChirp() },
		{ 504, (_, _) => new ItemMusicDiscFar() },
		{ 505, (_, _) => new ItemMusicDiscMall() },
		{ 506, (_, _) => new ItemMusicDiscMellohi() },
		{ 507, (_, _) => new ItemMusicDiscStal() },
		{ 508, (_, _) => new ItemMusicDiscStrad() },
		{ 509, (_, _) => new ItemMusicDiscWard() },
		{ 510, (_, _) => new ItemMusicDisc11() },
		{ 511, (_, _) => new ItemMusicDiscWait() },
		{ 513, (_, _) => new ItemShield() },
		{ 720, (_, _) => new ItemCampfire() },
		{ 734, (_, _) => new ItemSuspiciousStew() },
		{ 736, (_, _) => new ItemHoneycomb() },
		{ 737, (_, _) => new ItemHoneyBottle() },
		{ 741, (_, _) => new ItemLodestoneCompass() },
		{ 742, (_, _) => new ItemNetheriteIngot() },
		{ 743, (_, _) => new ItemNetheriteSword() },
		{ 744, (_, _) => new ItemNetheriteShovel() },
		{ 745, (_, _) => new ItemNetheritePickaxe() },
		{ 746, (_, _) => new ItemNetheriteAxe() },
		{ 747, (_, _) => new ItemNetheriteHoe() },
		{ 748, (_, _) => new ItemNetheriteHelmet() },
		{ 749, (_, _) => new ItemNetheriteChestplate() },
		{ 750, (_, _) => new ItemNetheriteLeggings() },
		{ 751, (_, _) => new ItemNetheriteBoots() },
		{ 752, (_, _) => new ItemNetheriteScrap() },
		{ 758, (_, _) => new ItemChain() },
		{ 759, (_, _) => new ItemMusicDiscPigstep() },
		{ 760, (_, _) => new ItemNetherSprouts() },
		{ 801, (_, _) => new ItemSoulCampfire() },
		{ 398, (_, _) => new ItemCarrotOnAStick() },
		{ 527, (_, _) => new ItemHopper() },
		{ 544, (_, _) => new ItemMusicDisc11() },
		{ 567, (_, _) => new ItemBanner() },
		{ 572, (_, _) => new ItemScute() },
		{ 580, (_, _) => new ItemDarkOakSign() },
		{ 581, (_, _) => new ItemFlowerBannerPattern() },
		{ 582, (_, _) => new ItemCreeperBannerPattern() },
		{ 583, (_, _) => new ItemSkullBannerPattern() },
		{ 584, (_, _) => new ItemMojangBannerPattern() },
		{ 585, (_, _) => new ItemFieldMasonedBannerPattern() },
		{ 586, (_, _) => new ItemBordureIndentedBannerPattern() },
		{ 587, (_, _) => new ItemPiglinBannerPattern() },
		{ 621, (_, _) => new ItemGlowFrame() },
		{ 757, (_, _) => new ItemWarpedFungusOnAStick() },
		{ 761, (_, _) => new ItemGoatHorn() },
		{ 623, (_, _) => new ItemAmethystShard() },
		{ 624, (_, _) => new ItemSpyglass() },
		{ 630, (_, _) => new ItemGlowBerries() },
		{ 778, (_, _) => new ItemRecoveryCompass() },
		{ 779, (_, _) => new ItemEchoShard() },
		{ 780, (_, _) => new ItemBundle() },
		{ 781, (_, _) => new ItemWhiteBundle() },
		{ 782, (_, _) => new ItemOrangeBundle() },
		{ 783, (_, _) => new ItemMagentaBundle() },
		{ 784, (_, _) => new ItemLightBlueBundle() },
		{ 785, (_, _) => new ItemYellowBundle() },
		{ 786, (_, _) => new ItemLimeBundle() },
		{ 787, (_, _) => new ItemPinkBundle() },
		{ 788, (_, _) => new ItemGrayBundle() },
		{ 789, (_, _) => new ItemLightGrayBundle() },
		{ 790, (_, _) => new ItemCyanBundle() },
		{ 791, (_, _) => new ItemPurpleBundle() },
		{ 792, (_, _) => new ItemBlueBundle() },
		{ 793, (_, _) => new ItemBrownBundle() },
		{ 794, (_, _) => new ItemGreenBundle() },
		{ 795, (_, _) => new ItemRedBundle() },
		{ 796, (_, _) => new ItemBlackBundle() },
		{ 1046, (_, _) => new ItemWindCharge() },
		{ 1047, (_, _) => new ItemMace() },
		{ 1048, (_, _) => new ItemOminousBottle() },
		{ 1049, (_, _) => new ItemOminousTrialKey() },
		{ 1050, (_, _) => new ItemWolfArmor() },
		{ 1051, (_, _) => new ItemBrush() },
		{ 1052, (_, _) => new ItemChiseledBookshelf() },
		{ 1053, (_, _) => new ItemMusicDiscCreator() },
		{ 1054, (_, _) => new ItemMusicDiscCreatorMusicBox() },
		{ 1055, (_, _) => new ItemMusicDiscPrecipice() },
		{ 1056, (_, _) => new ItemCherrySign() },
		{ 1057, (_, _) => new ItemPaleOakSign() },
		{ 1058, (_, _) => new ItemBambooSign() },
		{ 1059, (_, _) => new ItemSkeletonHead() },
		{ 1060, (_, _) => new ItemPiglinHead() },
		{ 1061, (_, _) =>  new ItemCharcoal() },
		{ 1062, (_, _) => new ItemCopperIngot() },
		{ 1063, (_, _) => new ItemNetherQuartz() },
		{ 1064, (_, _) =>  new ItemResinBrick() },
		{ 1065, (_, _) => new ItemTurtleScute() },
		{ 1066, (_, _) => new ItemArmadilloScute() },
		{ 1067, (_, _) =>  new ItemBreezeRod() },
		{ 1068, (_, _) => new ItemHeavyCore() },
		{ 1069, (_, _) => new ItemFlowBannerPattern() },
		{ 1070, (_, _) => new ItemGusterBannerPattern() },
		{ 1071, (_, _) => new ItemAnglerPotterySherd() },
		{ 1072, (_, _) => new ItemArcherPotterySherd() },
		{ 1073, (_, _) => new ItemArmsUpPotterySherd() },
		{ 1074, (_, _) => new ItemBladePotterySherd() },
		{ 1075, (_, _) => new ItemBrewerPotterySherd() },
		{ 1076, (_, _) => new ItemBurnPotterySherd() },
		{ 1077, (_, _) => new ItemDangerPotterySherd() },
		{ 1078, (_, _) => new ItemExplorerPotterySherd() },
		{ 1079, (_, _) => new ItemFlowPotterySherd() },
		{ 1080, (_, _) => new ItemFriendPotterySherd() },
		{ 1081, (_, _) => new ItemGusterPotterySherd() },
		{ 1082, (_, _) => new ItemHeartPotterySherd() },
		{ 1083, (_, _) => new ItemHeartbreakPotterySherd() },
		{ 1084, (_, _) => new ItemHowlPotterySherd() },
		{ 1085, (_, _) => new ItemMinerPotterySherd() },
		{ 1086, (_, _) => new ItemMournerPotterySherd() },
		{ 1087, (_, _) => new ItemPlentyPotterySherd() },
		{ 1088, (_, _) => new ItemPrizePotterySherd() },
		{ 1089, (_, _) => new ItemScrapePotterySherd() },
		{ 1090, (_, _) => new ItemSheafPotterySherd() },
		{ 1091, (_, _) => new ItemShelterPotterySherd() },
		{ 1092, (_, _) => new ItemSkullPotterySherd() },
		{ 1093, (_, _) => new ItemSnortPotteryShert() },
		{ 1094, (_, _) => new ItemNetheriteUpgrade() },
		{ 1095, (_, _) => new ItemSentryArmorTrim() },
		{ 1096, (_, _) => new ItemVexArmorTrim() },
		{ 1097, (_, _) => new ItemWildArmorTrim() },
		{ 1098, (_, _) => new ItemCoastArmorTrim() },
		{ 1099, (_, _) => new ItemDuneArmorDrim() },
		{ 1100, (_, _) => new ItemWayfinderArmorTrim() },
		{ 1101, (_, _) => new ItemShaperArmorTrim() },
		{ 1102, (_, _) => new ItemRaiserArmorTrim() },
		{ 1103, (_, _) => new ItemHostArmorTrim() },
		{ 1104, (_, _) => new ItemWardArmorTrim() },
		{ 1105, (_, _) => new ItemSilenceArmorTrim() },
		{ 1106, (_, _) => new ItemTideArmorTrim() },
		{ 1107, (_, _) => new ItemSnoutArmorTrim() },
		{ 1108, (_, _) => new ItemRibArmorTrim() },
		{ 1109, (_, _) => new ItemEyeArmorTrim() },
		{ 1110, (_, _) => new ItemSpireArmorTrim() },
		{ 1111, (_, _) => new ItemFlowArmorTrim() },
		{ 1112, (_, _) => new ItemBoltArmorTrim() },
		{ 1113, (_, _) => new ItemCupLove() },
		{ 1114, (_, _) => new ItemHiveEnderWings() }
	};

	public static Item GetItem(short id, short metadata = 0, int count = 1)
	{
		try
		{
			Item item = null;

			if (CustomItemFactory != null)
				item = CustomItemFactory.GetItem(id, metadata, count);

			item = item switch
			{
				null when ItemFactories.TryGetValue(id, out Func<short, int, Item> factory) => factory(metadata, count),
				null when id <= 255 => CreateBlockItem(id, metadata, count),
				_ => item
			};

			if (item == null)
			{
				return null;
			}

			item.Metadata = metadata;
			item.Count = (byte)count;

			if (string.IsNullOrWhiteSpace(item.Name)) return item;
			Itemstate result = Itemstates?.FirstOrDefault(x => x.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase));
			if (result != null)
				item.NetworkId = result.Id;

			return item;
		}
		catch (Exception ex)
		{
			Log.Error($"Error while getting item for ID {id} with metadata {metadata}: {ex.Message}", ex);
			return null;
		}
	}

	private static ItemBlock CreateBlockItem(short id, short metadata, int count)
	{
		int blockId = id < 0 ? (short)(Math.Abs(id) + 255) : id;

		Block block = BlockFactory.GetBlockById(blockId);
		if (block == null)
		{
			Log.Warn($"Block ID {blockId} is invalid. Returning null.");
			return null;
		}

		uint runtimeId = BlockFactory.GetRuntimeId(blockId, (byte)metadata);
		if (!BlockFactory.BlockPalette.TryGetValue((int)runtimeId, out BlockStateContainer blockState))
		{
			return CustomBlockItemFactory != null
				? CustomBlockItemFactory.GetBlockItem(block, metadata, count)
				: new ItemBlock(block, metadata);
		}

		block.SetState(blockState);

		return CustomBlockItemFactory != null
			? CustomBlockItemFactory.GetBlockItem(block, metadata, count)
			: new ItemBlock(block, metadata);
	}
}

public class ItemMusicDiscWard() : Item("minecraft:music_disc_ward", 509);

public class ItemSparkler() : Item("minecraft:sparkler", 442);

public class ItemNautilusShell() : Item("minecraft:nautilus_shell", 465);

public class ItemComparator() : Item("minecraft:comparator", 404);

public class ItemRabbitFoot() : Item("minecraft:rabbit_foot", 414);

public class ItemLingeringPotion(short metadata = 0) : Item("minecraft:lingering_potion", 441, metadata: metadata);

public class ItemCampfire() : Item("minecraft:campfire", 720);

public class ItemMusicDiscFar() : Item("minecraft:music_disc_far", 504);

public class ItemPumpkinSeeds() : Item("minecraft:pumpkin_seeds", 361);

public class ItemCommandBlockMinecart() : Item("minecraft:command_block_minecart", 443);

public class ItemMelonSeeds() : Item("minecraft:melon_seeds", 362);

public class ItemNetherWart() : Item("minecraft:nether_wart", 372);

public class ItemMusicDiscStrad() : Item("minecraft:music_disc_strad", 508);

public class ItemBowl() : Item("minecraft:bowl", 281);

public class ItemString() : Item("minecraft:string", 287);

public class ItemFeather() : Item("minecraft:feather", 288);

public class ItemGunpowder() : Item("minecraft:gunpowder", 289);

public class ItemMusicDiscMellohi() : Item("minecraft:music_disc_mellohi", 506);

public class ItemEnderEye() : Item("minecraft:ender_eye", 381);

public class ItemShield() : Item("minecraft:shield", 513);

public class ItemFlint() : Item("minecraft:flint", 318);

public class ItemHeartOfTheSea() : Item("minecraft:heart_of_the_sea", 467);

public class ItemMinecart() : Item("minecraft:minecart", 328);

public class ItemWrittenBook() : Item("minecraft:written_book", 387);

public class ItemLeather() : Item("minecraft:leather", 334);

public class ItemBrick() : Item("minecraft:brick", 336);

public class ItemCarrotOnAStick() : Item("minecraft:carrot_on_a_stick", 398);

public class ItemReeds() : Item("minecraft:item.reeds", 338);

public class ItemPaper() : Item("minecraft:paper", 339);

public class ItemTrident() : Item("minecraft:trident", 455);

public class ItemSlimeBall() : Item("minecraft:slime_ball", 341);

public class ItemChestMinecart() : Item("minecraft:chest_minecart", 342);

public class ItemFishingRod() : Item("minecraft:fishing_rod", 346);

public class ItemClock() : Item("minecraft:clock", 347);
	
public class ItemGlowstoneDust() : Item("minecraft:glowstone_dust", 348);

public class ItemNameTag() : Item("minecraft:name_tag", 421);

public class ItemCake() : Item("minecraft:cake", 354);

public class ItemRepeater() : Item("minecraft:repeater", 356);

public class ItemGhastTear() : Item("minecraft:ghast_tear", 370);

public class ItemGlassBottle() : Item("minecraft:glass_bottle", 374);

public class ItemFermentedSpiderEye() : Item("minecraft:fermented_spider_eye", 376);

public class ItemMagmaCream() : Item("minecraft:magma_cream", 378);

public class ItemBrewingStand() : Item("minecraft:brewing_stand", 379);

public class ItemRapidFertilizer() : Item("minecraft:rapid_fertilizer", 449); // what is this?

public class ItemGlisteningMelonSlice() : Item("minecraft:glistering_melon_slice", 382);

public class ItemFireCharge() : Item("minecraft:fire_charge", 385);

public class ItemWritableBook() : Item("minecraft:writable_book", 386);

public class ItemEmerald() : Item("minecraft:emerald", 388);

public class ItemMusicDiscPigstep() : Item("minecraft:music_disc_pigstep", 759);

public class ItemFlowerPot() : Item("minecraft:flower_pot", 390);

public class ItemNetherstar() : Item("minecraft:nether_star", 399);

public class ItemHopperMinecart() : Item("minecraft:hopper_minecart", 408);

public class ItemFireworkStar() : Item("minecraft:firework_star", 402);

public class ItemNetherbrick() : Item("minecraft:netherbrick", 405);

public class ItemQuartz() : Item("minecraft:quartz", 406);

public class ItemTntMinecart() : Item("minecraft:tnt_minecart", 407);

public class ItemHopper() : Item("minecraft:hopper", 410);

public class ItemDragonBreath() : Item("minecraft:dragon_breath", 437);

public class ItemRabbitHide() : Item("minecraft:rabbit_hide", 415);

public class ItemMusicDisc13() : Item("minecraft:music_disc_13", 500);

public class ItemMusicDiscCat() : Item("minecraft:music_disc_cat", 501);

public class ItemMusicDiscBlocks() : Item("minecraft:music_disc_blocks", 502);

public class ItemMusicDiscChirp() : Item("minecraft:music_disc_chirp", 503);

public class ItemMusicDiscMall() : Item("minecraft:music_disc_mall", 505);

public class ItemMusicDiscStal() : Item("minecraft:music_disc_stal", 507);

public class ItemMusicDisc11() : Item("minecraft:music_disc_11", 510);

public class ItemMusicDiscWait() : Item("minecraft:music_disc_wait", 511);

public class ItemLead() : Item("minecraft:lead", 420);

public class ItemPrismarineCrystals() : Item("minecraft:prismarine_crystals", 422);

public class ItemArmorStand() : Item("minecraft:armor_stand", 425);

public class ItemPhantomMembrane() : Item("minecraft:phantom_membrane", 470);

public class ItemSuspiciousStew() : Item("minecraft:suspicious_stew", 734);

public class ItemPoppedChorusFruit() : Item("minecraft:popped_chorus_fruit", 433);

public class ItemPrismarineShard() : Item("minecraft:prismarine_shard", 409);

public class ItemShulkerShell() : Item("minecraft:shulker_shell", 445);

public class ItemTotemOfUndying() : Item("minecraft:totem_of_undying", 450);

public class ItemTurtleShellPiece() : Item("minecraft:scute", 468);

public class ItemBalloon() : Item("minecraft:balloon", 448);

public class ItemBannerPattern() : Item("minecraft:banner_pattern", 434);

public class ItemHoneycomb() : Item("minecraft:honeycomb", 736);

public class ItemCompound() : Item("minecraft:compound", 499);

public class ItemIceBomb() : Item("minecraft:ice_bomb", 453);

public class ItemBleach() : Item("minecraft:bleach", 451);

public class ItemMedicine() : Item("minecraft:medicine", 447);

public class ItemLodestoneCompass() : Item("minecraft:lodestone_compass", 741);

public class ItemNetheriteIngot() : Item("minecraft:netherite_ingot", 742);

public class ItemNetheriteScrap() : Item("minecraft:netherite_scrap", 752);

public class ItemChain() : Item("minecraft:chain", 758);

public class ItemNetherSprouts() : Item("minecraft:nether_sprouts", 760);

public class ItemSoulCampfire() : Item("minecraft:soul_campfire", 801);

public class ItemEndCrystal() : Item("minecraft:end_crystal", 426);

public class ItemMace() : ItemSword("minecraft:mace", 1047, false);

public class ItemSpyglass() : Item("minecraft:spyglass", 624);

public class ItemGlowFrame() : Item("minecraft:glow_frame", 621);

public class ItemChickenSpawnEgg() : Item("minecraft:chicken_spawn_egg", 435);

public class ItemPiglinBannerPattern() : Item("minecraft:piglin_banner_pattern", 587);

public class ItemMojangBannerPattern() : Item("minecraft:mojang_banner_pattern", 584);

public class ItemSkullBannerPattern() : Item("minecraft:skull_banner_pattern", 583);

public class ItemDarkOakSign() : Item("minecraft:dark_oak_sign", 580);

public class ItemBordureIndentedBannerPattern() : Item("minecraft:bordure_intented_banner_pattern", 586);

public class ItemScute() : Item("minecraft:scute", 572);

public class ItemFlowerBannerPattern() : Item("minecraft:flower_banner_pattern", 581);

public class ItemCreeperBannerPattern() : Item("minecraft:creeper_banner_pattern", 582);

public class ItemFieldMasonedBannerPattern() : Item("minecraft:field_masoned_banner_pattern", 585);

public class ItemAmethystShard() : Item("minecaft:amethyst_shard", 623);

public class ItemWarpedFungusOnAStick() : Item("minecraft:warped_fungus_on_a_stick", 757);

public class ItemRecoveryCompass() : Item("minecraft:recovery_compass", 778);

public class ItemEchoShard() : Item("minecraft:echo_shard", 779);

public class ItemOminousBottle(short metadata = 0) : Item("minecraft:ominous_bottle", 1048, metadata: metadata);

public class ItemOminousTrialKey() : Item("minecraft:ominous_trial_key", 1049);

public class ItemWolfArmor() : Item("minecraft:wolf_armor", 1050);

public class ItemBrush() : Item("minecraft:brush", 1051);

public class ItemBoneMeal() : Item("minecraft:bone_meal", 15);

public class ItemFlowBannerPattern() : Item("minecraft:flow_banner_pattern", 1069);

public class ItemGusterBannerPattern() : Item("minecraft:guster_banner_pattern", 1070);