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

namespace MiNET.Net.Items;

public class ItemTranslator
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTranslator));
	private readonly Dictionary<int, ComplexMappingEntry> _internalIdToNetwork = new();
	private readonly Dictionary<string, string> _internalNameToNetworkName = new(StringComparer.Ordinal);
	private readonly Dictionary<string, int> _metaList = new();
	private readonly Dictionary<string, string> _metaMapList = new();
	private readonly MetaToName<string, int, string> _metaToName = new();

	private readonly Dictionary<int, TranslatedItem> _networkIdToInternal = new();
	private readonly Dictionary<int, int> _simpleInternalIdToNetwork = new();
	private readonly Dictionary<int, int> _simpleNetworkIdToInternal = new();

	public ItemTranslator(Itemstates itemStates)
	{
		Dictionary<string, short> legacyTranslations =
			ResourceUtil.ReadResource<Dictionary<string, short>>("item_id_map.json", typeof(Item));
		Dictionary<string, Dictionary<string, string>> metaMapRes =
			ResourceUtil.ReadResource<Dictionary<string, Dictionary<string, string>>>("block_meta_map.json", typeof(Item));

		foreach ((string stringId, short _) in legacyTranslations) _internalNameToNetworkName[stringId] = stringId;

		var complexMapping = new Dictionary<string, TranslatedItem>();
		foreach ((string oldId, Dictionary<string, string> value) in metaMapRes)
		{
			if (!legacyTranslations.TryGetValue(oldId, out short legacyIntegerId)) continue;

			_metaToName[oldId] = new MetaToName<int, string>();

			foreach ((string key, string newId) in value)
			{
				int meta = int.Parse(key);
				_metaToName[oldId][meta] = newId;
				_metaList.TryAdd(newId, meta);
				_metaMapList.TryAdd(newId, oldId);

				if (!complexMapping.TryAdd(newId, new TranslatedItem(legacyIntegerId, (short) meta))) Log.Debug($"Duplicate complex... OldId={oldId} NewId={newId} (IntegerID={legacyIntegerId} Meta={meta})");
			}
		}

		foreach (Itemstate state in itemStates)
		{
			string stringId = state.Name;
			short netId = state.Id;

			if (complexMapping.TryGetValue(stringId, out TranslatedItem translatedItem))
				AddComplexMapping(translatedItem, netId);
			else if (legacyTranslations.TryGetValue(stringId, out short legacyId))
			{
				_simpleNetworkIdToInternal[netId] = legacyId;
				_simpleInternalIdToNetwork[legacyId] = netId;
			}
		}
	}

	private void AddComplexMapping(TranslatedItem translatedItem, int netId)
	{
		if (!_internalIdToNetwork.TryGetValue(translatedItem.Id, out ComplexMappingEntry mappingEntry))
		{
			mappingEntry = new ComplexMappingEntry();
			_internalIdToNetwork[translatedItem.Id] = mappingEntry;
		}
		mappingEntry.Add(translatedItem.Meta, (short) netId);
		_networkIdToInternal[netId] = translatedItem;
	}

	public string GetNameByMeta(string cname, int meta)
	{
		return _metaToName[cname].GetValueOrDefault(meta);
	}

	public byte GetMetaByName(string name)
	{
		return _metaList.TryGetValue(name, out int meta) ? (byte) meta : (byte) 255;
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
		if (_simpleInternalIdToNetwork.TryGetValue(id, out netId))
		{
			item = new TranslatedItem(netId, meta);
			return true;
		}
		item = default;
		return false;
	}

	internal TranslatedItem ToNetworkId(int id, short meta)
	{
		return _internalIdToNetwork.TryGetValue(id, out ComplexMappingEntry complex) && complex.TryGet(meta, out int netId)
			? new TranslatedItem(netId, 0)
			: new TranslatedItem(_simpleInternalIdToNetwork.GetValueOrDefault(id, id), meta);
	}


	internal TranslatedItem FromNetworkId(int id, short meta)
	{
		return _networkIdToInternal.TryGetValue(id, out TranslatedItem value)
			? new TranslatedItem(value.Id, value.Meta)
			: new TranslatedItem(_simpleNetworkIdToInternal.GetValueOrDefault(id, id), meta);
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
		return obj.GetType() == GetType() && Equals((TranslatedItem) obj);
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