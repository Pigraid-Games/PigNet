
namespace PigNet.Net.Packets.Mcpe;

public class McpeDimensionData : Packet<McpeDimensionData>
{
	public DimensionDefinitions definitions;

	public McpeDimensionData()
	{
		Id = 0xb4;
		IsMcpe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(definitions);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		definitions = ReadDimensionDefinitions();
	}

	protected override void ResetPacket()
	{
		base.ResetPacket();

		definitions = default;
	}
}