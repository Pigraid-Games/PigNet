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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeInventoryTransaction : Packet<McpeInventoryTransaction>
{
	public enum CraftingAction
	{
		CraftAddIngredient = -2,
		CraftRemoveIngredient = -3,
		CraftResult = -4,
		CraftUseIngredient = -5,
		AnvilInput = -10,
		AnvilMaterial = -11,
		AnvilResult = -12,
		AnvilOutput = -13,
		EnchantItem = -15,
		EnchantLapis = -16,
		EnchantResult = -17,
		Drop = -100
	}

	public enum InventorySourceType
	{
		Container = 0,
		Global = 1,
		WorldInteraction = 2,
		Creative = 3,
		Crafting = 100,
		Unspecified = 99999
	}

	public enum ItemReleaseAction
	{
		Release = 0,
		Use = 1
	}

	public enum ItemUseAction
	{
		Place, Clickblock = 0,
		Use, Clickair = 1,
		Destroy = 2
	}

	public enum ItemUseOnEntityAction
	{
		Interact = 0,
		Attack = 1,
		ItemInteract = 2
	}

	public enum TransactionType
	{
		Normal = 0,
		InventoryMismatch = 1,
		ItemUse = 2,
		ItemUseOnEntity = 3,
		ItemRelease = 4
	}

	public enum TriggerType
	{
		Unknown = 0,
		PlayerInput = 1,
		SimulationTick = 2
	}

	public Transaction transaction;

	public McpeInventoryTransaction()
	{
		Id = 0x1e;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(transaction);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		transaction = ReadTransaction();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		transaction = default;
	}
}