using PigNet.Blocks;

namespace PigNet.Items;

public partial class ItemWheatSeeds : ItemBlock
{
	public ItemWheatSeeds()
	{
		Block = new Wheat();
	}
}