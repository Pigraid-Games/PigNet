using System;
using System.Collections.Generic;
using log4net;
using Newtonsoft.Json;
using PigNet.Items;
using PigNet.Utils;

namespace PigNet.Net.Items;

public class ItemTranslator
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTranslator));
	
	private readonly Dictionary<int, ComplexMappingEntry> _internalIdToNetwork = new();
	private readonly Dictionary<int, int> _simpleInternalIdToNetwork = new();
	private readonly Dictionary<int, TranslatedItem> _networkIdToInternal = new();
	private readonly Dictionary<int, int> _simpleNetworkIdToInternal = new();
	private readonly Dictionary<string, string> _internalNameToNetworkName = new(StringComparer.Ordinal);

	public ItemTranslator(Itemstates itemStates)
	{
		var legacyTranslations = ResourceUtil.ReadResource<Dictionary<string, short>>("item_id_map.json", typeof(Item), "Data");
		var r16Mapping = ResourceUtil.ReadResource<R16ToCurrentMap>("r16_to_current_item_map.json", typeof(Item), "Data");

		var simpleMappings = new Dictionary<string, short>();

		// Build simple mappings from R16 data
		foreach ((string oldId, string newId) in r16Mapping.Simple)
		{
			if (!legacyTranslations.TryGetValue(oldId, out short legacyId))
			{
				Log.Warn($"Missing legacy ID for {oldId} while building simple mappings.");
				continue;
			}
			
			if (!simpleMappings.TryAdd(newId, legacyId))
				Log.Warn($"Duplicate simple mapping detected: {newId} (from {oldId})");

			_internalNameToNetworkName[oldId] = newId;
		}

		// Add missing translations that were not updated in R16
		foreach ((string stringId, short legacyId) in legacyTranslations)
		{
			if (!simpleMappings.ContainsKey(stringId))
				simpleMappings[stringId] = legacyId;
		}

		// Build complex mappings
		var complexMapping = new Dictionary<string, TranslatedItem>();
		foreach ((string oldId, Dictionary<string, string> metaMapping) in r16Mapping.Complex)
		{
			if (!legacyTranslations.TryGetValue(oldId, out short legacyId))
			{
				Log.Warn($"Missing legacy ID for {oldId} while building complex mappings.");
				continue;
			}

			foreach ((string metaKey, string newId) in metaMapping)
			{
				if (!short.TryParse(metaKey, out var meta))
				{
					Log.Warn($"Invalid metadata {metaKey} for {oldId}");
					continue;
				}

				if (!complexMapping.TryAdd(newId, new TranslatedItem(legacyId, meta)))
					Log.Warn($"Duplicate complex mapping for {newId} (from {oldId}, meta {meta})");
			}
		}

		// Map network itemstates
		foreach (Itemstate state in itemStates)
		{
			string stringId = state.Name;
			short netId = state.Id;

			if (complexMapping.TryGetValue(stringId, out var translatedItem))
			{
				AddComplexMapping(translatedItem, netId);
			}
			else if (simpleMappings.TryGetValue(stringId, out short legacyId))
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

	internal bool TryGetNetworkId(int id, short meta, out TranslatedItem item)
	{
		if (_internalIdToNetwork.TryGetValue(id, out var complex) && complex.TryGet(meta, out int netId))
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
		return _internalIdToNetwork.TryGetValue(id, out var complex) && complex.TryGet(meta, out int netId)
			? new TranslatedItem(netId, 0)
			: new TranslatedItem(_simpleInternalIdToNetwork.GetValueOrDefault(id, id), meta);
	}

	internal TranslatedItem FromNetworkId(int id, short meta)
	{
		return _networkIdToInternal.TryGetValue(id, out var value)
			? new TranslatedItem(value.Id, value.Meta)
			: new TranslatedItem(_simpleNetworkIdToInternal.GetValueOrDefault(id, id), meta);
	}

	public bool TryGetName(string input, out string output)
	{
		return _internalNameToNetworkName.TryGetValue(input, out output);
	}
}

internal class TranslatedItem(int id, short meta) : IEquatable<TranslatedItem>
{
	public int Id { get; } = id;
	public short Meta { get; } = meta;

	public bool Equals(TranslatedItem other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Id == other.Id && Meta == other.Meta;
	}

	public override bool Equals(object obj)
	{
		return obj is TranslatedItem other && Equals(other);
	}

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

class R16ToCurrentMap
{
	[JsonProperty("complex")]
	public Dictionary<string, Dictionary<string, string>> Complex { get; set; }

	[JsonProperty("simple")]
	public Dictionary<string, string> Simple { get; set; }
}
