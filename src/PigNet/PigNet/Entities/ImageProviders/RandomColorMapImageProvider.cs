﻿#region LICENSE

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

using PigNet.Net;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;

namespace PigNet.Entities.ImageProviders
{
	public class RandomColorMapImageProvider : IMapImageProvider
	{
		private WannabeRandom _random = new();

		public virtual byte[] GetData(MapInfo mapInfo, bool forced)
		{
			return GenerateColors(mapInfo, (byte) _random.Next(255));
		}

		public virtual McpeClientboundMapItemData GetClientboundMapItemData(MapInfo mapInfo)
		{
			return null;
		}

		public virtual McpeWrapper GetBatch(MapInfo mapInfo, bool forced)
		{
			return null;
		}

		private byte[] GenerateColors(MapInfo map, byte next)
		{
			byte[] bytes = new byte[map.Col * map.Row * 4];

			int i = 0;
			for (byte y = 0; y < map.Col; y++)
			{
				for (byte x = 0; x < map.Row; x++)
				{
					bytes[i++] = (byte) _random.Next(255); // R
					bytes[i++] = (byte) _random.Next(255); // G
					bytes[i++] = (byte) _random.Next(255); // B
					bytes[i++] = 0xff; // A
				}
			}

			return bytes;
		}
	}
}