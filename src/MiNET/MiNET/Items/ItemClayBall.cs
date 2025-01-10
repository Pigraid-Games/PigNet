namespace MiNET.Items;

public class ItemClayBall() : Item("minecraft:clay_ball", 337, canInteract: false)
{
	public override Item GetSmelt()
	{
		return new ItemBrick();
	}
}