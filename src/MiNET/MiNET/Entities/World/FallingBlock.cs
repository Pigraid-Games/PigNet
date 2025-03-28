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

using System;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Net.Packets.Mcpe;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class FallingBlock : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(FallingBlock));


		private readonly int _original;
		private bool _checkPosition = true;

		public FallingBlock(Level level, int original) : base(EntityType.FallingBlock, level)
		{
			_original = original;
			//Gravity = 0.04;
			Height = Width = Length = 0.98;
			Velocity = new Vector3(0, -0.0392f, 0);
			NoAi = false;
			HasCollision = true;
			IsAffectedByGravity = true;

			Gravity = 0.04;
			Drag = 0.02;
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[(int) MetadataFlags.Variant] = new MetadataInt(_original);

			return metadata;
		}

		public override void SpawnToPlayers(Player[] players)
		{
			foreach (Player player in players)
			{
				McpeUpdateBlockSynced updateBlock = McpeUpdateBlockSynced.CreateObject();
				updateBlock.blockPosition = (BlockCoordinates) KnownPosition;
				updateBlock.blockRuntimeId = (uint) new Air().GetRuntimeId();
				updateBlock.flags = 3;
				updateBlock.dataLayerId = 0;
				updateBlock.runtimeEntityId = EntityId;
				updateBlock.runtimeEntitySyncMessageId = 1;

				McpeAddActor addActor = McpeAddActor.CreateObject();
				addActor.entityType = EntityTypeId;
				addActor.entityIdSelf = EntityId;
				addActor.runtimeEntityId = EntityId;
				addActor.x = KnownPosition.X;
				addActor.y = KnownPosition.Y;
				addActor.z = KnownPosition.Z;
				addActor.pitch = KnownPosition.Pitch;
				addActor.yaw = KnownPosition.Yaw;
				addActor.headYaw = KnownPosition.HeadYaw;
				addActor.metadata = GetMetadata();
				addActor.speedX = Velocity.X;
				addActor.speedY = Velocity.Y;
				addActor.speedZ = Velocity.Z;
				addActor.attributes = GetEntityAttributes();

				player.SendPacket(updateBlock);
				player.SendPacket(addActor);
			}
		}

		public override void OnTick(Entity[] entities)
		{
			PositionCheck();

			if (KnownPosition.Y > -1 && _checkPosition)
			{
				Velocity -= new Vector3(0, (float) Gravity, 0);
				Velocity *= (float) (1.0f - Drag);
			}
			else
			{
				var updateBlock = McpeUpdateBlockSynced.CreateObject();
				updateBlock.blockPosition = new BlockCoordinates(KnownPosition);
				updateBlock.blockRuntimeId = (uint) _original;
				updateBlock.flags = 3;
				updateBlock.dataLayerId = 0;
				updateBlock.runtimeEntityId = EntityId;
				updateBlock.runtimeEntitySyncMessageId = 2;

				Level.RelayBroadcast(updateBlock);

				DespawnEntity();

				var blockState = BlockFactory.BlockPalette[_original];
				var block = BlockFactory.GetBlockById(blockState.Id);
				block.SetState(blockState.States);
				block.Coordinates = (BlockCoordinates) KnownPosition;

				Level.SetBlock(block, false);
			}
		}

		//private void PositionCheck()
		//{
		//	if (Velocity.Y < -0.001)
		//	{
		//		int distance = (int) Math.Ceiling(Velocity.Length());
		//		BlockCoordinates check = new BlockCoordinates(KnownPosition);
		//		for (int i = 0; i < distance; i++)
		//		{
		//			if (Level.GetBlock(check).IsSolid)
		//			{
		//				_checkPosition = false;
		//				KnownPosition = check.BlockUp();
		//				return;
		//			}
		//			check = check.BlockDown();
		//		}
		//	}

		//	KnownPosition.X += (float) Velocity.X;
		//	KnownPosition.Y += (float) Velocity.Y;
		//	KnownPosition.Z += (float) Velocity.Z;
		//}

		private void PositionCheck()
		{
			if (Velocity.Y < -0.001)
			{
				int distance = (int) Math.Ceiling(Velocity.Length());
				var check = new BlockCoordinates(KnownPosition);
				for (int i = 0; i < distance; i++)
				{
					if (Level.GetBlock(check).IsSolid)
					{
						_checkPosition = false;
						KnownPosition = check.BlockUp();
						return;
					}
					check = check.BlockDown();
				}
			}

			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;
		}
	}
}