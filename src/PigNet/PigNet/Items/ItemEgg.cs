using PigNet.Entities.Projectiles;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemEgg
{
	public ItemEgg()
	{
		MaxStackSize = 16;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		float force = 1.5f;

		Count--;
		var egg = new Egg(player, world)
		{
			KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
		};
		egg.KnownPosition.Y += 1.62f;
		egg.Velocity = egg.KnownPosition.GetDirectionVector().Normalize() * (force);
		egg.SpawnEntity();
	}
}