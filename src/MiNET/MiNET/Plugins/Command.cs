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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MiNET.Entities;
using Newtonsoft.Json;

namespace MiNET.Plugins;

public class CommandSet : Dictionary<string, Command>;

public class Command
{
	[JsonIgnore] public string Name { get; set; }

	public Version[] Versions { get; set; }
}

public class Version
{
	[JsonProperty("version")] public int CommandVersion { get; set; }

	public string[] Aliases { get; set; }
	public string Description { get; set; }
	public string Permission { get; set; }
	public int CommandPermission { get; set; }
	public string ErrorMessage { get; set; }
	public bool RequiresTellPerms { get; set; }
	public bool RequiresChatPerms { get; set; }
	public bool OutputToSpeech { get; set; }

	[JsonProperty("requires_edu")] public bool RequiresEdu { get; set; }

	[JsonProperty("allows_indirect_exec")] public bool AllowsIndirectExec { get; set; }

	[JsonProperty("is_hidden")] public bool IsHidden { get; set; }

	public Dictionary<string, Overload> Overloads { get; set; }
}

public class Overload
{
	[JsonIgnore] public MethodInfo Method { get; set; }

	[JsonIgnore] public string Description { get; set; }

	public Input Input { get; set; }
	public Parser Parser { get; set; }
}

public class Input
{
	public Parameter[] Parameters { get; set; }
}

public class Output
{
	[JsonProperty("format_strings")] public FormatString[] FormatStrings { get; set; }

	public Parameter[] Parameters { get; set; }
}

public class FormatString
{
	public string Color { get; set; }
	public string Format { get; set; }

	[JsonProperty("params_to_use")] public string[] ParamsToUse { get; set; }

	[JsonProperty("should_show")] public FormatRule ShouldShow { get; set; }
}

public class FormatRule
{
	[JsonProperty("not_empty")] public string[] NotEmpty { get; set; }

	[JsonProperty("is_true")] public string[] IsTrue { get; set; }
}

public class Parser
{
	public string Tokens { get; set; }
}

public class Parameter
{
	public string Name { get; set; }
	public string Type { get; set; }

	[JsonProperty("enum_type")] public string EnumType { get; set; }

	[JsonProperty("enum_values")] public string[] EnumValues { get; set; }

	public bool Optional { get; set; }

	[JsonProperty("target_data")] public TargetData TargetData { get; set; }
}

public class TargetData
{
	[JsonProperty("players_only")] public bool PlayersOnly { get; set; }

	[JsonProperty("main_target")] public bool MainTarget { get; set; }

	[JsonProperty("allow_dead_players")] public bool AllowDeadPlayers { get; set; }
}

public class BlockPos
{
	public int X { get; set; }
	public bool XRelative { get; set; }

	public int Y { get; set; }
	public bool YRelative { get; set; }

	public int Z { get; set; }
	public bool ZRelative { get; set; }

	public override string ToString()
	{
		return $"{nameof(X)}: {X}, {nameof(XRelative)}: {XRelative}, {nameof(Y)}: {Y}, {nameof(YRelative)}: {YRelative}, {nameof(Z)}: {Z}, {nameof(ZRelative)}: {ZRelative}";
	}
}

public class EntityPos
{
	public double X { get; set; }
	public bool XRelative { get; set; }

	public double Y { get; set; }
	public bool YRelative { get; set; }

	public double Z { get; set; }
	public bool ZRelative { get; set; }

	public override string ToString()
	{
		return $"{nameof(X)}: {X}, {nameof(XRelative)}: {XRelative}, {nameof(Y)}: {Y}, {nameof(YRelative)}: {YRelative}, {nameof(Z)}: {Z}, {nameof(ZRelative)}: {ZRelative}";
	}
}

public class RelValue
{
	public double Value { get; set; }
	public bool Relative { get; set; }

	public override string ToString()
	{
		return $"{nameof(Value)}: {Value}, {nameof(Relative)}: {Relative}";
	}
}

public class Target
{
	private static readonly string[] Values = ["No valid target specified"];

	public Target(string input)
	{
		input = input.ToLower(); // Ensure input is lowercase

		if (MatchesSelector(input))
		{
			Selector = input[..(input.Contains('[') ? input.IndexOf('[') : input.Length)]; // Extract selector type
			ParseSelectorArguments(input);
		}
		else
			StringTarget = input;

		AddDefaultRules();
	}

	public Rule[] Rules { get; set; }
	public string Selector { get; set; }
	public Player[] Players { get; set; }
	public Entity[] Entities { get; set; }
	public string StringTarget { get; set; }
	public SelectorArgument[] SelectorArguments { get; set; }

	public void AddDefaultRules()
	{
		if (Rules == null || Rules.Length == 0)
			Rules =
			[
				new Rule
				{
					Inverted = false,
					Name = "type",
					Value = "player"
				}
			];

		// Ensure all rules are stored in lowercase
		foreach (Rule rule in Rules)
		{
			rule.Name = rule.Name.ToLower();
			rule.Value = rule.Value.ToLower();
		}
	}

	public void ParseSelectorArguments(string selectorString)
	{
		if (!selectorString.Contains('['))
			return;
		selectorString = selectorString.ToLower();
		string[] args = selectorString
			.Substring(selectorString.IndexOf('[') + 1,
				selectorString.LastIndexOf(']') - selectorString.IndexOf('[') - 1)
			.Split(',');

		SelectorArguments = args
			.Select(arg =>
			{
				string[] parts = arg.Split('=');
				return new SelectorArgument
				{
					Key = parts[0].ToLower(),
					Value = parts[1].ToLower()
				};
			})
			.ToArray();
	}

	public override string ToString()
	{
		var result = new List<string>();
		if (Players is { Length: > 0 })
			result.Add(string.Join(", ", Players.Select(p => p.Username.ToLower())));
		else if (!string.IsNullOrEmpty(StringTarget)) result.Add(StringTarget.ToLower());
		if (SelectorArguments is { Length: > 0 }) result.Add(string.Join(", ", SelectorArguments.Select(arg => arg.ToString())));
		return string.Join(" | ", result.Any() ? result : Values);
	}

	public static bool MatchesSelector(string selector)
	{
		return !string.IsNullOrEmpty(selector) && selector.StartsWith("@");
	}

	public class Rule
	{
		public bool Inverted { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}

public class SelectorArgument
{
	public string Key { get; set; }
	public string Value { get; set; }

	public override string ToString()
	{
		return $"{Key}={Value}";
	}
}

public enum SelectorType
{
	AllPlayers,
	NearestPlayer,
	RandomPlayer,
	AllEntities,
	Executor,
	Initiator
}

public abstract class SoftEnumBase;

public class TestSoftEnum : SoftEnumBase;

public abstract class EnumBase
{
	public string Value { get; set; }
}

// enchantmentType
public class EnchantmentTypeEnum : EnumBase;

// dimension
public class DimensionEnum : EnumBase;

// itemType
public class ItemTypeEnum : EnumBase;

// commandName
public class CommandNameEnum : EnumBase;

// entityType
public class EntityTypeEnum : EnumBase;

// blockType
public class BlockTypeEnum : EnumBase;

public class EffectEnum : EnumBase;

public class EnchantEnum : EnumBase;

public class FeatureEnum : EnumBase;

//"rules": [
//    {
//    "inverted": false,
//    "name": "name",
//    "value": "gurunx"
//	}
//],
//"selector": "nearestPlayer"