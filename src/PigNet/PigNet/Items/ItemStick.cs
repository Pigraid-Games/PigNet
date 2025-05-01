using System;
using System.Numerics;
using System.Threading.Tasks;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemStick
{
	public ItemStick()
	{
		FuelEfficiency = 5;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		if (player.IsGliding)
		{
			double currentSpeed = player.CurrentSpeed / 20f;
			if (currentSpeed > 35f / 20f) return;

			Vector3 velocity = Vector3.Normalize(player.KnownPosition.GetHeadDirectionVector()) * (float) currentSpeed;
			float factor = (float) (1 + 1 / (1 + currentSpeed * 2));
			velocity *= factor;

			if (currentSpeed < 7f / 20f) velocity = Vector3.Normalize(velocity) * 1.2f;

			McpeSetActorMotion motions = McpeSetActorMotion.CreateObject();
			motions.runtimeActorId = EntityManager.EntityIdSelf;
			motions.velocity = velocity;

			player.SendPacket(motions);
		}
		else if (player.Inventory.Chest is ItemElytra)
		{
			McpeSetActorMotion motions = McpeSetActorMotion.CreateObject();
			motions.runtimeActorId = EntityManager.EntityIdSelf;
			var velocity = new Vector3(0, 2, 0);
			motions.velocity = velocity;
			player.SendPacket(motions);

			_ = SendWithDelay(200, () =>
			{
				player.IsGliding = true;
				player.Height = 0.6;
				player.BroadcastSetEntityData();
			});
		}
	}

	private static async Task SendWithDelay(int delay, Action action)
	{
		await Task.Delay(delay);
		action();
	}
}