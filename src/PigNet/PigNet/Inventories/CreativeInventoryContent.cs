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

using System.Collections.Generic;
using System.Linq;
using PigNet.Items;
using PigNet.Net;

namespace PigNet.Inventories;

public class CreativeInventoryContent : IPacketDataObject
	{
		private int _runtimeIdCounter = 1;
		private uint _groupIdCounter;
		private readonly List<CreativeInventoryGroupItem> _items = [];
		private readonly Dictionary<CreativeInventoryCategoryType, CreativeInventoryCategory> _creativeInventoryCategories = new();

		public CreativeInventoryContent()
		{
			_items.Add(new CreativeInventoryGroupItem { Item = new ItemAir() });
		}

		public void AppendCategory(CreativeInventoryCategoryType type, ExternalDataCategory data)
		{
			var category = new CreativeInventoryCategory { Type = type };

			foreach (ExternalDataGroup dataGroup in data)
			{
				var group = new CreativeInventoryGroup
				{
					CategoryType = category.Type,
					Name = dataGroup.Name
				};

				if (dataGroup.IconItem != null)
				{
					if (!InventoryUtils.TryGetItemFromExternalData(dataGroup.IconItem, out Item iconItem)) continue;
					group.IconItem = iconItem;
				}

				foreach (ExternalDataItem dataItem in dataGroup.Items)
				{
					if (!InventoryUtils.TryGetItemFromExternalData(dataItem, out Item item)) continue;
					item.UniqueId = _runtimeIdCounter++;
					group.Items.Add(item);

					_items.Add(new CreativeInventoryGroupItem
					{
						Item = item,
						GroupIndex = _groupIdCounter
					});
				}

				if (group.Items.Count == 0) continue;
				_groupIdCounter++;
				category.Groups.Add(group);
			}

			_creativeInventoryCategories.Add(category.Type, category);
		}

		public Dictionary<CreativeInventoryCategoryType, CreativeInventoryCategory> GetCategories()
		{
			return new Dictionary<CreativeInventoryCategoryType, CreativeInventoryCategory>(_creativeInventoryCategories);
		}

		public Item GetItemById(uint creativeId)
		{
			return _items.ElementAtOrDefault((int) creativeId)?.Item;
		}

		public void Write(Packet packet)
		{
			packet.WriteLength((int) _groupIdCounter);
			foreach (CreativeInventoryCategory category in _creativeInventoryCategories.Values.ToArray())
			{
				foreach (CreativeInventoryGroup group in category.Groups.ToArray()) packet.Write(group);
			}

			packet.WriteLength(_items.Count - 1);
			foreach (CreativeInventoryGroupItem item in _items.Skip(1)) packet.Write(item);
		}

		public static CreativeInventoryContent Read(Packet packet)
		{
			var content = new CreativeInventoryContent();

			var groups = new CreativeInventoryGroup[packet.ReadLength()];
			for (int i = 0; i < groups.Length; i++)
			{
				CreativeInventoryGroup group = groups[i] = CreativeInventoryGroup.Read(packet);
				
				if (!content._creativeInventoryCategories.TryGetValue(group.CategoryType, out CreativeInventoryCategory category))
				{
					content._creativeInventoryCategories.Add(group.CategoryType, category = new CreativeInventoryCategory()
					{
						Type = group.CategoryType
					});
				}

				category.Groups.Add(group);
			}

			var items = new CreativeInventoryGroupItem[packet.ReadLength()];
			for (int i = 0; i < items.Length; i++)
			{
				var item = CreativeInventoryGroupItem.Read(packet);
				content._items.Add(item);
				groups[item.GroupIndex].Items.Add(item.Item);
			}

			return content;
		}
	}