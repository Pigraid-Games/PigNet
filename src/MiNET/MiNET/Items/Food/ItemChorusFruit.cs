using System;
using System.Numerics;
using MiNET.Blocks;
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
		PlayerLocation currentPosition = player.KnownPosition;
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
		Level world = player.Level;
		for (int y = (int) coordinates.Y; y >= 0; y--)
		{
			Block block = world.GetBlock(new Vector3(coordinates.X, y, coordinates.Z));

			if (block.IsBlockingSkylight)
			{
				Block blockAbove = world.GetBlock(new Vector3(coordinates.X, y + 1, coordinates.Z));
				Block blockAboveHead = world.GetBlock(new Vector3(coordinates.X, y + 2, coordinates.Z));

				if (blockAbove.Id == 0 && blockAboveHead.Id == 0) return true;
			}
		}

		return false;
		;
	}
}