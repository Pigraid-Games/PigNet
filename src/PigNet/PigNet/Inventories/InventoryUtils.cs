using System;
using System.Collections.Generic;
using System.IO;
using fNbt;
using PigNet.Blocks;
using PigNet.Items;
using PigNet.Net;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;
using PigNet.Utils.Nbt;

namespace PigNet.Inventories;

public static class InventoryUtils
	{
		private static readonly CreativeInventoryCategoryType[] DefaultCategories =
		[
			CreativeInventoryCategoryType.Construction,
			CreativeInventoryCategoryType.Nature,
			CreativeInventoryCategoryType.Equipment,
			CreativeInventoryCategoryType.Items,
		];

		public static CreativeInventoryContent Content { get; } = new CreativeInventoryContent();

		private static McpeCreativeContent _creativeContentData;
		private static McpeItemComponent _itemRegistryData;
		private static readonly bool _isEduEnabled;

		static InventoryUtils()
		{
			_isEduEnabled = Config.GetProperty("EnableEdu", false);

			foreach (var category in DefaultCategories)
			{
				var data = ResourceUtil.ReadResource<ExternalDataCategory>($"{category.ToString().ToLower()}.json", typeof(InventoryUtils), "Data");
				Content.AppendCategory(category, data);
			}
		}

		public static McpeCreativeContent GetCreativeInventoryData()
		{
			if (_creativeContentData != null) return _creativeContentData;
			McpeCreativeContent creativeContent = McpeCreativeContent.CreateObject();
			creativeContent.content = Content;
			creativeContent.MarkPermanent();
			_creativeContentData = creativeContent;

			return _creativeContentData;
		}

		public static McpeItemComponent GetItemRegistryData()
		{
			if (_itemRegistryData != null) return _itemRegistryData;
			McpeItemComponent creativeContent = McpeItemComponent.CreateObject();
			creativeContent.entries = ItemFactory.ItemStates;
			creativeContent.MarkPermanent();
			_itemRegistryData = creativeContent;

			return _itemRegistryData;
		}

		public static bool TryGetItemFromExternalData(ExternalDataItem itemData, out Item result)
		{
			result = null;

			if (string.IsNullOrEmpty(itemData.Id)) return false;

			var item = ItemFactory.GetItem(itemData.Id, itemData.Metadata, (byte) itemData.Count);
			if (item is ItemAir) return false;
			if (item.Edu && !_isEduEnabled) return false;

			if (itemData.BlockStates != null && item is ItemBlock itemBlock)
			{
				var compound = NbtExtensions.ReadNbtCompound(itemData.BlockStates, NbtFlavor.BedrockNoVarInt);

				itemBlock.Block.SetStates(BlockFactory.GetBlockStates(compound));
			}

			if (itemData.ExtraData != null)
			{
				item.ExtraData = NbtExtensions.ReadNbtCompound(itemData.ExtraData, NbtFlavor.BedrockNoVarInt);
			}

			item.Metadata = itemData.Metadata;

			result = item;
			return true;
		}
	}