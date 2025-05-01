using System;
using System.Collections.Generic;
using System.Linq;
using fNbt.Serialization;
using fNbt.Serialization.NamingStrategy;
using PigNet.Utils.Vectors;

namespace PigNet.Worlds.Anvil.Data;

	[NbtObject]
	public class LevelInfo : ICloneable
	{
		[NbtProperty("version")]
		public int NbtVersion { get; set; }

		public int DataVersion { get; set; }

		public VersionInfo Version { get; set; }

		public bool WasModded { get; set; }

		public string LevelName { get; set; }


		[NbtFlatProperty(typeof(SpawnNamingStrategy))]
		public BlockCoordinates Spawn { get; set; }
		public float SpawnAngle { get; set; }


		public long Time { get; set; }
		public long DayTime { get; set; }
		public Difficulty Difficulty { get; set; }
		public bool DifficultyLocked { get; set; }
		public long LastPlayed { get; set; }
		public bool MapFeatures { get; set; }


		[NbtProperty("allowCommands")]
		public bool AllowCommands { get; set; }
		public Dictionary<string, string> GameRules { get; set; }
		public int GameType { get; set; }


		[NbtProperty("hardcore")]
		public bool Hardcore { get; set; }

		[NbtFlatProperty]
		public BorderInfo BorderInfo { get; set; }

		#region Generation

		[NbtProperty("WorldGenSettings")]
		public WorldGenerationSettings GenerationSettings { get; set; }

		[NbtProperty("generatorName")]
		public string GeneratorName { get; set; }

		[NbtProperty("generatorOptions")]
		public string GeneratorOptions { get; set; }

		[NbtProperty("generatorVersion")]
		public int GeneratorVersion { get; set; }

		[NbtProperty("initialized")]
		public bool Initialized { get; set; }

		#endregion

		#region Weather

		[NbtProperty("clearWeatherTime")]
		public int ClearWeatherTime { get; set; }

		[NbtProperty("raining")]
		public bool Raining { get; set; }

		[NbtProperty("rainTime")]
		public int RainTime { get; set; }

		[NbtProperty("thundering")]
		public bool Thundering { get; set; }

		[NbtProperty("thunderTime")]
		public int ThunderTime { get; set; }

		#endregion

		// DataPacks

		// DimensionData

		// Player

		// WanderingTrader info

		public object Clone()
		{
			var clone = (LevelInfo) MemberwiseClone();
			clone.Version = (VersionInfo) Version.Clone();
			clone.GameRules = GameRules.ToDictionary();
			clone.BorderInfo = (BorderInfo) BorderInfo.Clone();
			clone.GenerationSettings = (WorldGenerationSettings) GenerationSettings.Clone();

			return clone;
		}

		private class SpawnNamingStrategy : NbtNamingStrategy
		{
			public override string ResolveMemberName(string name)
			{
				return $"Spawn{name}";
			}
		}
	}