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
using System.Linq;
using fNbt;

namespace PigNet.Worlds.Anvil.Mapping;

public class PropertyStateMapper : IPropertyStateMapper
{
	public string OldName { get; set; }
	public string NewName { get; set; }

	public Dictionary<string, PropertyValueStateMapper> ValuesMap { get; } = new Dictionary<string, PropertyValueStateMapper>();

	private readonly Func<string, NbtCompound, NbtString, NbtString> _func;

	public PropertyStateMapper(params PropertyValueStateMapper[] propertiesNameMap)
		: this(oldName: null, newName: null, propertiesNameMap)
	{

	}

	public PropertyStateMapper(Func<string, NbtCompound, NbtString, NbtString> func)
		: this(oldName: null, func)
	{

	}

	public PropertyStateMapper(string oldName, params PropertyValueStateMapper[] propertiesNameMap)
		: this(oldName, newName: null, propertiesNameMap)
	{

	}

	public PropertyStateMapper(string oldName, string newName, params PropertyValueStateMapper[] propertiesNameMap)
	{
		OldName = oldName;
		NewName = newName;

		foreach (var map in propertiesNameMap)
		{
			ValuesMap.Add(map.OldName, map);
		}
	}

	public PropertyStateMapper(string oldName, Func<string, NbtCompound, NbtString, NbtString> func, params PropertyValueStateMapper[] propertiesNameMap)
	{
		OldName = oldName;
		_func = func;

		foreach (var map in propertiesNameMap)
		{
			ValuesMap.Add(map.OldName, map);
		}
	}

	public NbtString Resolve(string oldName, NbtCompound properties, NbtString property)
	{
		return _func?.Invoke(oldName, properties, property)
				?? new NbtString(NewName ?? property.Name, ValuesMap.GetValueOrDefault(property.StringValue)?.Resolve(oldName, properties) ?? property.StringValue);
	}

	public PropertyStateMapper Clone()
	{
		return new PropertyStateMapper(
			OldName,
			(Func<string, NbtCompound, NbtString, NbtString>) _func?.Clone(),
			ValuesMap.Values.Select(v => v.Clone()).ToArray())
		{
			NewName = NewName
		};
	}
}