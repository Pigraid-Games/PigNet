using fNbt;

namespace PigNet.Items;

public class ItemMap : ItemFilledMap;

public partial class ItemFilledMap
{
	public ItemFilledMap()
	{
		MaxStackSize = 1;
	}

	public long MapId
	{
		get => ExtraData == null ? 0 : ExtraData["map_uuid"]!.LongValue;
		set => ExtraData = new NbtCompound("tag") { new NbtLong("map_uuid", value) };
	}
}