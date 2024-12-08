using MiNET.Net;
using MiNET.Worlds;
using System;

namespace MiNET
{
	public class ExperienceManager
	{
		public Player Player { get; set; }
		public float ExperienceLevel { get; set; } = 0f;
		public float Experience { get; set; } = 0f;

		public ExperienceManager(Player player)
		{
			Player = player;
		}

		public void AddExperience(float xp, bool send = true)
		{
			var xpToNextLevel = GetXpToNextLevel();

			if (xp + Experience < xpToNextLevel)
			{
				Experience += xp;
			}
			else
			{
				var expDiff = Experience + xp - xpToNextLevel;
				ExperienceLevel++;
				Experience = 0;
				AddExperience(expDiff, false);
			}

			if (send)
			{
				SendAttributes();
			}
		}

		public void RemoveExperienceLevels(float levels)
		{
			var currentXp = CalculateXp();
			ExperienceLevel = Experience - Math.Abs(levels);
			Experience = GetXpToNextLevel() * currentXp;
		}

		public virtual float GetXpToNextLevel()
		{
			if (ExperienceLevel >= 0 && ExperienceLevel <= 15)
			{
				return 2 * ExperienceLevel + 7;
			}
			else if (ExperienceLevel > 15 && ExperienceLevel <= 30)
			{
				return 5 * ExperienceLevel - 28;
			}
			else // Level > 30
			{
				return 9 * ExperienceLevel - 158;
			}
		}

		protected virtual float CalculateXp()
		{
			return Experience / GetXpToNextLevel();
		}

		public virtual PlayerAttributes AddExperienceAttributes(PlayerAttributes attributes)
		{
			attributes["minecraft:player.experience"] = new PlayerAttribute
			{
				Name = "minecraft:player.experience",
				MinValue = 0,
				MaxValue = 1,
				Value = CalculateXp(),
				Default = 0,
				Modifiers = new AttributeModifiers()
			};
			attributes["minecraft:player.level"] = new PlayerAttribute
			{
				Name = "minecraft:player.level",
				MinValue = 0,
				MaxValue = 24791,
				Value = ExperienceLevel,
				Default = 0,
				Modifiers = new AttributeModifiers()
			};
			return attributes;
		}

		public virtual void SendAttributes()
		{
			var attributes = new PlayerAttributes();
			attributes = AddExperienceAttributes(attributes);

			McpeUpdateAttributes attributesPackate = McpeUpdateAttributes.CreateObject();
			attributesPackate.runtimeEntityId = EntityManager.EntityIdSelf;
			attributesPackate.attributes = attributes;
			Player.SendPacket(attributesPackate);
		}
	}
}