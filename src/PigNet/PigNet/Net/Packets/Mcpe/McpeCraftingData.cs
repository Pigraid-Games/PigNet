using PigNet.Net.Crafting;

namespace PigNet.Net.Packets.Mcpe;

public class McpeCraftingData : Packet<McpeCraftingData>
{
	public Recipes craftingEntries;
	public PotionTypeRecipe[] potionMixes;
	public PotionContainerChangeRecipe[] containerMixes;
	public MaterialReducerRecipe[] materialReducers;
	public bool clearRecipes;


	public McpeCraftingData()
	{
		Id = 0x34;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(craftingEntries);
		Write(potionMixes);
		Write(containerMixes);
		WriteUnsignedVarInt(0); // Material Reducers
		Write(clearRecipes);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();

		craftingEntries = ReadRecipes();
		potionMixes = ReadPotionTypeRecipes();
		containerMixes = ReadPotionContainerChangeRecipes();
		materialReducers = ReadMaterialReducerRecipes();
		clearRecipes = ReadBool();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		craftingEntries = default;
		potionMixes = default;
		containerMixes = default;
		materialReducers = default;
		clearRecipes = default;
	}
}