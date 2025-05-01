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

namespace PigNet.Worlds.Anvil.Mapping;

public class StateMapper
{
	private readonly Dictionary<string, BlockStateMapper> _map = new Dictionary<string, BlockStateMapper>();
	private readonly List<BlockStateMapper> _defaultMap = new List<BlockStateMapper>();

	public void Add(BlockStateMapper map)
	{
		Add(map.OldName, map);
	}

	public void Add(string name, BlockStateMapper map)
	{
		_map.Add(name, map);
	}

	public bool TryAdd(BlockStateMapper map)
	{
		return TryAdd(map.OldName, map);
	}

	public bool TryAdd(string name, BlockStateMapper map)
	{
		return _map.TryAdd(name, map);
	}

	public void AddDefault(BlockStateMapper map)
	{
		_defaultMap.Add(map);
	}

	public string Resolve(BlockStateMapperContext context)
	{
		if (_map.TryGetValue(context.OldName, out var map))
		{
			context.OldName = map.Resolve(context);
		}

		foreach (var defMap in _defaultMap)
		{
			defMap.ResolveDefault(context);
		}

		return context.OldName;
	}
}