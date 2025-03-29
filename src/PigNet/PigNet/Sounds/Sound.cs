#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using PigNet.Net;
using PigNet.Net.Packets.Mcpe;
using PigNet.Worlds;

namespace PigNet.Sounds;

public enum SoundType
{
	LevelEvent,
	LevelSoundEvent
}

public class Sound(short id, Vector3 position, int pitch = 0, SoundType soundType = SoundType.LevelEvent)
{
	public short Id { get; } = id;
	public int Pitch { get; set; } = pitch;
	public Vector3 Position { get; set; } = position;
	public SoundType SoundType { get; set; } = soundType;
	public int ExtraData { get; set; }


	public virtual void Spawn(Level level)
	{
		switch (SoundType)
		{
			case SoundType.LevelEvent:
			{
				McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
				levelEvent.eventId = (LevelEventType) Id;
				levelEvent.data = Pitch;
				levelEvent.position = Position;
				level.RelayBroadcast(levelEvent);
				break;
			}
			case SoundType.LevelSoundEvent:
			{
				McpeLevelSoundEvent levelSoundEvent = McpeLevelSoundEvent.CreateObject();
				levelSoundEvent.soundId = (uint) Id;
				levelSoundEvent.position = Position;
				levelSoundEvent.disableRelativeVolume = false;
				levelSoundEvent.extraData = ExtraData;
				level.RelayBroadcast(levelSoundEvent);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public virtual void SpawnToPlayers(Player[] players)
	{
		if (players == null) return;
		if (players.Length == 0) return;

		switch (SoundType)
		{
			case SoundType.LevelEvent:
			{
				McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
				levelEvent.eventId = (LevelEventType) Id;
				levelEvent.data = Pitch;
				levelEvent.position = Position;
				levelEvent.AddReferences(players.Length - 1);
				foreach (Player player in players) player.SendPacket(levelEvent);
				break;
			}
			case SoundType.LevelSoundEvent:
			{
				McpeLevelSoundEvent levelSoundEvent = McpeLevelSoundEvent.CreateObject();
				levelSoundEvent.soundId = (uint) Id;
				levelSoundEvent.position = Position;
				levelSoundEvent.disableRelativeVolume = false;
				levelSoundEvent.extraData = ExtraData;
				foreach (Player player in players) player.SendPacket(levelSoundEvent);
				break;
			}
		}

	}
}