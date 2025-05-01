using PigNet.Entities.Projectiles;
using PigNet.Utils.Vectors;
using PigNet.Worlds;

namespace PigNet.Items;

public partial class ItemSnowball
{
	public ItemSnowball()
	{
		MaxStackSize = 16;
	}

	public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
	{
		float force = 1.5f;

		var snowBall = new Snowball(player, world)
		{
			KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
		};
		snowBall.KnownPosition.Y += 1.62f;
		snowBall.Velocity = snowBall.KnownPosition.GetDirectionVector().Normalize() * force;
		snowBall.SpawnEntity();
	}
}