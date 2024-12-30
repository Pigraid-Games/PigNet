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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2023 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Net.Items
{
	public class ItemTranslator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTranslator));
		
		private readonly Dictionary<int, TranslatedItem> _networkIdToInternal;
		private readonly Dictionary<int, int> _simpleNetworkIdToInternal;

		private readonly Dictionary<int, ComplexMappingEntry> _internalIdToNetwork;
		private readonly Dictionary<int, int> _simpleInternalIdToNetwork;

		private readonly Dictionary<string, string> _internalNameToNetworkName;
		private readonly MetaToName<string, int, string> _metaToName;
		private readonly Dictionary<string, int> _metaList;
		private readonly Dictionary<string, string> _metaMapList;

		public ItemTranslator(Itemstates itemStates)
		{
			var internalNameToNetworkName = new Dictionary<string, string>(StringComparer.Ordinal);
			Dictionary<string, short> legacyTranslations = ResourceUtil.ReadResource<Dictionary<string, short>>("item_id_map.json", typeof(Item));
			Dictionary<string, Dictionary<string, string>> metaMapRes = ResourceUtil.ReadResource<Dictionary<string, Dictionary<string, string>>>("block_meta_map.json", typeof(Item));

			var simpleMappings = new Dictionary<string, short>();
			var metaToNameList = new MetaToName<string, int, string>();
			var metaList = new Dictionary<string, int>();
			var metaMapList = new Dictionary<string, string>();

			foreach ((string stringId, short integerId) in legacyTranslations)
			{
				simpleMappings[stringId] = integerId;
				internalNameToNetworkName[stringId] = stringId;
			}

			var complexMapping = new Dictionary<string, TranslatedItem>();
			foreach ((string oldId, Dictionary<string, string> value) in metaMapRes)
			{
				if (!legacyTranslations.ContainsKey(oldId)) continue;
				metaToNameList[oldId] = new MetaToName<int, string>();
				short legacyIntegerId = legacyTranslations[oldId];
				foreach ((string key, string newId) in value)
				{
					metaToNameList[oldId].TryAdd(int.Parse(key), newId);
					metaList.TryAdd(newId, int.Parse(key));
					metaMapList.TryAdd(newId, oldId);
					if (!short.TryParse(key, out short meta)) continue;
					if (!complexMapping.TryAdd(newId, new TranslatedItem(legacyIntegerId, meta)))
					{
						Log.Debug($"Duplicate complex... OldId={oldId} NewId={newId} (IntegerID={legacyIntegerId} Meta={meta})");
					}
				}
			}
			
			var internalToNetwork = new Dictionary<int, ComplexMappingEntry>();
			var simpleInternalToNetwork = new Dictionary<int, int>();
			var networkIdToInternal = new Dictionary<int, TranslatedItem>();
			var simpleNetworkIdToInternal = new Dictionary<int, int>();
			foreach (Itemstate state in itemStates)
			{
				string stringId = state.Name;
				short netId = state.Id;

				if (complexMapping.TryGetValue(stringId, out TranslatedItem translatedItem))
				{
					int internalId = translatedItem.Id;
					short internalMeta = translatedItem.Meta;

					if (!internalToNetwork.TryGetValue(internalId, out ComplexMappingEntry mappingEntry))
					{
						mappingEntry = new ComplexMappingEntry();
						internalToNetwork.Add(internalId, mappingEntry);
					}

					mappingEntry.Add(internalMeta, netId);
					
					internalToNetwork[internalId] = mappingEntry;
					networkIdToInternal.Add(netId, translatedItem);
				}
				else if (simpleMappings.TryGetValue(stringId, out short legacyId))
				{
					simpleNetworkIdToInternal.Add(netId, legacyId);
					simpleInternalToNetwork.Add(legacyId, netId);
				}
			}

			_internalIdToNetwork = internalToNetwork;
			_simpleInternalIdToNetwork = simpleInternalToNetwork;
			_networkIdToInternal = networkIdToInternal;
			_simpleNetworkIdToInternal = simpleNetworkIdToInternal;
			_internalNameToNetworkName = internalNameToNetworkName;
			_metaToName = metaToNameList;
			_metaList = metaList;
			_metaMapList = metaMapList;
		}

		public string GetNameByMeta(string cname, int meta)
		{
			return _metaToName[cname].GetValueOrDefault(meta);
		}

		public byte GetMetaByName(string name)
		{
			if (_metaList.TryGetValue(name, out int meta))
			{
				return (byte)meta;
			}
			return 255;
		}

		public string GetMetaMapByName(string name)
		{
			return _metaMapList.GetValueOrDefault(name);
		}

		internal bool TryGetNetworkId(int id, short meta, out TranslatedItem item)
		{
			if (_internalIdToNetwork.TryGetValue(id, out ComplexMappingEntry complex) && complex.TryGet(meta, out int netId))
			{
				item = new TranslatedItem(netId, 0);
				return true;
			}
			else if (_simpleInternalIdToNetwork.TryGetValue(id, out netId))
			{
				item = new TranslatedItem(netId, meta);
				return true;
			}

			item = default;
			return false;
		}
		
		internal TranslatedItem ToNetworkId(int id, short meta)
		{
			if (_internalIdToNetwork.TryGetValue(id, out ComplexMappingEntry complex) && complex.TryGet(meta, out int netId))
			{
				id = netId;
				meta = 0;
			}
			else if (_simpleInternalIdToNetwork.TryGetValue(id, out netId))
			{
				id = netId;
			}

			return new TranslatedItem(id, meta);
		}
		
		internal TranslatedItem FromNetworkId(int id, short meta)
		{
			if (_networkIdToInternal.TryGetValue(id, out TranslatedItem value))
			{
				id = value.Id;
				meta = value.Meta;
			}
			else if (_simpleNetworkIdToInternal.TryGetValue(id, out int simpleValue))
			{
				id = simpleValue;
			}

			return new TranslatedItem(id, meta);
		}

		public bool TryGetName(string input, out string output)
		{
			return _internalNameToNetworkName.TryGetValue(input, out output);
		}
	}

	public class MetaToName<MetaId, MetaName> :
		Dictionary<MetaId, MetaName>
	{
	}

	public class MetaToName<MetaMap, MetaId, MetaName> :
		Dictionary<MetaMap, MetaToName<MetaId, MetaName>>
	{
	}

	internal class TranslatedItem(int id, short meta) : IEquatable<TranslatedItem>
	{
		public int Id { get; } = id;
		public short Meta { get; } = meta;

		/// <inheritdoc />
		public bool Equals(TranslatedItem other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return Id == other.Id && Meta == other.Meta;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((TranslatedItem)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Meta);
		}
	}

	internal class ComplexMappingEntry
	{
		private readonly Dictionary<short, int> _mapping = new();
		public void Add(short meta, short translatedItem)
		{
			_mapping.Add(meta, translatedItem);
		}

		public bool TryGet(short meta, out int result)
		{
			return _mapping.TryGetValue(meta, out result);
		}
	}
}