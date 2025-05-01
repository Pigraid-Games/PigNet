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

using System.Linq;
using fNbt;
using PigNet.BlockEntities;

namespace PigNet.Worlds.Anvil;

public class AnvilToBedrockBlockEntityConverter
	{
		public static NbtCompound Convert(NbtCompound origin)
		{
			var tag = origin.Clone() as NbtCompound;

			var id = tag["id"].StringValue.Split(':').Last();

			switch (id)
			{
				case "banner":
					var patterns = tag["patterns"] as NbtList;
					if (patterns != null)
					{
						patterns.Name = "Patterns";
						foreach (NbtCompound pattern in patterns)
						{
							pattern["color"].Name = "Color";
							pattern["pattern"].Name = "Pattern";
						}
					}

					break;
				case "chest":
				case "trapped_chest":
				case "barrel":
				case "shulker_box":
				case "dispenser":
				case "dropper":
				case "hopper":
					// TODO
					break;
				case "beacon":
					// TODO
					break;
				case "beehive":
					// TODO
					break;
				case "furnace":
				case "blast_furnace":
				case "smoker":
					// TODO
					break;
				case "brewing_stand":
					// TODO
					break;
				case "campfire":
					// TODO
					break;
				case "sculk_sensor":
				case "calibrated_sculk_sensor":
					// TODO
					break;
				case "command_block":
					// it is impossible to map commands unambiguously
					break;
				case "conduit":
					// TODO (how to map UUID to Long?)
					break;
				case "decorated_pot":
					// TODO - item
				case "enchanting_table":
					id = "EnchantTable";
					break;
				case "end_gateway":
					tag["Age"] = new NbtLong("Age", tag["Age"].LongValue);

					if (tag.Contains("exit_portal"))
					{
						var exit = tag["exit_portal"].IntArrayValue;
						tag["ExitPortal"] = new NbtCompound("ExitPortal")
						{
							new NbtInt("X", exit[0]),
							new NbtInt("Y", exit[1]),
							new NbtInt("Z", exit[2]),
						};
					}

					break;
				case "hanging_sign":
				case "sign":
					tag["is_waxed"].Name = "IsWaxed";
					ConvertSignText(tag["front_text"]);
					ConvertSignText(tag["back_text"]);
					tag["front_text"].Name = "FrontText";
					tag["back_text"].Name = "BackText";

					void ConvertSignText(NbtTag text)
					{
						text["has_glowing_text"].Name = "IgnoreLighting";

						var lines = (text["messages"] as NbtList).Select(t => string.IsNullOrEmpty(t.StringValue) ? string.Empty : t.StringValue.Substring(1, t.StringValue.Length - 2));
						text["Text"] = new NbtString("Text", string.Join("\n", lines.Where(l => !string.IsNullOrEmpty(l))));

						if (SignColor.TryParse(text["color"].StringValue, out var color))
						{
							text["SignTextColor"] = new NbtInt("SignTextColor", color.ToArgb());
						}
					}
					break;
				case "jigsaw":
					id = "JigsawBlock";
					// TODO
					break;
				case "jukebox":
					// TODO - item
					break;
				case "lectern":
					if (tag.Contains("Page"))
					{
						tag["Page"].Name = "page";
					}
					// TODO - book
					break;
				case "mob_spawner":
					// TODO - entity
					break;
				case "piston":
					// TODO - may be MovingBlock
					break;
				case "sculk_catalyst":
					// TODO
					break;
				case "skulk_shrieker":
					// TODO
					break;
				case "skull":
					// nothing?
					break;
				case "structure_block":
					// TODO
					break;
				case "brushable_block":
					// TODO
					break;
				case "trial_spawner":
					// TODO
					break;
				case "vault":
					// TODO
					break;
			}

			id = ConvertId(id);
			tag["id"] = new NbtString("id", id);

			return tag;
		}

		private static string ConvertId(string id)
		{
			var splitId = id.Split('_');

			return string.Join("", splitId.Select(p => p.First().ToString().ToUpperInvariant() + p.Substring(1)));
		}
	}