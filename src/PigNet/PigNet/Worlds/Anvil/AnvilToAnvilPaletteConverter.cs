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
using fNbt;
using log4net;
using PigNet.Worlds.Anvil.Mapping;

namespace PigNet.Worlds.Anvil;

public class AnvilToAnvilPaletteConverter
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(AnvilToAnvilPaletteConverter));

	private static readonly List<AnvilVersionedStateMapper> _mappers = new List<AnvilVersionedStateMapper>();

	static AnvilToAnvilPaletteConverter()
	{
		var _3693_mapper = new AnvilVersionedStateMapper(3693); // Java Edition 1.20.3 Pre-Release 1
		_mappers.Add(_3693_mapper);

		_3693_mapper.Mapper.Add(new BlockStateMapper("minecraft:grass", "minecraft:short_grass"));


		var _4325_mapper = new AnvilVersionedStateMapper(4325); // Java Edition 1.21.5 ?? maybe it's worth finding a pre-release version?
		_mappers.Add(_4325_mapper);

		//minecraft:creaking_heart
		_4325_mapper.Mapper.Add(new BlockStateMapper("minecraft:creaking_heart",
			new PropertyStateMapper("active", "creaking_heart_state",
				new PropertyValueStateMapper("true", "awake"),
				new PropertyValueStateMapper("false", "dormant"))));
	}

	public static void MapStates(NbtCompound palette, int dataVersion)
	{
		var name = palette["Name"].StringValue;
		var properties = palette["Properties"] as NbtCompound;

		var context = new BlockStateMapperContext(name, properties ?? new NbtCompound("Properties"));

		foreach (var mapper in _mappers)
		{
			if (dataVersion < mapper.Version)
			{
				name = mapper.Mapper.Resolve(context);
			}
		}

		palette["Name"] = new NbtString("Name", name);

		if (properties == null && context.Properties.Any())
		{
			palette.Add(context.Properties);
		}
	}

	private class AnvilVersionedStateMapper
	{
		public int Version { get; set; }

		public StateMapper Mapper { get; set; } = new StateMapper();

		public AnvilVersionedStateMapper(int version)
		{
			Version = version;
		}
	}
}