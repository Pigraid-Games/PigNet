
namespace PigNet.Net.Packets.Mcpe;

public class McpeStructureTemplateDataRequest : Packet<McpeStructureTemplateDataRequest>
{
	// TODO: Implement: https://mojang.github.io/bedrock-protocol-docs/html/StructureTemplateDataRequestPacket.html
	// TODO: https://mojang.github.io/bedrock-protocol-docs/html/StructureSettings.html
	public McpeStructureTemplateDataRequest()
	{
		Id = 0x84;
		IsMcpe = true;
	}
}