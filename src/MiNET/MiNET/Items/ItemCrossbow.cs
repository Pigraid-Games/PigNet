using System;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemCrossbow : Item
	{
		public ItemCrossbow() : base("minecraft:crossbow", 471)
		{
			MaxStackSize = 1;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			base.UseItem(world, player, blockCoordinates);
			Console.WriteLine("Called use item");
		}


	}
}
