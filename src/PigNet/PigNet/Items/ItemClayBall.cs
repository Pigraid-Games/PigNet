namespace PigNet.Items;

public partial class ItemClayBall
{
	public Item GetSmelt()
	{
		return new ItemBrick();
	}
}