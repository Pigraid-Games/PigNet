#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

namespace PigNet.BlockEntities;

public interface ICustomBlockEntityFactory
{
	BlockEntity GetBlockEntityById(string blockEntityId);
}

public static class BlockEntityFactory
{
	public static ICustomBlockEntityFactory CustomBlockEntityFactory { get; set; }

	public static BlockEntity GetBlockEntityById(string blockEntityId)
	{
		BlockEntity blockEntity = CustomBlockEntityFactory?.GetBlockEntityById(blockEntityId);

		if (blockEntity != null) return blockEntity;

		blockEntity = blockEntityId switch
		{
			"Sign" => new SignBlockEntity(),
			"Chest" => new ChestBlockEntity(),
			"EnchantTable" => new EnchantingTableBlockEntity(),
			"Furnace" => new FurnaceBlockEntity(),
			"BlastFurnace" => new BlastFurnaceBlockEntity(),
			"Skull" => new SkullBlockEntity(),
			"ItemFrame" => new ItemFrameBlockEntity(),
			"Bed" => new BedBlockEntity(),
			"Banner" => new BannerBlockEntity(),
			"FlowerPot" => new FlowerPotBlockEntity(),
			"Beacon" => new BeaconBlockEntity(),
			"MobSpawner" => new MobSpawnerBlockEntity(),
			"ShulkerBox" => new ShulkerBoxBlockEntity(),
			"StructureBlock" => new StructureBlockBlockEntity(),
			_ => null
		};

		return blockEntity;
	}
}