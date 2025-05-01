
using PigNet.Utils;

namespace PigNet.Net.Packets.Mcpe;

public class McpeItemStackRequest : Packet<McpeItemStackRequest>
{
	public enum ActionType
	{
		Take = 0,
		Place = 1,
		Swap = 2,
		Drop = 3,
		Destroy = 4,
		Consume = 5,
		Create = 6,
		PlaceIntoBundleDeprecated = 7,
		TakeFromBundleDeprecated = 8,
		LabTableCombine = 9,
		BeaconPayment = 10,
		MineBlock = 11,
		CraftRecipe = 12,
		CraftRecipeAuto = 13,
		CraftCreative = 14,
		CraftRecipeOptional = 15,
		CraftGrindstone = 16,
		CraftLoom = 17,
		CraftNotImplementedDeprecated = 18,
		CraftResultsDeprecated = 19
	}

	public ItemStackRequests requests;

	public McpeItemStackRequest()
	{
		Id = 0x93;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(requests);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		requests = ReadItemStackRequests();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		requests = default;
	}
}