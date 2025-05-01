namespace PigNet.Items;

public partial class ItemKelp
{
	public Item GetSmelt()
	{
		return new ItemDriedKelp();
	}
}