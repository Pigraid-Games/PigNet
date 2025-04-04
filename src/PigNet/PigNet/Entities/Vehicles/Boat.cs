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
using PigNet.Items;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils.Metadata;
using PigNet.Worlds;

namespace PigNet.Entities.Vehicles
{
	public class Boat : Vehicle
	{
		public Boat(Level level) : base(EntityType.Boat, level)
		{
			IsStackable = true;
			HasCollision = true;
			IsAffectedByGravity = true;
			Length = 1.4f;
			Height = 0.455f;
			NoAi = false;
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(333)
			};
		}

		public override void DoInteraction(int actionId, Player player)
		{
			player.Vehicle = EntityId;

			McpeSetActorLink link = McpeSetActorLink.CreateObject();
			link.linkType = ActorLinkType.Riding;
			link.riderId = player.EntityId;
			link.riddenId = EntityId;
			Level.RelayBroadcast(link);

			SendSetEntityData(player);
		}

		public void SendSetEntityData(Player player)
		{
			player.IsRiding = true;

			// FOR PLAYER
			MetadataDictionary metadata = player.GetMetadata();
			metadata[(int) MetadataFlags.RiderSeatPosition] = new MetadataVector3(0, 1.02001f, 0);
			metadata[(int) MetadataFlags.RiderRotationLocked] = new MetadataByte(1);
			metadata[(int) MetadataFlags.RiderMaxRotation] = new MetadataFloat(90f);
			metadata[(int) MetadataFlags.RiderMinRotation] = new MetadataFloat(-90f);
			metadata[(int) MetadataFlags.RiderRotationOffset] = new MetadataFloat(-90f);

			player.BroadcastSetEntityData(metadata);
		}
	}
}