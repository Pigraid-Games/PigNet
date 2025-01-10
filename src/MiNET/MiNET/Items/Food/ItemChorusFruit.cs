using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items.Food;

public class ItemChorusFruit() : FoodItem("minecraft:chorus_fruit", 432, 0, 4, 2.4)
{
	public override Item GetSmelt()
	{
		return new ItemPoppedChorusFruit();
	}

	protected override void Consume(Player player)
	{
		base.Consume(player);

		var random = new Random();
		var currentPosition = player.KnownPosition;
		Vector3 teleportationCoordinates;

		for (int attempts = 0; attempts < 10; attempts++)
		{
			int offsetX = random.Next(-32, 32);
			int offsetY = random.Next(-32, 32);
			int offsetZ = random.Next(-32, 32);
			
			teleportationCoordinates = new Vector3(
				currentPosition.X + offsetX,
				currentPosition.Y + offsetY,
				currentPosition.Z + offsetZ
			);
			
			if (IsValidTeleportationDestination(teleportationCoordinates, player))
			{
				player.Teleport(new PlayerLocation(currentPosition.X + offsetX, currentPosition.Y + offsetY, currentPosition.Z + offsetZ));
				return;
			}
		}
	}

	private bool IsValidTeleportationDestination(Vector3 coordinates, Player player)
	{
		var world = player.Level;
		for (int y = (int) coordinates.Y; y >= 0; y--)
		{
			var block = world.GetBlock(new Vector3(coordinates.X, y, coordinates.Z));

			if (block.IsBlockingSkylight)
			{
				var blockAbove = world.GetBlock(new Vector3(coordinates.X, y + 1, coordinates.Z));
				var blockAboveHead = world.GetBlock(new Vector3(coordinates.X, y + 2, coordinates.Z));

				if (blockAbove.Id == 0 && blockAboveHead.Id == 0)
				{
					return true;
				}
			}
		}

		return false; ;
	}
}