
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