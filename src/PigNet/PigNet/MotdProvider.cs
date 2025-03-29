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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Net;
using PigNet.BlockEntities;
using PigNet.Net;
using PigNet.Net.RakNet;
using PigNet.Utils;

namespace PigNet
{
	public class MotdProvider
	{
		public string Motd { get; set; }

		public string SecondLine { get; set; }

		public int MaxNumberOfPlayers { get; set; }

		public int NumberOfPlayers { get; set; }

		public long ServerId { get; set; }

		public string GameMode { get; set; }
		public List<String> Motds { get; set; } = new List<String>();

		private int clock = 0;
		public MotdProvider()
		{
			byte[] buffer = new byte[8];
			new Random().NextBytes(buffer);
			buffer[7] = 0;
			ServerId = BitConverter.ToInt64(buffer, 0);

			ServerId = Config.GetProperty("serverid", ServerId);
			SecondLine = Config.GetProperty("motd-2nd", "PigNet");
			GameMode = Config.GetProperty("gamemode", "Survival");
			var motdData = Config.GetProperty("motd", "PigNet Server");
			foreach (string motd in motdData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
			{
				Motds.Add(motd);
			}

		}

		public virtual string GetMotd(ConnectionInfo connectionInfo, IPEndPoint caller, bool eduMotd = false)
		{
			NumberOfPlayers = connectionInfo.NumberOfPlayers;
			MaxNumberOfPlayers = connectionInfo.MaxNumberOfPlayers;

			ulong serverId = (ulong) ServerId;

			var protocolVersion = McpeProtocolInfo.ProtocolVersion.ToString();

			if(clock/10 == Motds.Count)
			{
				clock = 0;
			}
			Motd = Motds[clock / 10];
			if (DateTime.Now.Second % 5 == 0)
			{
				clock++;
			}
			return string.Format($"{"MCPE"};{Motd};{protocolVersion};{McpeProtocolInfo.GameVersion};{NumberOfPlayers};{MaxNumberOfPlayers};{serverId};{SecondLine};{GameMode};");
		}
	}
}