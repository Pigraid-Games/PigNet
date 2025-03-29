using PigNet.Net;
using System;
using PigNet.Net.Packets.Mcpe;
using PigNet.Worlds;

namespace PigNet
{
	public class ExperienceManager(Player player)
	{
		public Player Player { get; set; } = player;
		public float ExperienceLevel { get; set; }
		public float Experience { get; set; }

		public void AddExperience(float xp, bool send = true)
		{
			float xpToNextLevel = GetXpToNextLevel();

			if (xp + Experience < xpToNextLevel) Experience += xp;
			else
			{
				float expDiff = Experience + xp - xpToNextLevel;
				ExperienceLevel++;
				Experience = 0;
				AddExperience(expDiff, false);
			}

			if (send) SendAttributes();
		}

		public void RemoveExperienceLevels(float levels)
		{
			float currentXp = CalculateXp();
			ExperienceLevel = Experience - Math.Abs(levels);
			Experience = GetXpToNextLevel() * currentXp;
		}

		public virtual float GetXpToNextLevel()
		{
			return ExperienceLevel switch
			{
				>= 0 and <= 15 => 2 * ExperienceLevel + 7,
				> 15 and <= 30 => 5 * ExperienceLevel - 28,
				_ => 9 * ExperienceLevel - 158
			};
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

			McpeUpdateAttributes attributesPackage = McpeUpdateAttributes.CreateObject();
			attributesPackage.runtimeEntityId = EntityManager.EntityIdSelf;
			attributesPackage.attributes = attributes;
			Player.SendPacket(attributesPackage);
		}
	}
}