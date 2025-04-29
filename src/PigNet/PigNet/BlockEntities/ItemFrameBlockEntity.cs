using System;
using System.Collections.Generic;
using fNbt.Serialization;
using PigNet.Items;

namespace PigNet.BlockEntities;

public class ItemFrameBlockEntity : BlockEntity
{
	public Item Item { get; set; }

	[NbtProperty("ItemRotation")]
	public float Rotation { get; set; }

	[NbtProperty("ItemDropChance")]
	public float DropChance { get; set; } = 1f;

	public ItemFrameBlockEntity() : base(BlockEntityIds.ItemFrame)
	{
		Item = new ItemAir();
	}

	/// <summary>
	/// Set the rotation value from 0 to 7
	/// </summary>
	/// <param name="rotation"></param>
	public void SetLagacyRotation(int rotation)
	{
		if (rotation < 0 || rotation > 7)
		{
			rotation = 0;
		}

		Rotation = rotation * 45;
	}

	/// <summary>
	/// Get the rotation value from 0 to 7
	/// </summary>
	/// <param name="rotation"></param>
	public int GetLagacyRotation()
	{
		return (int)Math.Clamp(Math.Floor(Rotation / 45), 0, 7);
	}

	public override List<Item> GetDrops()
	{
		return [Item];
	}
}