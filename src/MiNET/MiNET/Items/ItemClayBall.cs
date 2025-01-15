namespace MiNET.Items;

public class ItemClayBall() : Item("minecraft:clay_ball", canInteract: false)
{
	public override Item GetSmelt()
	{
		return new ItemBrick();
	}
}