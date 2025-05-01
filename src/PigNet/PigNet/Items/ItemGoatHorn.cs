using System;
using System.Collections.Generic;
using PigNet.Sounds;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemGoatHorn
{
	public enum GoatHornType
	{
		Ponder,
		Sing,
		Seek,
		Feel,
		Admire,
		Call,
		Yearn,
		Dream
	}

	private static readonly Dictionary<Player, DateTime> CooldownTracker = new();

	public ItemGoatHorn(GoatHornType goatHornType = GoatHornType.Ponder)
	{
		Metadata = (short) goatHornType;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (CooldownTracker.TryGetValue(player, out DateTime lastUsed))
		{
			TimeSpan timeSinceLastUse = DateTime.UtcNow - lastUsed;
			if (timeSinceLastUse.TotalSeconds < 7) return;
		}

		CooldownTracker[player] = DateTime.UtcNow;

		switch ((GoatHornType) Metadata)
		{
			case GoatHornType.Ponder:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall0);
				break;
			case GoatHornType.Sing:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall1);
				break;
			case GoatHornType.Seek:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall2);
				break;
			case GoatHornType.Feel:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall3);
				break;
			case GoatHornType.Admire:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall4);
				break;
			case GoatHornType.Call:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall5);
				break;
			case GoatHornType.Yearn:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall6);
				break;
			case GoatHornType.Dream:
				world.BroadcastSound(player.KnownPosition.ToVector3(), LevelSoundEventType.HornCall7);
				break;
			default:
				return;
		}
	}
}