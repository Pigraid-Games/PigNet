using fNbt;

namespace PigNet.Items;

public partial class ItemMap : ItemFilledMap;

public partial class ItemFilledMap
{
	public long MapId
	{
		get
		{
			return ExtraData == null ? 0 : ExtraData["map_uuid"]!.LongValue;
		}
		set { ExtraData = new NbtCompound("tag") {new NbtLong("map_uuid", value)}; }
	}

	public ItemFilledMap()
	{
		MaxStackSize = 1;
	}
}