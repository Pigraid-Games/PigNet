using System.Collections.Generic;
using PigNet.Items;
using PigNet.Net;

namespace PigNet.Inventories;

public class CreativeInventoryGroup : IPacketDataObject
{
	public string Name { get; set; }

	public CreativeInventoryCategoryType CategoryType { get; set; }
		
	public Item IconItem { get; set; }

	public List<Item> Items { get; set; } = [];

	public void Write(Packet packet)
	{
		packet.Write((int) CategoryType);
		packet.Write(Name);
		packet.Write(IconItem, false);
	}

	public static CreativeInventoryGroup Read(Packet packet)
	{
		return new CreativeInventoryGroup
		{
			CategoryType = (CreativeInventoryCategoryType) packet.ReadInt(),
			Name = packet.ReadString(),
			IconItem = packet.ReadItem(false)
		};
	}
}