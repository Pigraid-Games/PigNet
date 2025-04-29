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
using System.Collections.Generic;
using System.IO;
using fNbt;
using PigNet.Blocks;
using PigNet.Items;
using PigNet.Net;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;

namespace PigNet.Inventories;

public static class InventoryUtils
{
	private static McpeCreativeContent _creativeInventoryData;
	private static readonly bool _isEduEnabled;

	static InventoryUtils()
	{
		_isEduEnabled = Config.GetProperty("EnableEdu", false);

		var creativeItems = ResourceUtil.ReadResource<List<ExternalDataItem>>("creativeitems.json", typeof(InventoryUtils), "Data");

		CreativeInventoryItems.Add(new ItemAir());

		int uniqueId = 1;
		foreach (var itemData in creativeItems)
			if (TryGetItemFromExternalData(itemData, out Item item))
			{
				item.UniqueId = uniqueId++;
				CreativeInventoryItems.Add(item);
			}
	}

	public static List<Item> CreativeInventoryItems { get; } = [];

	public static McpeCreativeContent GetCreativeInventoryData()
	{
		if (_creativeInventoryData == null)
		{
			McpeCreativeContent creativeContent = McpeCreativeContent.CreateObject();
			creativeContent.input = GetCreativeMetadataSlots();
			creativeContent.MarkPermanent();
			_creativeInventoryData = creativeContent;
		}

		return _creativeInventoryData;
	}

	public static CreativeItemStacks GetCreativeMetadataSlots()
	{
		return new CreativeItemStacks(CreativeInventoryItems.ToArray());
	}

	public static bool TryGetItemFromExternalData(ExternalDataItem itemData, out Item result)
	{
		result = null;

		if (string.IsNullOrEmpty(itemData.Id)) return false;

		Item item = ItemFactory.GetItem(itemData.Id, itemData.Metadata, (byte) itemData.Count);
		if (item is ItemAir) return false;
		if (item.Edu && !_isEduEnabled) return false;

		if (itemData.BlockStates != null && item is ItemBlock itemBlock)
		{
			byte[] bytes = Convert.FromBase64String(itemData.BlockStates);

			using var memoryStream = new MemoryStream(bytes, 0, bytes.Length);
			NbtCompound compound = Packet.ReadNbtCompound(memoryStream);

			itemBlock.Block.SetStates(BlockFactory.GetBlockStates(compound));
		}

		if (itemData.ExtraData != null)
		{
			byte[] bytes = Convert.FromBase64String(itemData.ExtraData);

			using var memoryStream = new MemoryStream(bytes, 0, bytes.Length);
			item.ExtraData = Packet.ReadNbtCompound(memoryStream);
		}

		item.Metadata = itemData.Metadata;

		result = item;
		return true;
	}
}